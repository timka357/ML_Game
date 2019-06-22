using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DetailMapApplier : MonoBehaviour {

	public GameObject[] gameObjects;
	public Texture DiffuseMap;
	public Texture NormalMap;

	public void ApplyDetailMaps(){
		foreach (var go in gameObjects) {
			go.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture ("_DetailAlbedoMap", DiffuseMap);
			go.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture ("_DetailNormalMap", NormalMap);
		}
	}

	public void DeleteDetailMaps(){
		foreach (var go in gameObjects) {
			go.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture ("_DetailAlbedoMap", null);
			go.GetComponent<MeshRenderer> ().sharedMaterial.SetTexture ("_DetailNormalMap", null);
		}

	}


}


