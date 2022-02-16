using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float maxSpeed;
    public float acceleration;
    public float maxFallSpeed;
    public float gravity;
    public float tornadoRiseAccel;
    public float tornadoRiseSpeed;

    [Header("References")]
    public PlayerAnimation playerAnimation;
    public Transform cameraFacingTransform;
    public Transform rotateTransform;
    public SpriteRenderer mainSprite;
    public GameObject peckHitbox;
    public Transform gustSpawnLocation;
    public GameObject gustPrefab;


    Camera mainCamera;

    new Rigidbody rigidbody;
    new Collider collider;
    Vector3 horizontalInput;
    Vector3 groundNormal;
    float groundDistance;
    int wallMask;
    Tornado currentTornado;

    private bool peckInput;
    private bool oldPeckInput;
    private bool gustInput;
    private bool oldGustInput;
    public GameObject tornadoMarker;

    private bool inCutscene;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        wallMask = LayerMask.GetMask("Wall", "Box");

        peckHitbox.SetActive(false);
    }

    private void OnEnable() {
        WarpAltar.OnAltarWarp += PlayWarpAnimation;
    }
    private void OnDisable() {
        WarpAltar.OnAltarWarp -= PlayWarpAnimation;
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        peckInput = Input.GetAxisRaw("Action1") > 0;
        gustInput = Input.GetAxisRaw("Action2") > 0;
    }

    private void FixedUpdate() {
        groundNormal = Vector3.zero;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask)) {
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            
            if (Vector3.Dot(Vector3.up, hit.normal) > 0.7f) {
                groundDistance = hit.distance - 0.11f;
                groundNormal = hit.normal;
            }
        }
        
        GetHorizontalInput();
        HandleMovement();

        if (inCutscene) {
            return;
        }

        if (peckInput && !oldPeckInput && !peckHitbox.activeInHierarchy) {
            // peck
            Managers.AudioManager.PlaySound("Peck");
            peckHitbox.SetActive(true);
            this.Invoke(() => peckHitbox.SetActive(false), 0.2f);
        }
        if (gustInput && !oldGustInput) {
            //calculate the position that the tornado will spawn
            Debug.Log("Sending out calc gust");
            Object.Instantiate(gustPrefab, gustSpawnLocation.position, rotateTransform.rotation, null);

        }

        //just let go of tornado button
        if(!gustInput && oldGustInput)
        {
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

        Vector3 cameraDir = transform.position - mainCamera.transform.position;
        cameraDir = cameraDir.WithY(0).normalized;
        Debug.DrawRay(transform.position, Vector3.Cross(cameraDir, Vector3.up), Color.green, Time.fixedDeltaTime);
        horizontalInput = horizontalInput.x * -Vector3.Cross(cameraDir, Vector3.up) + horizontalInput.z * cameraDir;
        horizontalInput = horizontalInput.normalized;

        // since we have the camera angle we'll do the sprite rotation now
        float rotateAngle = Vector3.SignedAngle(cameraFacingTransform.forward, cameraDir, Vector3.up);
        cameraFacingTransform.Rotate(new Vector3(0, rotateAngle, 0));
    
        if (horizontalInput != Vector3.zero) {
            Vector3 hInputRounded = Quaternion.Euler(0, Helpers.RoundToNearest(Vector3.SignedAngle(Vector3.forward, horizontalInput, Vector3.up), 90f), 0) * Vector3.forward;
            rotateTransform.Rotate(0, Vector3.SignedAngle(rotateTransform.forward, hInputRounded, Vector3.up), 0);
        }
    }


    private void HandleMovement() {
        Vector3 velocity = rigidbody.velocity;
        
        Vector3 horizontalVel = velocity.WithY(0);
        horizontalVel = Vector3.MoveTowards(horizontalVel, horizontalInput * maxSpeed, acceleration * Time.deltaTime);
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.z;

        if (groundNormal != Vector3.zero) {
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, horizontalVel.normalized);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            if (groundNormalUp.magnitude != 0)
                velocity.y = horizontalVel.magnitude * horizontalFactor / groundNormalUp.magnitude;
            else
                velocity.y = 0;
            
            Debug.DrawRay(transform.position, horizontalVel, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + horizontalVel, new Vector3(0, velocity.y, 0), Color.magenta, Time.fixedDeltaTime);
        }
        
        if (currentTornado != null) {
            velocity.y += tornadoRiseAccel * Time.deltaTime;
            velocity.y = Mathf.Min(velocity.y, tornadoRiseSpeed * Time.deltaTime);
        }
        else if (groundNormal == Vector3.zero) {
            velocity += Vector3.down * gravity * Time.deltaTime;
            if (velocity.y < -maxFallSpeed)
                velocity.y = -maxFallSpeed;
        }
        else {
            velocity.y -= groundDistance / Time.fixedDeltaTime;
        }

        Debug.DrawRay(transform.position, velocity, Color.cyan, Time.fixedDeltaTime);
        rigidbody.velocity = velocity;
    }


    public void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
        playerAnimation.isInsideTornado = true;
    }
    public void ExitTornado() {
        currentTornado = null;
        playerAnimation.isInsideTornado = false;
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
        if (peckHitbox != null && peckHitbox.activeInHierarchy) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(peckHitbox.transform.position, Vector3.one * 0.5f);
        }
    }

}
