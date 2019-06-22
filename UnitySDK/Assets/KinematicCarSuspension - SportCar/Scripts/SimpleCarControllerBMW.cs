using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wheel {
	public WheelCollider wheelCollider;
	public bool 
		Steering,
		Motor,
		Handbrake;
	public float DefSidewaysStiffness;
}

public class SimpleCarControllerBMW : MonoBehaviour {

	public Wheel[] Wheels;
	public float MotorPower;
	public float SteeringAngle;
	public float BrakingTorque;

	[Range(10,200)]
	public float SteeringSpeed;

	private float SmoothSteeringInput;

	void Start(){
		foreach (var wheel in Wheels)
			wheel.DefSidewaysStiffness = wheel.wheelCollider.sidewaysFriction.stiffness;
	}

	void GetSmoothSteeringInput(){
		SmoothSteeringInput = Mathf.MoveTowards (SmoothSteeringInput, Input.GetAxisRaw ("Horizontal"), SteeringSpeed * Time.fixedDeltaTime);
	}

	void Update () {
		GetSmoothSteeringInput ();

		foreach (var wheel in Wheels) {
			WheelFrictionCurve _wfc = wheel.wheelCollider.sidewaysFriction;
			_wfc.stiffness = wheel.DefSidewaysStiffness;

            if (wheel.Steering && wheel.wheelCollider != null)
                wheel.wheelCollider.steerAngle = SmoothSteeringInput * SteeringAngle;

            if (wheel.Motor && wheel.wheelCollider!=null) wheel.wheelCollider.motorTorque = Input.GetAxis ("Vertical") * MotorPower;

			if (Mathf.Sign (Input.GetAxis ("Vertical")) != Mathf.Sign (wheel.wheelCollider.rpm) 
				&& Mathf.Abs(wheel.wheelCollider.rpm)>20 
				&& Input.GetAxis("Vertical")!=0) 
				wheel.wheelCollider.brakeTorque = BrakingTorque;
			else wheel.wheelCollider.brakeTorque = 0;

			if (wheel.Handbrake) 
			if (Input.GetKey (KeyCode.Space)){
					wheel.wheelCollider.brakeTorque = 1000;
				_wfc.stiffness = 0.7f;
			}

			wheel.wheelCollider.sidewaysFriction = _wfc;

		}
	}
}
