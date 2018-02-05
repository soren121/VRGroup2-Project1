using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : Actionable {

	Rigidbody rb;
	
	public GameObject PoofSound;
	public GameObject Poof;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.maxAngularVelocity = Mathf.Infinity;
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	public override IEnumerable HandleFloorCollision(Collision collision) {
		if (!hasFired) {
			GameStatus.instance.SpawnNextBird();
			GameObject.Destroy(gameObject);
		} else {
			GameObject newPoofSound = GameObject.Instantiate(PoofSound);
			//GameObject newPoof = GameObject.Instantiate(Poof);

			// newPoof.transform.parent = gameObject.transform;
			newPoofSound.transform.parent = gameObject.transform;

			float lockPos = 0f;
			// newPoof.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);
			newPoofSound.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);

			GameObject.Destroy(newPoofSound, 2f);
			// GameObject.Destroy(newPoof, 2f);
			GameObject.Destroy(gameObject, 1f);
		}

		yield return null;
	}

	public override IEnumerable HandlePlankCollision(Collision c) {
		GameObject newPoofSound = GameObject.Instantiate(PoofSound);
		Vector3 direction = (c.gameObject.transform.position - newPoofSound.transform.position).normalized;
		newPoofSound.transform.position += direction;

		yield return new WaitForSeconds(1);

		GameObject.Destroy(newPoofSound);
		//GameObject.Destroy(c.collider.gameObject);

		yield return null;
	}
}
