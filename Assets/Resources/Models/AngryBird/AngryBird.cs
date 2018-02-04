using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : Actionable {

	Rigidbody rb;
	
    public GameObject PoofSound;
    public GameObject Poof;

	public bool hasCollided = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.maxAngularVelocity = Mathf.Infinity;
	}

	public override IEnumerable HandleCollision(Collision collision) {
		if (!hasCollided) {
			GameObject newPoofSound = GameObject.Instantiate(PoofSound);
			Vector3 direction = (collision.gameObject.transform.position - newPoofSound.transform.position).normalized;
			newPoofSound.transform.position += direction;

			//GameObject.Instantiate(Poof);
			//Vector3 direction1 = (collision.gameObject.transform.position - Poof.transform.position).normalized;
			//Poof.transform.position += direction1;

			GameStatus.instance.SpawnNextBird();
			yield return new WaitForSeconds(1);

			GameObject.Destroy(newPoofSound);
			//GameObject.Destroy(Poof);
			//GameObject.Destroy(collision.gameObject);
			hasCollided = true;
		}
		
		yield return null;
	}
}
