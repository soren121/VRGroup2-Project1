using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour {

	public GameObject birdsTextObj;
	public GameObject pigsTextObj;

	public static int pigCount;
	public static int birdCount;

	void Awake() {
		DontDestroyOnLoad(this);

		pigCount = 3;
		birdCount = 3;
	}

	void Update() {
		birdsTextObj.GetComponent<TextMesh>().text = "Birds: " + birdCount;
		pigsTextObj.GetComponent<TextMesh>().text = "Pigs: " + pigCount;
	}

}
