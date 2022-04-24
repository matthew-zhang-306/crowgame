using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

	[SerializeField]
	GameObject dustCloud;

	[SerializeField]
	private bool coroutineAllowed, grounded;

	private PlayerMovement playerMovement;
	private Rigidbody rb;

	private void Start()
	{
		playerMovement = this.gameObject.GetComponentInParent<PlayerMovement>();
		rb = this.gameObject.GetComponentInParent<Rigidbody>();
	}

	//private void FixedUpdate()
	//{
	//	if (playerMovement.IsOnGround)
	//	{
	//		grounded = true;
	//		coroutineAllowed = true;
	//		//Instantiate(dustCloud, transform.position, dustCloud.transform.rotation);
	//	}
	//	else
	//	{
	//		grounded = false;
	//		coroutineAllowed = false;
	//	}
	//}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			grounded = true;
			coroutineAllowed = true;
			Instantiate(dustCloud, transform.position, dustCloud.transform.rotation);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			grounded = false;
			coroutineAllowed = false;
		}
	}

	void Update()
	{
		if (grounded && rb.velocity.x != 0 && coroutineAllowed)
		{
			StartCoroutine("SpawnCloud");
			coroutineAllowed = false;
		}

		if (rb.velocity.x == 0 || !grounded)
		{
			StopCoroutine("SpawnCloud");
			coroutineAllowed = true;
		}
	}

	IEnumerator SpawnCloud()
	{
		while (grounded)
		{
			Instantiate(dustCloud, transform.position, dustCloud.transform.rotation);
			yield return new WaitForSeconds(100f);
		}
	}
}
