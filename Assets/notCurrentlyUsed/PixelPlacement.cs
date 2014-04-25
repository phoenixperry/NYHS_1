using UnityEngine;
using System.Collections;

public class PixelPlacement : MonoBehaviour {
    public Camera cam;
    public float pixelRatio; 
	// Use this for initialization
	void Start () {
        pixelRatio = (cam.orthographicSize * 2) / camera.pixelHeight;
        Debug.Log(pixelRatio + "i am ratio"); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
