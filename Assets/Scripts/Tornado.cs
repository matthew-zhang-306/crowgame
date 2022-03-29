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
        OnTornadoSpawned += TornadoSpawned;
    }

    protected override void OnEnable() {
        base.OnEnable();
        
    }
    protected override void OnDisable() {
        base.OnDisable();

        previousPlayer?.ExitTornado();
        previousPlayer = null;
    }

    private void OnDestroy()
    {
        OnTornadoSpawned -= TornadoSpawned;
    }


    private void TornadoSpawned(Tornado otherTornado) {
        OnTornadoSpawned -= TornadoSpawned;
        Destroy(gameObject);
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        // lift any boxes in the tornado
        bool cast = Physics.Raycast(
            transform.position - new Vector3(0, 0.05f, 0),
            Vector3.up,
            out RaycastHit hit,
            topOffset + 0.1f, // cast slightly higher than the top so that any boxes that move slightly above can still be caught
            LayerMask.GetMask("Box")
        );
        PushableBox box = null;
        if (cast)
            box = hit.rigidbody?.GetComponent<PushableBox>();

        if (box != previousBox) {
            // change which box we're lifting
            RemoveRider(previousBox);
            AddRider(box);
        }
        previousBox = box;

        // lift any player in the tornado, provided they're under any boxes in the tornado
        cast = Physics.BoxCast(
            transform.position - new Vector3(0, 0.05f, 0),
            new Vector3(0.3f, 0.01f, 0.3f),
            Vector3.up,
            out hit,
            Quaternion.identity,
            cast ? hit.distance : topOffset + 0.1f,
            LayerMask.GetMask("Player")
        );
        PlayerMovement player = null;
        if (cast)
            player = hit.rigidbody?.GetComponent<PlayerMovement>();
        
        if (player != previousPlayer) {
            // change which player we're lifting
            RemoveRider(previousPlayer);
            AddRider(player);
        }
        previousPlayer = player;
    }


    public override Vector3 GetRidePoint(PhysicsObject rider) {
        if (rider is PlayerMovement player) {
            // lift the player up less
            return Top - new Vector3(0, 0.5f, 0);
        }

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
