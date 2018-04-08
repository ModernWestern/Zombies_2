using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour 
{
	public float gravityScale;
	public static float globalGravity = Physics.gravity.y;
	private Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
	{
		Vector3 gravity = globalGravity * gravityScale * Vector3.up;
		rb.AddForce (gravity, ForceMode.Acceleration);
	}
}
