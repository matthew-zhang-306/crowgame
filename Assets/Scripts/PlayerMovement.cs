using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : PhysicsObject
{
    [Header("Parameters")]
    public float maxSpeed;
    public float acceleration;

    [Header("References")]
    public PlayerAnimation playerAnimation;
    public Transform cameraFacingTransform;
    public Transform rotateTransform;
    public SpriteRenderer mainSprite;
    public GameObject peckHitbox;
    public Transform gustSpawnLocation;
    public GameObject gustPrefab;

    public PositionSO cameraPosition;

    Vector3 horizontalInput;
    Vector3 horizontalVelocity; // used for calculating vertical velocity

    private bool peckInput;
    private bool oldPeckInput;
    private bool gustInput;
    private bool oldGustInput;
    public GameObject tornadoMarker;

    private bool inCutscene;


    protected override void Awake() {
        base.Awake();
        peckHitbox.SetActive(false);
    }

    protected override void OnEnable() {
        base.OnEnable();
        WarpAltar.OnAltarWarp += PlayWarpAnimation;
    }
    protected override void OnDisable() {
        base.OnDisable();
        WarpAltar.OnAltarWarp -= PlayWarpAnimation;
    }


    private void Update() {
        peckInput = Input.GetAxisRaw("Action1") > 0;
        gustInput = Input.GetAxisRaw("Action2") > 0;
    }

    protected override void FixedUpdate() {
        GetHorizontalInput();
        base.FixedUpdate();

        Debug.DrawRay(transform.position, rigidbody.velocity, Color.cyan, Time.fixedDeltaTime);

        if (inCutscene) {
            return;
        }

        if (peckInput && !oldPeckInput && !peckHitbox.activeInHierarchy) {
            Peck();
        }
        if (gustInput && !oldGustInput) {
            ChargeGust();
        }

        //just let go of tornado button
        if(!gustInput && oldGustInput)
        {
            Gust();
        }

        oldPeckInput = peckInput;
        oldGustInput = gustInput;
    }


    private void GetHorizontalInput() {
        horizontalInput = Vector3.zero;
        if (inCutscene) {
            return;
        }

        horizontalInput = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")
        );

        Vector3 cameraDir = cameraPosition.direction.WithY(0).normalized;
        Debug.DrawRay(transform.position, Vector3.Cross(cameraDir, Vector3.up), Color.green, Time.fixedDeltaTime);
        horizontalInput = horizontalInput.x * -Vector3.Cross(cameraDir, Vector3.up) + horizontalInput.z * cameraDir;
        horizontalInput = horizontalInput.normalized;
    
        if (horizontalInput != Vector3.zero) {
            Vector3 hInputRounded = Quaternion.Euler(0, Helpers.RoundToNearest(Vector3.SignedAngle(Vector3.forward, horizontalInput, Vector3.up), 90f), 0) * Vector3.forward;
            rotateTransform.Rotate(0, Vector3.SignedAngle(rotateTransform.forward, hInputRounded, Vector3.up), 0);
        }
    }


    protected override void CheckForGround() {
        groundNormal = Vector3.zero;
        groundRigidbody = null;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask)) {
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            
            // player requires extra check for slope angle
            if (Vector3.Dot(Vector3.up, hit.normal) > 0.65f) {
                groundY = hit.point.y;
                groundDistance = hit.distance - 0.11f;
                groundNormal = hit.normal;
                groundRigidbody = hit.rigidbody ?? hit.collider.GetComponentInParent<Rigidbody>();
            }
        }
    }

    protected override Vector3 HandleHorizontalMovement(Vector3 hVelocity) {
        hVelocity = Vector3.MoveTowards(hVelocity, horizontalInput * maxSpeed, acceleration * Time.deltaTime);

        if (groundNormal != Vector3.zero) {
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, hVelocity.normalized);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            if (groundNormalUp.magnitude != 0)
                hVelocity.y = hVelocity.magnitude * horizontalFactor / groundNormalUp.magnitude;
            else
                hVelocity.y = 0;
            
            Debug.DrawRay(transform.position, hVelocity, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + hVelocity, new Vector3(0, hVelocity.y, 0), Color.magenta, Time.fixedDeltaTime);
        }

        horizontalVelocity = hVelocity;
        return hVelocity;
    }

    protected override float HandleVerticalMovement(float vVelocity) {
        vVelocity = base.HandleVerticalMovement(vVelocity);

        if (currentTornado == null && groundNormal != Vector3.zero) {
            // we could be on a slope. aim velocity up or down the slope
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, horizontalVelocity.normalized);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            if (groundNormalUp.magnitude != 0)
                vVelocity = horizontalVelocity.magnitude * horizontalFactor / groundNormalUp.magnitude;
            else
                vVelocity = 0;
            
            Debug.DrawRay(transform.position, horizontalVelocity, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + horizontalVelocity, new Vector3(0, vVelocity, 0), Color.magenta, Time.fixedDeltaTime);
        }
        return vVelocity;
    }


    public override void EnterTornado(Tornado tornado) {
        base.EnterTornado(tornado);
        playerAnimation.isInsideTornado = true;
    }
    public override void ExitTornado() {
        base.ExitTornado();
        playerAnimation.isInsideTornado = false;
    }


    public void Peck() {
        Managers.AudioManager.PlaySound("Peck");
        peckHitbox.SetActive(true);
        this.Invoke(() => peckHitbox.SetActive(false), 0.2f);
    }

    public void ChargeGust() {
        Debug.Log("Sending out calc gust");
        Object.Instantiate(gustPrefab, gustSpawnLocation.position, rotateTransform.rotation, null);
    }

    public void Gust() {
        //send out the tornado

        //cant keep reference to gust or error, so need to manually find the wc to destroy it
        GameObject[] markers = GameObject.FindGameObjectsWithTag("WorldCanvas");

        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
        Debug.Log("Destroying WC");
        GameObject wc = GameObject.Find("WorldCanvas(Clone)");
        Destroy(wc);

        Debug.Log("Sending out real gust");
        Managers.AudioManager.PlaySound("Tornado");
        Instantiate(gustPrefab, gustSpawnLocation.position, rotateTransform.rotation, null);
    }


    public void PlayWarpAnimation(WarpAltar _) {
        inCutscene = true;

        mainSprite.DOKill();
        DOTween.Sequence()
            .Append(mainSprite.transform.DOScaleX(0.05f, 0.4f).SetEase(Ease.InOutCubic))
            .Append(mainSprite.transform.DOLocalMoveY(10f, 0.4f).SetEase(Ease.InCubic))
            .SetLink(gameObject).SetTarget(mainSprite);
    }


    private void OnDrawGizmos() {
        if (peckHitbox != null) {
            Gizmos.color = peckHitbox.activeInHierarchy ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(peckHitbox.transform.position, Vector3.one * 0.5f);
        }
    }

}
