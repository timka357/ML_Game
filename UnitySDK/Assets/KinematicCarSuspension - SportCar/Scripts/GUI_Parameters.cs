using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI_Parameters : MonoBehaviour {

	public GameObject CarBody;
	public bool ShowGUI;

	private float 
		FrontSpringsLength, 
		RearSpringsLength,
		SpringsStiffness,
		AbsorbersDamping,
		FrontSpringsDefLength,
		RearSpringsDefLength,
		SpringsDefStiffness,
		AbsorbersDefDamping;
	private GameObject SelectedCarBody;
	private bool ShowCarBody;
	private SuspensionSportcar SuspensionSportcarScript;


	void Start(){
		SuspensionSportcarScript = GetComponentInChildren<SuspensionSportcar> ();

		if (SuspensionSportcarScript == null) {
			Debug.LogError ("Suspension sport car script is not found on " + gameObject.name);
			return;
		}

		FrontSpringsDefLength = SuspensionSportcarScript.WheelColliderFL.suspensionDistance;
		RearSpringsDefLength = SuspensionSportcarScript.WheelColliderRL.suspensionDistance;
		SpringsDefStiffness = SuspensionSportcarScript.WheelColliderFL.suspensionSpring.spring;
		AbsorbersDefDamping = SuspensionSportcarScript.WheelColliderFL.suspensionSpring.damper;

	}


	void OnGUI(){
		if (!ShowGUI || SuspensionSportcarScript==null) return;

		GUIStyle newGUI = "label";
		newGUI.normal.textColor = Color.white;

		GUI.Box (new Rect (0, 0, 250, 340), "");

		GUI.Label (new Rect (10, 10, 100, 20), "Front camber");
		SuspensionSportcarScript.FrontCamber = GUI.HorizontalSlider (new Rect (130, 15, 100, 20), SuspensionSportcarScript.FrontCamber, 0, 20);

		GUI.Label (new Rect (10, 30, 100, 20), "Front axis width");
		SuspensionSportcarScript.FrontAxisWidth = GUI.HorizontalSlider (new Rect (130, 35, 100, 20),SuspensionSportcarScript.FrontAxisWidth, 100, 110);

		GUI.Label (new Rect (10, 50, 100, 20), "Rear camber");
		SuspensionSportcarScript.RearCamber = GUI.HorizontalSlider (new Rect (130, 55, 100, 20), SuspensionSportcarScript.RearCamber, 0, 20);

		GUI.Label (new Rect (10, 70, 100, 20), "Rear axis width");
		SuspensionSportcarScript.RearAxisWidth = GUI.HorizontalSlider (new Rect (130, 75, 100, 20), SuspensionSportcarScript.RearAxisWidth, 100, 110);

		GUI.Label (new Rect (10, 90, 100, 20), "Wheels radius");
		SuspensionSportcarScript.WheelsRadius = GUI.HorizontalSlider (new Rect (130, 95, 100, 20), SuspensionSportcarScript.WheelsRadius, 1, 1.3f);

		GUI.Label (new Rect (10, 110, 100, 20), "Wheels width");
		SuspensionSportcarScript.WheelsWidth = GUI.HorizontalSlider (new Rect (130, 115, 100, 20), SuspensionSportcarScript.WheelsWidth, 1, 1.5f);

		GUI.Label (new Rect (10, 130, 100, 20), "Caster");
		SuspensionSportcarScript.Caster = GUI.HorizontalSlider (new Rect (130, 135, 100, 20), SuspensionSportcarScript.Caster, -0.3f, 0.3f);

		GUI.Label (new Rect (10, 150, 100, 20), "Front toe");
		SuspensionSportcarScript.FrontToe = GUI.HorizontalSlider (new Rect (130, 155, 100, 20), SuspensionSportcarScript.FrontToe, -0.5f, 0.5f);

		GUI.Label (new Rect (10, 170, 100, 20), "Rear toe");
		SuspensionSportcarScript.RearToe = GUI.HorizontalSlider (new Rect (130, 175, 100, 20), SuspensionSportcarScript.RearToe, -15f, 10f);

		GUI.Label (new Rect (10, 190, 130, 20), "Front springs length");
		FrontSpringsLength = GUI.HorizontalSlider (new Rect (130, 195, 100, 20), FrontSpringsLength, -0.2f, 0.1f);

		GUI.Label (new Rect (10, 210, 130, 20), "Rear springs length");
		RearSpringsLength = GUI.HorizontalSlider (new Rect (130, 215, 100, 20), RearSpringsLength, -0.2f, 0.1f);

		GUI.Label (new Rect (10, 230, 100, 20), "Springs stiffness");
		SpringsStiffness = GUI.HorizontalSlider (new Rect (130, 235, 100, 20), SpringsStiffness, -20000f, 20000f);

		GUI.Label (new Rect (10, 250, 130, 20), "Absorbers damping");
		AbsorbersDamping = GUI.HorizontalSlider (new Rect (130, 255, 100, 20), AbsorbersDamping, -2000f, 2000f);

		ShowCarBody = GUI.Toggle (new Rect (10, 270, 100, 20), ShowCarBody, "Show car body");

		GUI.Label (new Rect (10, 290, 150, 20), "Press C to Slow-Mo");

		GUI.Label (new Rect (10, 310, 150, 20), "Hold RMB to free cam");
	}


	void Update(){
		if (SuspensionSportcarScript==null) return;

		SuspensionSportcarScript.WheelColliderFL.suspensionDistance = FrontSpringsDefLength + FrontSpringsLength;
		SuspensionSportcarScript.WheelColliderFR.suspensionDistance = FrontSpringsDefLength + FrontSpringsLength;
		SuspensionSportcarScript.WheelColliderRL.suspensionDistance = RearSpringsDefLength + RearSpringsLength;
		SuspensionSportcarScript.WheelColliderRR.suspensionDistance = RearSpringsDefLength + RearSpringsLength;

		JointSpring _newSpring = SuspensionSportcarScript.WheelColliderFL.suspensionSpring;
		_newSpring.spring = SpringsDefStiffness + SpringsStiffness;
		_newSpring.damper = AbsorbersDefDamping + AbsorbersDamping;
		SuspensionSportcarScript.WheelColliderFL.suspensionSpring = _newSpring;
		SuspensionSportcarScript.WheelColliderFR.suspensionSpring = _newSpring;
		SuspensionSportcarScript.WheelColliderRL.suspensionSpring = _newSpring;
		SuspensionSportcarScript.WheelColliderRR.suspensionSpring = _newSpring;

		ControlBody ();

	}

	void ControlBody(){
		if (CarBody == null)
			return;

		if (ShowCarBody && !CarBody.activeSelf)
			CarBody.SetActive (true);

		if (!ShowCarBody && CarBody.activeSelf)
			CarBody.SetActive (false);
	}


}
