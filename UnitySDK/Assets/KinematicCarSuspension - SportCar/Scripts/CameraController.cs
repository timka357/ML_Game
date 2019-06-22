using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private float 
		DistanceCam1,
		DistanceCam=4,
		rotX=45,
		rotY=30;
	private int CamType=0;
	private string[] CameraTypes = { "Free cam", "Front fixed cam", "Rear fixed cam","Front wheel cam", "Rear wheel cam","Fixed cam" };
	public Transform 
		FixCamPointFront,
		FixCamPointRear,
		FixCamPointWheelFront,
		FixCamPointWheelRear;

	public float
		MouseSpeed = 4,
		MouseScrollSpeed = 2,
		MinDistance = 3,
		MaxDistance = 4;

	public Transform target;

	void Update () {
		switch (CamType) {
		case(0):
			if (target == null) break;
			if (Input.GetKey (KeyCode.Mouse1)) {
				rotX += Input.GetAxis ("Mouse X") * MouseSpeed;
				rotY -= Input.GetAxis ("Mouse Y") * MouseSpeed;
			}
			rotY = Mathf.Clamp (rotY, 0, 70);
			DistanceCam -= Input.GetAxis ("Mouse ScrollWheel") * MouseScrollSpeed;
			DistanceCam = Mathf.Clamp (DistanceCam, MinDistance, MaxDistance);
			DistanceCam1 = Mathf.Lerp (DistanceCam1, DistanceCam, 7 * Time.deltaTime);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (rotY, rotX, 0), Time.deltaTime * 6);
			transform.position = target.position + transform.rotation * new Vector3 (0.0f, 0.0f, -DistanceCam1);
			break;

		case(1):
			if (FixCamPointFront == null) break;
			transform.position = FixCamPointFront.position;
			transform.rotation = FixCamPointFront.rotation;
			break;
		case(2):
			if (FixCamPointRear == null) break;
			transform.position = FixCamPointRear.position;
			transform.rotation = FixCamPointRear.rotation;
			break;

		case(3):
			if (FixCamPointWheelFront == null) break;
			transform.position = FixCamPointWheelFront.position;
			transform.rotation = FixCamPointWheelFront.rotation;
			break;
		case(4):
			if (FixCamPointWheelRear == null) break;
			transform.position = FixCamPointWheelRear.position;
			transform.rotation = FixCamPointWheelRear.rotation;
			break;
		case(5):
			if (target == null) break;
			transform.LookAt (target);
			break;
		}
	}

	void OnGUI(){
		CamType = GUI.SelectionGrid (new Rect (Screen.width/2-225, 10, 400, 60), CamType, CameraTypes, 3);
	}


}
