using UnityEngine;
using System.Collections;

public class ForceRenderOrder : MonoBehaviour {

	public int renderQ = 0;

	// Use this for initialization
	void Start () {
		renderer.material.renderQueue = renderQ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
