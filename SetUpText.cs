using UnityEngine;
using System.Collections;

public class SetUpText : MonoBehaviour {
    //code for adding database text to screen 
    public GUIText name;
    public GUIText location;
    public GUIText description;
    //you put the data on the camera encase you forgot 
    GameObject data; 
	// Use this for initialization
	void Start () {
        data = GameObject.Find("Data"); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void GetData() {
   
    }
}
