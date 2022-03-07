using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : PhysicsObject
{
    public enum PlayerState {
        MOVE,
        PECK,
        CHARGE,
        FLAP,
        DEAD
    }
    private PlayerState playerState = PlayerState.MOVE;

    [Header("Parameters")]
    public float maxSpeed;
    public float chargingMaxSpeed;
    public float acceleration;
    public float tornadoSpawnDistance;
    public float peckDuration;
    public float flapDuration;

    [Header("References")]
    public PlayerAnimation playerAnimation;
    public Transform cameraFacingTransform;
    public Transform rotateTransform;
    public SpriteRenderer mainSprite;
    public GameObject peckHitbox;
    public Transform gustSpawnLocation;
    public GameObject gustPrefab;
    public GameObject tornadoPrefab;
    public TornadoMarker tornadoMarker;

    [Header("Materials")]
    public Material whiteMaterial;
    public Material dissolveMaterial;
    private Material normalMaterial;

    public PositionSO cameraPosition;

    Vector3 horizontalInput;
    Vector3 horizontalVelocity; // used for calculating vertical velocity

    private bool peckInput;
    private bool oldPeckInput;
    private bool gustInput;
    private bool oldGustInput;

    private bool inCutscene;

    //raycast variables
    public bool boxCastHit;
    public bool colliding;
    RaycastHit objectHit;


    protected override void Awake()
    {
        base.Awake();
        peckHitbox.SetActive(false);
        normalMaterial = mainSprite.material;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        WarpAltar.OnAltarWarp += PlayWarpAnimation;
        ScenesManager.OnTransition += OnSceneTransition;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        WarpAltar.OnAltarWarp -= PlayWarpAnimation;
        ScenesManager.OnTransition -= OnSceneTransition;
    }


    private void Update()
    {
        if (!inCutscene) {
            peckInput = Input.GetAxisRaw("Action1") > 0;
            gustInput = Input.GetAxisRaw("Action2") > 0;
        }
        else {
            peckInput = false;
            gustInput = false;
        }
    }

    protected override void FixedUpdate()
    {
        if (playerState == PlayerState.DEAD) {
            // stay stationary when dead
            SetRelativeVelocity(Vector3.zero);
            return;
        }

        GetHorizontalInput();
        base.FixedUpdate();

        Debug.DrawRay(transform.position, rigidbody.velocity, Color.cyan, Time.fixedDeltaTime);
        CalculateTornadoPlacement();

        if (playerState == PlayerState.MOVE && peckInput && !oldPeckInput)
        {
            Peck();
        }

        //started holding down tornado button
        if (playerState == PlayerState.MOVE && gustInput && !oldGustInput)
        {
            ChargeGust();
        }

        //just let go of tornado button
        if (playerState == PlayerState.CHARGE && !gustInput && oldGustInput)
        {
            Gust();
        }

        oldPeckInput = peckInput;
        oldGustInput = gustInput;
    }


    // fetches the player's directional input and rotates it so that it matches w the camera facing direction
    // it also sets the player's facing angle for pecking/flapping
    private void GetHorizontalInput()
    {
        if (inCutscene || (playerState != PlayerState.MOVE && playerState != PlayerState.CHARGE)) {
            // don't read any horizontal input
            horizontalInput = Vector3.zero;
            return;
        }

        Vector3 horizontalInputRaw = new Vector3(
            Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")
        );

        // rotate the input directions to point in the direction of the camera
        Vector3 cameraDir = cameraPosition.direction.WithY(0).normalized;
        Debug.DrawRay(transform.position, Vector3.Cross(cameraDir, Vector3.up), Color.green, Time.fixedDeltaTime);
        horizontalInput = horizontalInputRaw.x * -Vector3.Cross(cameraDir, Vector3.up) + horizontalInputRaw.z * cameraDir;
        horizontalInput = horizontalInput.normalized;

        if (horizontalInput != Vector3.zero)
        {
            // the player is moving, so update the facing direction for pecking/flapping by rounding it to the nearest axis
            // get the eight-way direction of the camera
            float cameraAngle = Vector3.SignedAngle(Vector3.forward, cameraDir, Vector3.up);
            float cameraAngleRounded = Helpers.RoundToNearest(cameraAngle, 45f);
            Vector3 cameraDirRounded = Quaternion.Euler(0, cameraAngleRounded, 0) * Vector3.forward;

            // get the four-way angle of the input
            float hInputAngle = Vector3.SignedAngle(Vector3.forward, horizontalInputRaw, Vector3.up);
            float hInputAngleRounded = Helpers.RoundToNearest(hInputAngle, 90f);

            // set our facing direction as the rounded camera direction rotated by the rounded input angle
            Vector3 facingDir = Quaternion.Euler(0, hInputAngleRounded, 0) * cameraDirRounded;
            rotateTransform.Rotate(0, Vector3.SignedAngle(rotateTransform.forward, facingDir, Vector3.up), 0);
        }
    }


    protected override void CheckForGround()
    {
        // this is for the most part identical to PhysicsObject.CheckForGround()
        groundNormal = Vector3.zero;
        groundRigidbody = null;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask))
        {
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.red, Time.fixedDeltaTime);

            // but the player requires an extra check for the slope angle
            if (Vector3.Dot(Vector3.up, hit.normal) > 0.65f)
            {
                groundY = hit.point.y;
                groundDistance = hit.distance - 0.11f;
                groundNormal = hit.normal;
                groundRigidbody = hit.rigidbody ?? hit.collider.GetComponentInParent<Rigidbody>();
            }
        }
    }

    protected override Vector3 HandleHorizontalMovement(Vector3 hVelocity)
    {
        // select max speed based on player state
        float theMaxSpeed = playerState == PlayerState.CHARGE ? chargingMaxSpeed : maxSpeed;

        // accelerate towards our input direction
        hVelocity = Vector3.MoveTowards(hVelocity, horizontalInput * theMaxSpeed, acceleration * Time.deltaTime);

        // update this member variable so that HandleVerticalMovement knows how fast we're moving
        horizontalVelocity = hVelocity;

        return hVelocity;
    }

    protected override float HandleVerticalMovement(float vVelocity)
    {
        vVelocity = base.HandleVerticalMovement(vVelocity);

        if (currentTornado == null && groundNormal != Vector3.zero)
        {
            // we could be on a slope. aim velocity up or down the slope
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, horizontalVelocity.normalized);
            if (groundNormalUp.magnitude != 0) {
                vVelocity = horizontalVelocity.magnitude * horizontalFactor / groundNormalUp.magnitude;
            } else {
                vVelocity = 0;
            }
            
            // this will snap the player onto the floor if we are hovering slightl above it
            vVelocity = vVelocity - groundDistance / Time.fixedDeltaTime;
            
            Debug.DrawRay(transform.position, horizontalVelocity, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + horizontalVelocity, new Vector3(0, vVelocity, 0), Color.magenta, Time.fixedDeltaTime);
        }
        return vVelocity;
    }


    public override void EnterTornado(Tornado tornado)
    {
        base.EnterTornado(tornado);
        playerAnimation.isInsideTornado = true;
    }
    public override void ExitTornado()
    {
        base.ExitTornado();
        playerAnimation.isInsideTornado = false;
    }


    public void Peck()
    {
        Debug.Log("peck");
        playerState = PlayerState.PECK;
        
        Managers.AudioManager.PlaySound("Peck");
        peckHitbox.SetActive(true);
        
        // stop pecking after some time
        this.Invoke(() => {
            if (playerState == PlayerState.PECK) {
                playerState = PlayerState.MOVE;
            }

            peckHitbox.SetActive(false);
        }, peckDuration);
    }

    public void ChargeGust()
    {
        playerState = PlayerState.CHARGE;
        tornadoMarker.activateMarker();
    }

    public void Gust()
    {
        playerState = PlayerState.FLAP;

        tornadoMarker.deactivateMarker();

        //Debug.Log("Sending out real gust");
        Managers.AudioManager.PlaySound("Tornado");
        Gust gust = Instantiate(gustPrefab, gustSpawnLocation.position, rotateTransform.rotation, null)
            .GetComponent<Gust>();
        gust.SetPlayer(this);

        // stop flapping after some time
        this.Invoke(() => {
            if (playerState == PlayerState.FLAP) {
                playerState = PlayerState.MOVE;
            }

            peckHitbox.SetActive(false);
        }, flapDuration);
    }


    public void Die() {
        if (playerState == PlayerState.DEAD || inCutscene) {
            return;
        }

        inCutscene = true;
        playerState = PlayerState.DEAD;

        Debug.Log("dead!");

        DOTween.Sequence()
            .InsertCallback(0, () => mainSprite.material = whiteMaterial) // white flash
            .InsertCallback(0.1f, () => mainSprite.material = dissolveMaterial) // dissolve
            .Insert(0.1f, DOTween.To(t => mainSprite.material.SetFloat("_Threshold", t), 1f, 0f, 0.7f).SetEase(Ease.InOutCubic))
            .InsertCallback(1f, () => Managers.ScenesManager.ResetScene()); // reset
    }


    public void PlayWarpAnimation(WarpAltar _)
    {
        inCutscene = true;

        mainSprite.DOKill();
        DOTween.Sequence()
            .Append(mainSprite.transform.DOScaleX(0.05f, 0.4f).SetEase(Ease.InOutCubic))
            .Append(mainSprite.transform.DOLocalMoveY(10f, 0.4f).SetEase(Ease.InCubic))
            .SetLink(gameObject).SetTarget(mainSprite);
    }

    public void OnSceneTransition() {
        inCutscene = true;
    }


    private void OnDrawGizmos()
    {
        if (peckHitbox != null)
        {
            Gizmos.color = peckHitbox.activeInHierarchy ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(peckHitbox.transform.position, Vector3.one * 0.5f);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(gustSpawnLocation.position,
            gustPrefab.transform.localScale);
    }


    private void CalculateTornadoPlacement()
    {
        boxCastHit = Physics.BoxCast(
            gustSpawnLocation.position,
            gustPrefab.transform.localScale,
            rotateTransform.forward,
            out objectHit,
            transform.rotation,
            tornadoSpawnDistance,
            wallMask
        );
        
        //boxcast collided with something
        if(boxCastHit)
        {
            Debug.DrawRay(gustSpawnLocation.position, rotateTransform.forward * objectHit.distance, Color.black, Time.fixedDeltaTime);
            //set tornado's spawn point to be 1/2 tornado size away from the object it collided with (prevents from being inside walls)
            Vector3 backward = -1 * rotateTransform.forward;
            tornadoMarker.setTornadoMarker(objectHit.point 
                + Vector3.Scale(backward, tornadoPrefab.transform.localScale / 2));
        }
        else
        {
            //set marker to be at max distance for tornado spawn
            Debug.DrawRay(gustSpawnLocation.position, rotateTransform.forward * tornadoSpawnDistance, Color.black, Time.fixedDeltaTime);
            tornadoMarker.setTornadoMarker(gustSpawnLocation.position + rotateTransform.forward * tornadoSpawnDistance);
        }
    }

}
