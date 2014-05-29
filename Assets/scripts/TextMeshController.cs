using UnityEngine;
using System.Collections;

public class TextMeshController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (this.ToString() + " " + transform.renderer.material.color);
//		if (GetComponent<DynamicText>() != null) {
//			GetComponent<DynamicText>().color = renderer.material.color;
//		}
	}
}
