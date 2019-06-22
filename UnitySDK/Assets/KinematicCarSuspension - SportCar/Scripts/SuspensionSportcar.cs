using UnityEngine;
using System.Collections;

[System.Serializable]
public class FrontWheel {
	public GameObject 
		KnuckleTarget,
		SteeringDummy,
		CamberPlate,
		Driveshaft,
		DriveshaftTarget,
		DriveshaftDuster,
		TieRod,
		TieRodEnd,
		SteeringRackDuster,
		Spring,
		AbsorberDuster,
		AbsorberCap,
		Wishbone,
		WishboneTarget,
		Wheel;

	[HideInInspector]
	public WheelCollider wheelCollider;

	[HideInInspector]
	public Vector3 
		TieRodDefPos, 
		AbsorberCapDefPos,
		DriveshaftDefScale,
		WishboneTargetDefPos,
		KnuckleTargetDefPos,
		WCPosition;

	[HideInInspector] 
	public float 
		Delta,
		SpringHeight,
		AbsorberDusterHeight,
		Travel,
		Compression,
		AngularVelocity,
		StartTime,
		TieRodOffset,
	SpringDefHeight,
	SteeringDusterDefWidth,
	AbsorberDusterDefHeight,
	DriveshaftDefWidth;

	[HideInInspector] 
	public bool 
		Grounded,
		SlowMoving;

	[HideInInspector]public Quaternion WCRotation;
	[HideInInspector]public WheelHit wheelhit;

}

[System.Serializable]
public class RearWheel{
	public GameObject 
		CamberPlate,
		BrakeDisk,
		Driveshaft,
		DriveshaftTarget,
		CVJointInner,
		CVJointOuter,
		Spring,
		AbsorberCap,
		Absorber,
		AbsorberTarget,
		CamberAxle,
		Wishbone,
		WishboneTarget,
		Wheel;

	[HideInInspector]
	public WheelCollider wheelCollider;

	[HideInInspector]
	public Vector3 
		AbsorberCapDefPos, 
		WCPosition,
		DriveshaftDefScale;

	[HideInInspector] 
	public float 
		SpringHeight,
		Travel,
		Delta,
		AngularVelocity,
		Compression,
		StartTime,
		SpringDefHeight,
		DriveshaftDefWidth;

	[HideInInspector] public bool 
		Grounded, 
		SlowMoving;

	[HideInInspector]public Quaternion WCRotation;
	[HideInInspector]public WheelHit wheelhit;

}



[System.Serializable]
public class OtherElements {
	public GameObject 
		SwayBarLeftPart,
		SwayBarRightPart,
		SwayBarBridge,
		SteeringRackCenter,
		SubframeFLeft,
		SubframeFRight,
		SubframeFBridge,
		SteeringWheel,
		SteeringCoupling,
		SteeringShaft;

	public GameObject 
		SubframeRLeft,
		SubframeRRight,
		SubframeRBridgeL,
		SubframeRBridgeR,
		DriveshaftRear1,
		DriveshaftRear2,
		CardanStart,
		CardanBridge,
		CardanEnd;

	[HideInInspector]
	public Vector3 
	SubframeRLeftDefPos,
	SubframeRRightDefPos,
	SubframeRBridgeLDefScale,
	SubframeRBridgeRDefScale,
	SubframeFLeftDefPos,
	SubframeFRightDefPos,
	SubframeFBridgeDefScale,
	SteeringWheelDefRot,
	SwayBarBridgeDefScale,

	SteeringCouplingDefRot;

	[HideInInspector]
	public float 
	SubframeFBridgeDefWidth,
	SwayBarBridgeDefWidth,
	SubframeRBridgeLDefWidth,
	SubframeRBridgeRDefWidth,
	SteeringWheelAngleFloat,
	SteeringCouplingAngleFloat;




}


public class SuspensionSportcar : MonoBehaviour {

	[Header("System")]
	public FrontWheel WheelFL;
	public FrontWheel WheelFR;
	public RearWheel WheelRL;
	public RearWheel WheelRR;
	public OtherElements otherElements;

