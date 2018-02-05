using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {

	public static GameStatus instance = null;
	[HideInInspector] public Transform spawnLocation { get; private set; }

	public GameObject birdsTextObj;
	public GameObject pigsTextObj;
	public AudioClip gameOverClip;
	public AudioClip levelWonClip;

	private int pigCount = 0;
	private int birdCount = 0;

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
		spawnLocation = GameObject.Find("SpawnPlatform/SpawnPlatformGraphics/SpawnLocation").transform;

		//ResetLevel();
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

		pigCount = 3;
		birdCount = birdOrder.Count;

		SpawnNextBird();
	}

	private IEnumerator PlayAndResetLevel(AudioSource source) {
		source.Play();
		yield return new WaitForSeconds(source.clip.length);

		string currentSceneName = SceneManager.GetActiveScene().name;
    	SceneManager.LoadScene(currentSceneName);

		InitLevel();
		yield return null;
	}

	private void LevelWon() {
		Debug.Log("Player won! Load next level.");

		AudioSource source = GameObject.Find("Player/Head").GetComponent<AudioSource>();
		source.clip = levelWonClip;

		StartCoroutine(PlayAndResetLevel(source));
	}

	private void GameOver() {
		Debug.Log("womp womp");

		AudioSource source = GameObject.Find("Player/Head").GetComponent<AudioSource>();
		source.clip = gameOverClip;

		StartCoroutine(PlayAndResetLevel(source));
	}

	public void CheckScore() {
		if (birdCount <= 0 && pigCount > 0) {
			GameOver();
			return;
		}

		if (pigCount <= 0) {
			LevelWon();
		}
	}

	public void DecreasePigCount() {
		pigCount--;
	}

	public void DecreaseBirdCount() {
		birdCount--;
	}

	public void SpawnNextBird() {
		if (birdCount <= 0) {
			return;
		}

		string bird = birdOrder[birdOrder.Count - birdCount];
		Debug.Log("Accessing bird \"" + bird + "\" at index " + (birdOrder.Count - birdCount));
		GameObject newBird = Resources.Load("Models/" + bird + "/" + bird, typeof(GameObject)) as GameObject;
		Instantiate(newBird, spawnLocation.position, spawnLocation.rotation);
	}	
}
