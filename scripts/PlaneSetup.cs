using UnityEngine;
using System.Collections;

public class PlaneSetup : MonoBehaviour {

	//this is the current data holder for the gold plane's positional data
    public float radius = 6;
    public float radiusX = 8;
    public float startAngle, range;
    public Vector3 pos = new Vector3();

    public Vector3 posLerp = new Vector3(); 


    public GameObject data;
    public GameObject photo;

    public void pullPhoto()
    {
        transform.position = new Vector3(10.0f, 1.0f, 1.0f);
    }


}


