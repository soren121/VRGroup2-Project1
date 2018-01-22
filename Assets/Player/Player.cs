using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Transform head;
	public HandController leftHand;
	public HandController rightHand;

	public Transform hmd;
	public Transform leftController;
	public Transform rightController;

	public Rigidbody leftHeldObject;
	public Rigidbody rightHeldObject;

	float saveMaxLeft;
	float saveMaxRight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void LateUpdate()
	{
		head.position = hmd.position;
		head.rotation = hmd.rotation;
		leftHand.transform.position = leftController.position;
		rightHand.transform.position = rightController.position;
		leftHand.transform.rotation = leftController.rotation;
		rightHand.transform.rotation = rightController.rotation;

		int leftIndex = (int)leftController.GetComponent<SteamVR_TrackedObject>().index;
		if (leftIndex >= 0)
		{
			float leftTrigger = SteamVR_Controller.Input(leftIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).magnitude;
			if (leftHand.intersected != null && leftTrigger > .2f)
			{
				leftHeldObject = leftHand.intersected;
				saveMaxLeft = leftHand.intersected.maxAngularVelocity;
				leftHand.intersected.maxAngularVelocity = Mathf.Infinity;
			}
			if (leftHeldObject != null && leftTrigger <= .2f)
			{
				//release it
				leftHeldObject.velocity = SteamVR_Controller.Input(leftIndex).velocity;
				leftHeldObject.angularVelocity = SteamVR_Controller.Input(leftIndex).angularVelocity;
				leftHeldObject.maxAngularVelocity = saveMaxLeft;
				leftHeldObject = null;
			}
			if (leftHeldObject != null)
			{
				//force the object to follow my hand
				leftHeldObject.velocity = (leftHand.transform.position - leftHeldObject.position) / Time.deltaTime;
				float angle;
				Vector3 axis;
				Quaternion q = rightHand.transform.rotation * Quaternion.Inverse(leftHeldObject.rotation);
				q.ToAngleAxis(out angle, out axis);

				leftHeldObject.angularVelocity = axis * angle * Mathf.Deg2Rad / Time.deltaTime;
			}
		}
		int rightIndex = (int)rightController.GetComponent<SteamVR_TrackedObject>().index;
		if (rightIndex >= 0)
		{
			float rightTrigger = SteamVR_Controller.Input(rightIndex).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).magnitude;
			if (rightHand.intersected != null && rightTrigger > .2f)
			{
				rightHeldObject = rightHand.intersected;
				saveMaxRight = rightHand.intersected.maxAngularVelocity;
				rightHand.intersected.maxAngularVelocity = Mathf.Infinity;

			}

			if (rightHeldObject != null && rightTrigger <= .2f)
			{
				//release it
				rightHeldObject.velocity = SteamVR_Controller.Input(rightIndex).velocity;
				rightHeldObject.angularVelocity = SteamVR_Controller.Input(rightIndex).angularVelocity;
				rightHeldObject.maxAngularVelocity = saveMaxRight;
				rightHeldObject = null;
			}

			if (rightHeldObject != null)
			{
				//force the object to follow my hand
				rightHeldObject.velocity = (rightHand.transform.position - rightHeldObject.position) / Time.deltaTime;
				float angle;
				Vector3 axis;
				Quaternion q = rightHand.transform.rotation * Quaternion.Inverse(rightHeldObject.rotation);
				q.ToAngleAxis(out angle, out axis);

				rightHeldObject.angularVelocity = axis * angle * Mathf.Deg2Rad / Time.deltaTime;
			}
		}

	}
}
