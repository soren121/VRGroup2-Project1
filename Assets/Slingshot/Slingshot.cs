using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	public Player player;
	public float strength;
	public string slingshotHand;
	public Shader alphaBlendPremultiply;

	private Transform leftAnchorPoint;
	private Transform rightAnchorPoint;
	private PullZone pullZone;

	public Vector3 launchVelocity;
	public GameObject ShotSound;

	// Use this for initialization
	void Start () {
		pullZone = GetComponentInChildren<PullZone>();
		pullZone.OnObjectLoaded += LoadSlingshot;
		// the slingshot's anchor points for the band
		leftAnchorPoint = transform.Find("LeftAnchorPoint");
		rightAnchorPoint = transform.Find("RightAnchorPoint");
		gameObject.transform.Find("ballisticPathGraphics").gameObject.SetActive(false);
	}

	public void LoadSlingshot() {
		gameObject.transform.Find("ballisticPathGraphics").gameObject.SetActive(true);

		StartCoroutine(DrawSlingshotBand());
		StartCoroutine(CheckForLaunch());
	}

	// @ben - moved velocity calcs out of launchobject so they would be accessible to external scrpit (ballistic arc)
	public Vector3 CalculateV()
	{
		Rigidbody obj = pullZone.loadedObject;
		Vector3 pullbackStart = (leftAnchorPoint.position + rightAnchorPoint.position) / 2;
		Vector3 pullbackEnd = obj.transform.position;
		// calculate launch direction
		Vector3 launchDirection = -(pullbackEnd - pullbackStart).normalized;
		// calculate launch speed (spring force formula)
		float launchSpeed = (pullbackEnd - pullbackStart).magnitude * Mathf.Sqrt(strength / obj.mass);
		// calculate launch velocity
	   launchVelocity = launchDirection * launchSpeed;
	   return launchVelocity;
	}

	public void LaunchObject() {
		Debug.Log ("object launched");
		Rigidbody obj = pullZone.loadedObject;
		Vector3 pullbackStart = (leftAnchorPoint.position + rightAnchorPoint.position) / 2;
		Vector3 pullbackEnd = obj.transform.position;
		Vector3 launchV = CalculateV();
		// make the loaded object non-kinematic so force can be added
		obj.isKinematic = false;
		obj.velocity = launchV;
		pullZone.loadedObject = null;
		Debug.DrawLine (pullbackStart, pullbackEnd);

		gameObject.transform.Find("ballisticPathGraphics").gameObject.SetActive(false);
	}

   
	IEnumerator CheckForLaunch() {
		while (pullZone.loadedObject != null) {
			// check if let go of loaded object inside pullzone
			if((slingshotHand == "left" && player.rightHeldObject == null) || 
				(slingshotHand == "right" && player.leftHeldObject == null)) {
				GameObject loadedObject = pullZone.loadedObject.gameObject;
				LaunchObject();
				GameObject.Instantiate(ShotSound);
				loadedObject.GetComponent<Actionable>().hasFired = true;
				Debug.Log(pullZone.loadedObject);

				StartCoroutine(WaitForPhysics());

				yield return new WaitForSeconds(2);
				GameStatus.instance.DecreaseBirdCount();
				GameStatus.instance.SpawnNextBird();
			}
			yield return null;
		}
		yield return null;
	}

	IEnumerator DrawSlingshotBand() {
		Debug.Log ("slingshot band drawn");
		GameObject leftBand = new GameObject();
		GameObject rightBand = new GameObject();
		// get anchor points of loaded object
		Transform loadedObjectLeftAnchorPoint = pullZone.loadedObject.transform.Find("LeftAnchorPoint");
		Transform loadedObjectRightAnchorPoint = pullZone.loadedObject.transform.Find("RightAnchorPoint");
		// attach bands to slingshot
		leftBand.transform.parent = transform;
		rightBand.transform.parent = transform;
		leftBand.transform.position = leftAnchorPoint.position;
		rightBand.transform.position = rightAnchorPoint.position;
		// give bands a line renderer
		leftBand.AddComponent<LineRenderer>();
		rightBand.AddComponent<LineRenderer>();
		// get reference to each line renderer
		LineRenderer leftLR = leftBand.GetComponent<LineRenderer>();
		LineRenderer rightLR = rightBand.GetComponent<LineRenderer>();
		// **(idk what this does yet)
		leftLR.material = new Material(alphaBlendPremultiply);
		rightLR.material = new Material(alphaBlendPremultiply);
		// set the color of the bands
		leftLR.startColor = Color.black; leftLR.endColor = Color.red;
		rightLR.startColor = Color.black; rightLR.endColor = Color.red;
		// set the width of the bands
		leftLR.startWidth = 0.01f; leftLR.endWidth = 0.02f;
		rightLR.startWidth = 0.01f; rightLR.endWidth = 0.02f;
		// continuously update bands based on new location of loaded object
		while(pullZone.loadedObject != null) {
			leftLR.SetPosition(0, leftAnchorPoint.position);
			leftLR.SetPosition(1, loadedObjectLeftAnchorPoint.position);
			rightLR.SetPosition(0, rightAnchorPoint.position);
			rightLR.SetPosition(1, loadedObjectRightAnchorPoint.position);
			yield return null;
		} // while
		  // destroy the bands since there is no more loaded object
		gameObject.transform.Find("ballisticPathGraphics").gameObject.SetActive(false);
		Debug.Log("bands destroyed");
		Destroy(leftBand);
		Destroy(rightBand);
		yield return null;
	}

	IEnumerator WaitForPhysics() {
		Rigidbody[] rbs = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		bool sleeping = false;

		while (!sleeping) {
			sleeping = true;

			foreach (Rigidbody rb in rbs) {
				if (rb != null && !rb.IsSleeping()) {
					sleeping = false;
					yield return null;
					break;
				}
			}
		}

		GameStatus.instance.CheckScore();
		yield return null;
	}
}
