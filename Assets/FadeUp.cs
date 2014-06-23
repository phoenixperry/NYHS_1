using UnityEngine;
using System.Collections;

public class FadeUp : MonoBehaviour {
	Color  colorShift;	
	float alpha =0.0f; 
	// Use this for initialization
	void Start () {
		colorShift = gameObject.renderer.material.color; 
		colorShift.a = 0.0f; 
		gameObject.renderer.material.color = colorShift; 
	}
	
	// Update is called once per frame
	void Update () {
			alpha = Mathf.Lerp(0.0f, 1.0f, 2.0f);
			colorShift.a = alpha; 
			gameObject.renderer.material.color  = colorShift; 

		 
	}
}
