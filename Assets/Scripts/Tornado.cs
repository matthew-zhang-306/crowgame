using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : PhysicsObject
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

    protected override void OnEnable() {
        base.OnEnable();
        OnTornadoSpawned += TornadoSpawned;
    }
    protected override void OnDisable() {
        base.OnDisable();

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
            RemoveRider(previousBox);
            AddRider(box);
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


    public override Vector3 GetRidePoint() {
        return Top;
    }


    public override void AddRider(PhysicsObject rider) {
        if (rider == null) {
            return;
        }

        rider.EnterTornado(this);
        base.AddRider(rider);
    }

    public override void RemoveRider(PhysicsObject rider) {
        if (rider == null) {
            return;
        }

        rider.ExitTornado();
        base.RemoveRider(rider);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Top, 0.2f);
    }
}