	private FrontWheel[] frontWheels;
	private RearWheel[] rearWheels;

	[Header("Wheel colliders")]
	public WheelCollider WheelColliderFL;
	public WheelCollider WheelColliderFR;
	public WheelCollider WheelColliderRL;
	public WheelCollider WheelColliderRR;

	[Header("Adjustments")]

	[Range(100,110)]
	public float FrontAxisWidth=100;
	[Range(100,110)]
	public float RearAxisWidth=100;
	[Range(0,20)]
	public float FrontCamber;
	[Range(0,20)]
	public float RearCamber;
	[Range (1,1.3f)]
	public float WheelsRadius;
	[Range(1,1.5f)]
	public float WheelsWidth;

	[Range(-0.2f,0.2f)]
	public float Caster;

	[Range(-5.5f,5.5f)]
	public float FrontToe;

	[Range(-15f,10f)]
	public float RearToe;

	public float 
	CompressionLimitMin,
	CompressionLimitMax ;

	public int CompressionMultiplier;

	private Vector3 
		_defWishboneTargetRotation,
		SteeringRackCenterDefPos;

	private float 
		WheelsDefRadius,
		WCDefRadius, 
		WheelsDefWidth,
		SteeringRackOffset,
		SmoothSteeringInput;

	[HideInInspector] public bool EVPConnected;

	[Range(0,0.4f)]
	public float FrontSpringsOffset,RearSpringsOffset;

	[Range(3,8)]
	public float SteeringWheelAngleMultiplier;

	void Start () {
		InitializeSuspension ();
	}

