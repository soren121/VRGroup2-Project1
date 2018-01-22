using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour {

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.maxAngularVelocity = Mathf.Infinity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
