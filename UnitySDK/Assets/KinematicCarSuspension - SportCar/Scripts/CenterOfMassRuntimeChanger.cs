using UnityEngine;
using System.Collections;

public class CenterOfMassRuntimeChanger : MonoBehaviour {

	private Rigidbody rigidBody;
	private Vector3 Point;
	public GameObject Target;

	void Start () {
		rigidBody = GetComponentInParent<Rigidbody> ();
	}
	
	void Update () {
		if (Target == null)
			return;
		
		if (transform.InverseTransformPoint(Target.transform.position) == rigidBody.centerOfMass)
			return;
		
		rigidBody.centerOfMass=transform.InverseTransformPoint(Target.transform.position);	

	}
}
