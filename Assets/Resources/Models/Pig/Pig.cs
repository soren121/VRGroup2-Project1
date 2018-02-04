using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Actionable {

	public GameObject PoofSound;
    public GameObject Poof;

	public override IEnumerable HandleFloorCollision(Collision c) {
		GameObject newPoofSound = GameObject.Instantiate(PoofSound);

		// Decrement pig count
		GameStatus.instance.DecreasePigCount();

		GameObject.Destroy(newPoofSound, 2f);
		GameObject.Destroy(gameObject, 2f);

		yield return null;
	}

	public override IEnumerable HandlePlankCollision(Collision c) {
		yield return null;
	}
}
