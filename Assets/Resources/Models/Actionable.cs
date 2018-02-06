using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actionable : MonoBehaviour {

	[HideInInspector] public bool hasFired = false;
	public abstract IEnumerable HandleFloorCollision(Collision c);
	public abstract IEnumerable HandlePlankCollision(Collision c);
}
