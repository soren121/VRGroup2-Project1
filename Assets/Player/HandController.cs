using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
	public Player player;
	public Rigidbody intersected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (intersected == null)
		{
			intersected = other.attachedRigidbody;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		intersected = null;
	}

}
