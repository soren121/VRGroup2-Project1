using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PullZone : MonoBehaviour {

	public event Action OnObjectLoaded;
	public event Action OnObjectLaunched;
	public event Action OnObjectUnloaded;
	public Rigidbody loadedObject;

	private Player player;
	private string slingshotHand;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<Player>();
		slingshotHand = GetComponentInParent<Slingshot>().slingshotHand;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if(slingshotHand == "left") {
			// check if object being held in RIGHT hand is one being loaded
			if(player.rightHeldObject != null && 
				player.rightHeldObject.gameObject.GetInstanceID() == collider.gameObject.GetInstanceID()) {
				// load object
				loadedObject = player.rightHeldObject;
				// let slingshot know its loaded
				if(OnObjectLoaded != null) OnObjectLoaded();
			} // if
		} else if(slingshotHand == "right") {
			// check if object being held in LEFT hand is one being loaded
			if(player.leftHeldObject != null && 
				player.leftHeldObject.gameObject.GetInstanceID() == collider.gameObject.GetInstanceID()) {
				// load object
				loadedObject = player.leftHeldObject;
				// let slingshot know its loaded
				if(OnObjectLoaded != null) OnObjectLoaded();
			} // if
		} // if
	}

	void OnTriggerStay(Collider collider) {
		if(loadedObject != null) {
			// check if let go of loaded object inside pullzone
			if((slingshotHand == "left" && player.rightHeldObject == null) || 
				(slingshotHand == "right" && player.leftHeldObject == null)) {
				// let slingshot know to launch object
				if(OnObjectLaunched != null) OnObjectLaunched();
			}
		} // if
	}

	void OnTriggerExit(Collider collider) {
		// unload object
		loadedObject = null;
		// let slingshot know its unloaded
		if(OnObjectUnloaded != null) OnObjectUnloaded();
	}

}