	void InitializeSuspension(){
		frontWheels = new FrontWheel[2] {WheelFL,WheelFR};
		rearWheels = new RearWheel[2]{ WheelRL, WheelRR };

		WheelFL.wheelCollider = WheelColliderFL;
		WheelFR.wheelCollider = WheelColliderFR;
		WheelRL.wheelCollider = WheelColliderRL;
		WheelRR.wheelCollider = WheelColliderRR;

		_defWishboneTargetRotation = WheelFL.WishboneTarget.transform.localEulerAngles;

		SteeringRackCenterDefPos = otherElements.SteeringRackCenter.transform.localPosition;

		foreach (var wheel in frontWheels) {
			wheel.TieRodDefPos = transform.InverseTransformPoint (wheel.TieRodEnd.transform.position);
			wheel.DriveshaftDefScale = wheel.Driveshaft.transform.localScale;
			wheel.Delta = wheel.AbsorberDuster.transform.position.y - wheel.Spring.transform.position.y;
			wheel.AbsorberCapDefPos = wheel.AbsorberCap.transform.localPosition;
			wheel.WishboneTargetDefPos = wheel.WishboneTarget.transform.localPosition;
			wheel.KnuckleTargetDefPos = wheel.KnuckleTarget.transform.localPosition;
			wheel.wheelCollider.ConfigureVehicleSubsteps (50, 50, 50);

			wheel.SpringDefHeight = wheel.Spring.AddComponent<BoxCollider> ().size.z;
			Destroy (wheel.Spring.GetComponent<BoxCollider> ());
			wheel.SteeringDusterDefWidth = wheel.SteeringRackDuster.AddComponent<BoxCollider> ().size.x;
			Destroy (wheel.SteeringRackDuster.GetComponent<BoxCollider> ());
			wheel.AbsorberDusterDefHeight = wheel.AbsorberDuster.AddComponent<BoxCollider> ().size.z;
			Destroy (wheel.AbsorberDuster.GetComponent<BoxCollider> ());
			wheel.DriveshaftDefWidth = wheel.Driveshaft.transform.GetChild(0).gameObject.AddComponent<BoxCollider> ().size.x;
			Destroy (wheel.Driveshaft.transform.GetChild(0).gameObject.GetComponent<BoxCollider> ());
		}

		foreach (var wheel in rearWheels) {
			wheel.AbsorberCapDefPos = wheel.AbsorberCap.transform.localPosition;
			wheel.Delta = wheel.Wheel.transform.position.y - wheel.Wishbone.transform.position.y;
			wheel.DriveshaftDefScale = wheel.Driveshaft.transform.localScale;
			wheel.wheelCollider.ConfigureVehicleSubsteps (50, 50, 50);

			wheel.DriveshaftDefWidth = wheel.Driveshaft.transform.GetChild(0).gameObject.AddComponent<BoxCollider> ().size.x;
			Destroy (wheel.Driveshaft.transform.GetChild(0).gameObject.GetComponent<BoxCollider> ());
			wheel.SpringDefHeight = wheel.Spring.AddComponent<BoxCollider> ().size.z;
			Destroy (wheel.Spring.GetComponent<BoxCollider> ());
		}


		//OTHER
		WCDefRadius = WheelColliderFL.radius;
		WheelsDefRadius = WheelFL.Wheel.transform.localScale.z;
		WheelsRadius = WheelsDefRadius;
		WheelsDefWidth = WheelFL.Wheel.transform.localScale.x;
		WheelsWidth = WheelsDefWidth;

		otherElements.SubframeFLeftDefPos = otherElements.SubframeFLeft.transform.localPosition;
		otherElements.SubframeFRightDefPos =otherElements.SubframeFRight.transform.localPosition;
		otherElements.SubframeFBridgeDefScale = otherElements.SubframeFBridge.transform.localScale;
		otherElements.SwayBarBridgeDefScale = otherElements.SwayBarBridge.transform.localScale;
		otherElements.SubframeRLeftDefPos = otherElements.SubframeRLeft.transform.localPosition;
		otherElements.SubframeRRightDefPos = otherElements.SubframeRRight.transform.localPosition;
		otherElements.SubframeRBridgeLDefScale = otherElements.SubframeRBridgeL.transform.localScale;
		otherElements.SubframeRBridgeRDefScale = otherElements.SubframeRBridgeR.transform.localScale;
		otherElements.SteeringWheelDefRot = otherElements.SteeringWheel.transform.localEulerAngles;
		otherElements.SteeringCouplingDefRot = otherElements.SteeringCoupling.transform.localEulerAngles;

		otherElements.SwayBarBridgeDefWidth = otherElements.SwayBarBridge.AddComponent<BoxCollider> ().size.x;
		Destroy (otherElements.SwayBarBridge.GetComponent<BoxCollider> ());
		otherElements.SubframeFBridgeDefWidth = otherElements.SubframeFBridge.AddComponent<BoxCollider> ().size.x;
		Destroy (otherElements.SubframeFBridge.GetComponent<BoxCollider> ());
		otherElements.SubframeRBridgeLDefWidth = otherElements.SubframeRBridgeL.AddComponent<BoxCollider> ().size.x;
		Destroy (otherElements.SubframeRBridgeL.GetComponent<BoxCollider> ());
		otherElements.SubframeRBridgeRDefWidth = otherElements.SubframeRBridgeR.AddComponent<BoxCollider> ().size.x;
		Destroy (otherElements.SubframeRBridgeR.GetComponent<BoxCollider> ());
	}


	void OnDrawGizmos(){
		if (Application.isPlaying) return;

		otherElements.CardanEnd.transform.LookAt (otherElements.CardanStart.transform, otherElements.DriveshaftRear2.transform.forward);
		otherElements.CardanStart.transform.LookAt (otherElements.CardanEnd.transform, otherElements.DriveshaftRear1.transform.forward);

		float _cardanEndLength=0.43f;
		float _cardanBridgeLength = 0.111f;
		float _distance = Vector3.Distance (otherElements.CardanEnd.transform.position, otherElements.CardanBridge.transform.position);
		float _pureDistance = _distance - _cardanEndLength - _cardanBridgeLength;

		Vector3 TempScale = otherElements.CardanBridge.transform.localScale;
		TempScale.y = _pureDistance / _cardanBridgeLength + 1;
		otherElements.CardanBridge.transform.localScale = TempScale;

		float SteeringShaftDefLength = 0.61f;
		_distance = Vector3.Distance (otherElements.SteeringCoupling.transform.position, otherElements.SteeringShaft.transform.position);
		_pureDistance = _distance - SteeringShaftDefLength;
		otherElements.SteeringShaft.transform.LookAt (otherElements.SteeringCoupling.transform,otherElements.SteeringWheel.transform.up);
		TempScale = otherElements.SteeringShaft.transform.localScale;
		TempScale.z = _pureDistance / SteeringShaftDefLength + 1;
		otherElements.SteeringShaft.transform.localScale = TempScale;
	}
		

