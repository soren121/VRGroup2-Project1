using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour {

	public static GameStatus instance = null;
	public GameObject spawnPlatform;
	[HideInInspector] public Transform spawnLocation { get; private set; }

	public GameObject birdsTextObj;
	public GameObject pigsTextObj;
	public AudioClip gameOverClip;
	public AudioClip levelWonClip;

	private int pigCount = 0;
	private int birdCount = 0;

	private int level = 1;
	private List<string> birdOrder;

	void Awake() {
		// Enforce singleton pattern
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		
		// Prevent status from being destroyed between levels
		DontDestroyOnLoad(gameObject);

		// Set spawn location
		spawnLocation = spawnPlatform.transform.Find("SpawnPlatformGraphics/SpawnLocation").transform;

		InitLevel();
	}

	void Update() {
		birdsTextObj.GetComponent<TextMesh>().text = "Birds: " + birdCount;
		pigsTextObj.GetComponent<TextMesh>().text = "Pigs: " + pigCount;
	}

	private void InitLevel() {
		birdOrder = new List<string>() {
			"AngryBird",
			"AngryBird",
			"BombBird"
		};

		pigCount = GameObject.FindGameObjectsWithTag("Pig").Length;
		birdCount = birdOrder.Count + 1;

		SpawnNextBird();
	}

	private void LoadNextLevel() {
		Debug.Log("Player won! Load next level.");

		AudioSource source = GameObject.Find("Player/Head").GetComponent<AudioSource>();
		source.clip = levelWonClip;
		source.Play();
	}

	private void GameOver() {
		Debug.Log("womp womp");

		AudioSource source = GameObject.Find("Player/Head").GetComponent<AudioSource>();
		source.clip = gameOverClip;
		source.Play();
	}

	public void CheckScore() {
		if (birdCount <= 0 && pigCount > 0) {
			GameOver();
			return;
		}

		if (pigCount <= 0) {
			LoadNextLevel();
		}
	}

	public void DecreasePigCount() {
		pigCount--;
	}

	public void SpawnNextBird() {
		birdCount--;
		if (birdCount <= 0) {
			birdCount = 0;
			return;
		}

		string bird = birdOrder[birdOrder.Count - birdCount];
		GameObject newBird = Resources.Load("Models/" + bird + "/" + bird, typeof(GameObject)) as GameObject;
		Instantiate(newBird.GetComponent<Rigidbody>(), spawnLocation.position, spawnLocation.rotation);
	}	
}
