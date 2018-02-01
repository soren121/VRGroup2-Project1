using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PullZone : MonoBehaviour {

	public event Action OnObjectLoaded;
	public Rigidbody loadedObject;
 

	private Player player;
	private string slingshotHand;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<Slingshot>().player;
		slingshotHand = GetComponentInParent<Slingshot>().slingshotHand;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if(slingshotHand == "left") {
			// check if object being held in RIGHT hand is one being loaded
			if(player.rightHeldObject != null && 
				player.rightHeldObject.gameObject.GetInstanceID() == collider.attachedRigidbody.gameObject.GetInstanceID()) {
				// load object
				loadedObject = player.rightHeldObject;
				Debug.Log(loadedObject);
				// let slingshot know its loaded
				if(OnObjectLoaded != null) OnObjectLoaded();
			} // if
		} else if(slingshotHand == "right") {
			// check if object being held in LEFT hand is one being loaded
			if(player.leftHeldObject != null && 
				player.leftHeldObject.gameObject.GetInstanceID() == collider.attachedRigidbody.gameObject.GetInstanceID()) {
				// load object
				loadedObject = player.leftHeldObject;
				Debug.Log(loadedObject);
				// let slingshot know its loaded
				if(OnObjectLoaded != null) OnObjectLoaded();
			} // if
		} // if
	}

	void OnTriggerExit(Collider collider) {
		// check if object leaving pullzone is one that was loaded
		if(loadedObject != null && 
			loadedObject.gameObject.GetInstanceID() == collider.attachedRigidbody.gameObject.GetInstanceID()) {
            // unload object
            loadedObject = null;
            Debug.Log(loadedObject);

		} // if
	}

}