	void FixedUpdate () {
		GetBasicParameters ();
		DoWheelsRadius ();
		DoSubframeWidth ();
		DoCamberToeCaster ();
		DoSteeringRack ();
		DoTieRods ();
		DoCompression ();
		DoRotation ();
		DoSwayBar ();
		DoDriveshafts ();

	}


	void GetBasicParameters(){
		foreach (var wheel in frontWheels) {
			if (!EVPConnected) wheel.AngularVelocity = wheel.wheelCollider.rpm / (0.02f / Time.fixedDeltaTime) / 8.3f;
			if (!wheel.wheelCollider.isGrounded) {
				wheel.SlowMoving = false;
				wheel.StartTime = Time.time;
			}

			if ((Time.time - wheel.StartTime) > 0.3f)
				wheel.SlowMoving = true;

			wheel.SpringHeight = wheel.SpringDefHeight;
			wheel.AbsorberDusterHeight = wheel.AbsorberDusterDefHeight;
			wheel.Grounded = wheel.wheelCollider.GetGroundHit (out wheel.wheelhit);
			wheel.Travel = (-wheel.wheelCollider.transform.InverseTransformPoint (wheel.wheelhit.point).y - wheel.wheelCollider.radius);
			wheel.wheelCollider.GetWorldPose (out wheel.WCPosition, out wheel.WCRotation);
		}

		foreach (var wheel in rearWheels) {
			if (!EVPConnected) wheel.AngularVelocity = wheel.wheelCollider.rpm / (0.02f / Time.fixedDeltaTime) / 8.3f;
			if (!wheel.wheelCollider.isGrounded) {
				wheel.SlowMoving = false;
				wheel.StartTime = Time.time;
			}

			if ((Time.time - wheel.StartTime) > 0.3f)
				wheel.SlowMoving = true;

			wheel.SpringHeight = wheel.SpringDefHeight;
			wheel.Grounded = wheel.wheelCollider.GetGroundHit (out wheel.wheelhit);
			wheel.Travel = (-wheel.wheelCollider.transform.InverseTransformPoint (wheel.wheelhit.point).y - wheel.wheelCollider.radius);
			wheel.wheelCollider.GetWorldPose (out wheel.WCPosition, out wheel.WCRotation);
		}
	}

	void DoWheelsRadius(){
		foreach (var wheel in frontWheels) {
			Vector3 TempScale = wheel.Wheel.transform.localScale;
			TempScale = new Vector3 (WheelsWidth, WheelsRadius, WheelsRadius);
			wheel.Wheel.transform.localScale = TempScale;
			wheel.wheelCollider.radius = WCDefRadius * WheelsRadius;
		}

		foreach (var wheel in rearWheels) {
			Vector3 TempScale = wheel.Wheel.transform.localScale;
			TempScale = new Vector3 (WheelsWidth, WheelsRadius, WheelsRadius);
			wheel.Wheel.transform.localScale = TempScale;
			wheel.wheelCollider.radius = WCDefRadius * WheelsRadius;
		}
	}

