using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : GridAlignedObject
{
    public float topOffset;
    public Vector3 Top => transform.position + Vector3.up * topOffset;

    PushableBox previousBox;
    PlayerMovement previousPlayer;

    public delegate void TornadoDelegate(Tornado tornado);
    public static TornadoDelegate OnTornadoSpawned;


    protected override void Awake() {
        base.Awake();
        OnTornadoSpawned?.Invoke(this);
    }

    private void OnEnable() {
        OnTornadoSpawned += TornadoSpawned;
    }
    private void OnDisable() {
        previousBox?.ExitTornado();
        previousBox = null;
        previousPlayer?.ExitTornado();
        previousPlayer = null;

        OnTornadoSpawned -= TornadoSpawned;
    }


    private void TornadoSpawned(Tornado otherTornado) {
        Destroy(gameObject);
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        // lift whatever is in the tornado
        bool cast = Physics.Raycast(
            transform.position - new Vector3(0, 0.05f, 0),
            Vector3.up,
            out RaycastHit hit,
            topOffset + 0.1f,
            LayerMask.GetMask("Box")
        );
        PushableBox box = null;
        if (cast)
            box = hit.rigidbody?.GetComponent<PushableBox>();

        if (box != previousBox) {
            previousBox?.ExitTornado();
            box?.EnterTornado(this);
        }
        previousBox = box;

        cast = Physics.Raycast(
            transform.position - new Vector3(0, 0.05f, 0),
            Vector3.up,
            out hit,
            cast ? hit.distance : topOffset,
            LayerMask.GetMask("Player")
        );
        PlayerMovement player = null;
        if (cast)
            player = hit.rigidbody?.GetComponent<PlayerMovement>();
        
        if (player != previousPlayer) {
            previousPlayer?.ExitTornado();
            player?.EnterTornado(this);
        }
        previousPlayer = player;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Top, 0.2f);
    }
}
