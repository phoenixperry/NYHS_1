using UnityEngine;
using System.Collections;
//handy little script to make stuff move on arrow key presses. Good for debugging
public class Mover : MonoBehaviour {
    public float xpos = 0.0f;
    public float zpos = 0.0f;
    public float speed = 500.0f;
    public Camera cam; 
	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
       /// Debug.Log(Time.fixedDeltaTime);
        Debug.Log(Input.GetAxis("Horizontal") + "axis");
        xpos = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        zpos = Input.GetAxis("Vertical") * Time.fixedDeltaTime;
       // transform.Translate(xpos, zpos, 0);    
        transform.position =cam.ScreenToWorldPoint(Input.mousePosition);
	}
}