	void DoSubframeWidth(){
		Vector3 TempPos = otherElements.SubframeFLeftDefPos;
		TempPos.x -= (FrontAxisWidth / 100) - 1;
		otherElements.SubframeFLeft.transform.localPosition = TempPos;

		TempPos = otherElements.SubframeFRightDefPos;
		TempPos.x += (FrontAxisWidth / 100) - 1;
		otherElements.SubframeFRight.transform.localPosition = TempPos;

		Vector3 TempScale = otherElements.SubframeFBridgeDefScale;
		TempScale.x = 1 + (FrontAxisWidth/100-1) / otherElements.SubframeFBridgeDefWidth * 2;
		otherElements.SubframeFBridge.transform.localScale = TempScale;

		TempPos = otherElements.SubframeRLeftDefPos;
		TempPos.x -= (RearAxisWidth / 100) - 1;
		otherElements.SubframeRLeft.transform.localPosition = TempPos;

		TempPos = otherElements.SubframeRRightDefPos;
		TempPos.x += (RearAxisWidth / 100) - 1;
		otherElements.SubframeRRight.transform.localPosition = TempPos;

		TempScale = otherElements.SubframeRBridgeLDefScale;
		TempScale.x = 1 + (RearAxisWidth / 100 - 1) / otherElements.SubframeRBridgeLDefWidth;
		otherElements.SubframeRBridgeL.transform.localScale = TempScale;

		TempScale = otherElements.SubframeRBridgeRDefScale;
		TempScale.x = 1 + (RearAxisWidth / 100 - 1) / otherElements.SubframeRBridgeRDefWidth;
		otherElements.SubframeRBridgeR.transform.localScale = TempScale;

	}

	void DoDriveshafts(){
		foreach (var wheel in frontWheels) {
			Vector3 TempScale = wheel.DriveshaftDefScale;
			TempScale.z += (FrontAxisWidth / 100 - 1 - FrontCamber / 1000) / wheel.DriveshaftDefWidth;
			wheel.Driveshaft.transform.localScale = TempScale;
			wheel.DriveshaftDuster.transform.Rotate(new Vector3 (wheel.AngularVelocity, 0, 0), Space.Self);
			wheel.Driveshaft.transform.LookAt (wheel.DriveshaftTarget.transform, wheel.CamberPlate.transform.up);

		}

		foreach(var wheel in rearWheels){
			Vector3 TempScale = wheel.DriveshaftDefScale;
			TempScale.z += (RearAxisWidth / 100 - 1 - RearCamber / 1000) / wheel.DriveshaftDefWidth;
			wheel.Driveshaft.transform.localScale = TempScale;
			wheel.Driveshaft.transform.LookAt (wheel.DriveshaftTarget.transform, wheel.BrakeDisk.transform.up);
		}

		otherElements.DriveshaftRear1.transform.Rotate (new Vector3 (0, WheelRL.AngularVelocity, 0));
		otherElements.DriveshaftRear2.transform.Rotate (new Vector3 (0, WheelRL.AngularVelocity, 0));
		otherElements.CardanEnd.transform.LookAt (otherElements.CardanStart.transform, otherElements.DriveshaftRear2.transform.forward);
		otherElements.CardanStart.transform.LookAt (otherElements.CardanEnd.transform, otherElements.DriveshaftRear1.transform.forward);
	}

	void DoSwayBar(){
		otherElements.SwayBarLeftPart.transform.LookAt (otherElements.SwayBarRightPart.transform,transform.up);
		otherElements.SwayBarRightPart.transform.LookAt (otherElements.SwayBarLeftPart.transform,transform.up);

		Vector3 TempScale = otherElements.SwayBarBridgeDefScale;
		TempScale.x = 1 + (FrontAxisWidth/100-1) / otherElements.SwayBarBridgeDefWidth*2 ;
		otherElements.SwayBarBridge.transform.localScale = TempScale;
	}

	void DoCamberToeCaster(){
		//FL
		WheelFL.SteeringDummy.transform.LookAt (WheelFL.KnuckleTarget.transform, WheelFL.WishboneTarget.transform.right + WheelFL.WishboneTarget.transform.up * FrontToe);
		Vector3 TempPos = WheelFL.KnuckleTargetDefPos;
		TempPos.x += FrontCamber/100;
		TempPos.y += Caster;
		WheelFL.KnuckleTarget.transform.localPosition = TempPos;

		//FR
		WheelFR.SteeringDummy.transform.LookAt (WheelFR.KnuckleTarget.transform, WheelFR.WishboneTarget.transform.right - WheelFR.WishboneTarget.transform.up * FrontToe);
		TempPos = WheelFR.KnuckleTargetDefPos;
		TempPos.x -= FrontCamber/100;
		TempPos.y += Caster;
		WheelFR.KnuckleTarget.transform.localPosition = TempPos;

		//RL
		Vector3 TempRot = WheelRL.CamberAxle.transform.localEulerAngles;
		TempRot.x = -RearCamber;
		TempRot.y = RearToe;
		WheelRL.CamberAxle.transform.localEulerAngles = TempRot;

		//RR
		TempRot = WheelRR.CamberAxle.transform.localEulerAngles;
		TempRot.x = -RearCamber;
		TempRot.y = -RearToe;
		WheelRR.CamberAxle.transform.localEulerAngles = TempRot;
	}
		

