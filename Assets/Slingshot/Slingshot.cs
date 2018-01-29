using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	public Player player;
	public float strength;
	public string slingshotHand;

	private Transform leftAnchorPoint;
	private Transform rightAnchorPoint;
	private PullZone pullZone;

	// Use this for initialization
	void Start () {
		pullZone = GetComponentInChildren<PullZone>();
		pullZone.OnObjectLoaded += LoadSlingshot;
		pullZone.OnObjectLaunched += LaunchObject;
		// the slingshot's anchor points for the band
		leftAnchorPoint = transform.Find("LeftAnchorPoint");
		rightAnchorPoint = transform.Find("RightAnchorPoint");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadSlingshot() {
		StartCoroutine(DrawSlingshotBand());
	}

	public void LaunchObject() {
		Rigidbody obj = pullZone.loadedObject;
		Vector3 pullbackStart = (leftAnchorPoint.position - rightAnchorPoint.position)/2;
		Vector3 pullbackEnd = obj.transform.position;
		// calculate launch direction
		Vector3 launchDirection = (pullbackStart - pullbackEnd).normalized;
		// calculate launch speed (spring force formula)
		float launchSpeed = (pullbackStart - pullbackEnd).magnitude * Mathf.Sqrt(strength / obj.mass);
		// calculate launch velocity
		Vector3 launchVelocity = launchDirection * launchSpeed;
		// make the loaded object non-kinematic so force can be added
		obj.isKinematic = false;
		obj.AddForce(launchVelocity);
	}

	IEnumerator DrawSlingshotBand() {
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
		leftLR.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		rightLR.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		// set the color of the bands
		leftLR.startColor = Color.black; leftLR.endColor = Color.red;
		rightLR.startColor = Color.black; rightLR.endColor = Color.red;
		// set the width of the bands
		leftLR.startWidth = 0.2f; leftLR.endWidth = 0.4f;
		rightLR.startWidth = 0.2f; rightLR.endWidth = 0.4f;
		// continuously update bands based on new location of loaded object
		while(pullZone.loadedObject != null) {
			leftLR.SetPosition(0, leftAnchorPoint.position);
			leftLR.SetPosition(1, loadedObjectLeftAnchorPoint.position);
			rightLR.SetPosition(0, rightAnchorPoint.position);
			rightLR.SetPosition(1, loadedObjectRightAnchorPoint.position);
			yield return null;
		} // while
		// destroy the bands since there is no more loaded object
		Destroy(leftBand);
		Destroy(rightBand);
		yield return null;
	}
}
