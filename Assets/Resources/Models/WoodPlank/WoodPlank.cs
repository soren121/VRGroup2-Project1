using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlank : MonoBehaviour {

	// Called when something hits the game platform
	void OnCollisionEnter(Collision collision) {
		StartCoroutine(handleCollision(collision));
	}

	// Determines what hit the floor, and then performs the appropriate action
	private IEnumerator handleCollision(Collision collision) {
		// Log what collided
		Debug.Log("A " + collision.transform.name + " hit a plank.");
		
		// Determine if the collider is an actionable object (bird/pig)
		Actionable actionableObj = collision.gameObject.GetComponent<Actionable>();
		if (actionableObj != null) {
			Debug.Log(collision.transform.name + " is actionable!");
			// Retrieve its collision handler and execute it
			IEnumerator collisionHandler = actionableObj.HandlePlankCollision(collision).GetEnumerator();
			while (collisionHandler.MoveNext()) {
				yield return collisionHandler.Current;
			}
		}

		yield return null;
	}
}