	void DoSteeringRack(){
		Vector3 TempPos = otherElements.SteeringRackCenter.transform.localPosition;
		TempPos.x = SteeringRackCenterDefPos.x - (WheelFL.TieRodOffset+WheelFR.TieRodOffset)/2;
		otherElements.SteeringRackCenter.transform.localPosition = TempPos;
		SteeringRackOffset = SteeringRackCenterDefPos.x - otherElements.SteeringRackCenter.transform.localPosition.x;

		Vector3 TempScale = WheelFL.SteeringRackDuster.transform.localScale;
		TempScale.x=1 - SteeringRackOffset/WheelFL.SteeringDusterDefWidth;
		WheelFL.SteeringRackDuster.transform.localScale = TempScale;

		TempScale = WheelFR.SteeringRackDuster.transform.localScale;
		TempScale.x=1 + SteeringRackOffset/WheelFR.SteeringDusterDefWidth;
		WheelFR.SteeringRackDuster.transform.localScale = TempScale;


		float TargetSteeringWheelAngle = otherElements.SteeringWheelDefRot.z + frontWheels[0].wheelCollider.steerAngle * SteeringWheelAngleMultiplier;

		otherElements.SteeringWheelAngleFloat = Mathf.MoveTowards (otherElements.SteeringWheelAngleFloat, TargetSteeringWheelAngle, 1);

		Vector3 tempSteeringWheelRot = otherElements.SteeringWheel.transform.localEulerAngles;
		tempSteeringWheelRot.z =otherElements.SteeringWheelDefRot.z + otherElements.SteeringWheelAngleFloat;
		otherElements.SteeringWheel.transform.localEulerAngles = tempSteeringWheelRot; 

		float TargetSteeringCouplingAngle = otherElements.SteeringCouplingDefRot.z - frontWheels[0].wheelCollider.steerAngle * SteeringWheelAngleMultiplier;

		otherElements.SteeringCouplingAngleFloat = Mathf.MoveTowards (otherElements.SteeringCouplingAngleFloat, TargetSteeringCouplingAngle, 1);

		Vector3 tempSteeringCouplingRot = otherElements.SteeringCoupling.transform.localEulerAngles;
		tempSteeringCouplingRot.z =otherElements.SteeringCouplingDefRot.z + otherElements.SteeringCouplingAngleFloat;
		otherElements.SteeringCoupling.transform.localEulerAngles = tempSteeringCouplingRot; 

		otherElements.SteeringShaft.transform.LookAt (otherElements.SteeringCoupling.transform,otherElements.SteeringWheel.transform.up);
	}

	void DoTieRods(){
		foreach (var wheel in frontWheels) {
			wheel.TieRod.transform.LookAt(wheel.TieRodEnd.transform);
			wheel.TieRodEnd.transform.LookAt (wheel.TieRod.transform);
			wheel.TieRodOffset =wheel.TieRodDefPos.x - transform.InverseTransformPoint (wheel.TieRodEnd.transform.position).x;
		}
	}

