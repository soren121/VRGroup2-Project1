using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Called when something hits the game platform
	void OnCollisionEnter(Collision collision) {
		StartCoroutine(handleCollision(collision));
	}

	// Determines what hit the floor, and then performs the appropriate action
	private IEnumerator handleCollision(Collision collision) {
		Debug.Log("A "+collision.transform.name+" hit the floor.");
		if(collision.transform.name == "PigGraphics") {
			yield return new WaitForSeconds(3);
			GameObject.Destroy(collision.gameObject);
		}
		yield return null;
	}
}
