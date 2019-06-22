using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(DetailMapApplier))]
public class DetailMapApplierEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();
		DetailMapApplier TargetScript = (DetailMapApplier)target;
		if (GUILayout.Button ("Apply detail maps"))
			TargetScript.ApplyDetailMaps ();
		if (GUILayout.Button ("Delete detail maps"))
			TargetScript.DeleteDetailMaps ();
	}
		
}
#endif
