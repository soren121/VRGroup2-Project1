using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlatform : MonoBehaviour {

	public GameObject spawnPlatform;
	public Rigidbody thingToSpawn;
    public GameObject PoofSound;

	private Transform spawnLocation;

	// Use this for initialization
	void Start () {
		spawnLocation = GameObject.Find("SpawnPlatformGraphics/SpawnLocation").transform;
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
		if(collision.rigidbody.transform.name == "Pig") {
            GameObject.Instantiate(PoofSound);
            yield return new WaitForSeconds(2);
            GameObject.Destroy(PoofSound);
            GameObject.Destroy(collision.gameObject);
		} else if(collision.rigidbody.transform.name == "AngryBird" || collision.rigidbody.transform.name == "AngryBird(Clone)") {
            GameObject.Instantiate(PoofSound);
			yield return new WaitForSeconds(1);
            GameObject.Destroy(PoofSound);
			GameObject.Instantiate<Rigidbody>(thingToSpawn, spawnLocation.position, spawnLocation.rotation);
		}
		yield return null;
	}
}
