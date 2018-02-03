using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Actionable {

	public GameObject PoofSound;
    public GameObject Poof;

	public override IEnumerable HandleCollision(Collision c, Transform spawnLocation) {
		GameObject newPoofSound = GameObject.Instantiate(PoofSound);

		// Decrement score
		GameStatus.pigCount--;
		Debug.Log("pigCount: " + GameStatus.pigCount);

		GameObject.Destroy(newPoofSound, 2f);
		GameObject.Destroy(this.gameObject, 2f);

		yield return null;
	}
}