	void DoCompression(){
		foreach (var wheel in frontWheels) {
			wheel.Compression= Mathf.MoveTowards(wheel.Compression, 1 - (wheel.wheelCollider.suspensionDistance*0.4f - wheel.Travel + FrontSpringsOffset)*CompressionMultiplier,Time.deltaTime);
			wheel.Compression = Mathf.Clamp (wheel.Compression, CompressionLimitMin, CompressionLimitMax);

			Vector3 TempScale = wheel.Spring.transform.localScale;
			TempScale.z = wheel.Compression;
			wheel.Spring.transform.localScale = TempScale;

			TempScale = wheel.AbsorberDuster.transform.localScale;
			TempScale.z = (wheel.Compression * wheel.SpringHeight - wheel.Delta) / wheel.AbsorberDusterHeight;
			wheel.AbsorberDuster.transform.localScale = TempScale;

			Vector3 TempPos = wheel.AbsorberCap.transform.localPosition;
			TempPos.z = wheel.AbsorberCapDefPos.z - wheel.SpringHeight * (1 - wheel.Compression);
			wheel.AbsorberCap.transform.localPosition = TempPos;

			TempPos = wheel.WishboneTarget.transform.localPosition;
			if (wheel.SlowMoving) TempPos.z =Mathf.MoveTowards(TempPos.z, wheel.WishboneTarget.transform.parent.InverseTransformPoint(wheel.WCPosition).z - (wheel.Wheel.transform.position.y - wheel.SteeringDummy.transform.position.y),Time.deltaTime*3);
			else TempPos.z =wheel.WishboneTarget.transform.parent.InverseTransformPoint(wheel.WCPosition).z - (wheel.Wheel.transform.position.y - wheel.SteeringDummy.transform.position.y);
			wheel.WishboneTarget.transform.localPosition = TempPos;
		}

		foreach (var wheel in rearWheels) {
			wheel.Compression= Mathf.MoveTowards(wheel.Compression, 1 - (wheel.wheelCollider.suspensionDistance*0.4f - wheel.Travel + RearSpringsOffset)*CompressionMultiplier,Time.deltaTime);
			wheel.Compression = Mathf.Clamp (wheel.Compression, CompressionLimitMin, CompressionLimitMax);

			Vector3 TempPos=wheel.WishboneTarget.transform.localPosition;
			if (wheel.SlowMoving) TempPos.z = Mathf.MoveTowards(TempPos.z, wheel.WishboneTarget.transform.parent.transform.InverseTransformPoint (wheel.WCPosition).z - wheel.Delta,Time.deltaTime);
			else TempPos.z = wheel.WishboneTarget.transform.parent.transform.InverseTransformPoint (wheel.WCPosition).z - wheel.Delta;
			wheel.WishboneTarget.transform.localPosition = TempPos;

			wheel.Absorber.transform.LookAt (wheel.AbsorberTarget.transform,wheel.CamberPlate.transform.up);

			Vector3 TempScale = wheel.Spring.transform.localScale;
			TempScale.z = wheel.Compression;
			wheel.Spring.transform.localScale = TempScale;

			TempPos = wheel.AbsorberCap.transform.localPosition;
			TempPos.z = wheel.AbsorberCapDefPos.z - wheel.SpringHeight * (1 - wheel.Compression);
			wheel.AbsorberCap.transform.localPosition = TempPos;
		}
	}

	void DoRotation(){
		foreach (var wheel in frontWheels) {
			Quaternion WishboneTargetRot = Quaternion.Euler (_defWishboneTargetRotation + new Vector3 (0,0 , wheel.wheelCollider.steerAngle));
			wheel.WishboneTarget.transform.localRotation = Quaternion.RotateTowards (wheel.WishboneTarget.transform.localRotation, WishboneTargetRot, 5 / (0.02f / Time.fixedDeltaTime));
			wheel.CamberPlate.transform.Rotate (new Vector3 (wheel.AngularVelocity * (1-Mathf.Abs(FrontToe/2)), 0, 0), Space.Self);
			wheel.Wishbone.transform.LookAt (wheel.WishboneTarget.transform, transform.up);
		}

		foreach (var wheel in rearWheels) {
			wheel.BrakeDisk.transform.Rotate (new Vector3 (wheel.AngularVelocity, 0, 0), Space.Self);
			wheel.Wishbone.transform.LookAt (wheel.WishboneTarget.transform,transform.up);
			wheel.CVJointOuter.transform.Rotate(new Vector3 (wheel.AngularVelocity, 0, 0), Space.Self);
			wheel.CVJointInner.transform.Rotate(new Vector3 (wheel.AngularVelocity, 0, 0), Space.Self);
		}
	}

}
