using UnityEngine;
using System.Collections;

public class SwapTexture : MonoBehaviour {
    public Texture2D photo; 

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = photo;
	    //stub out a  method to get a new piece of data for this
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            renderer.material.mainTexture = photo;
//
//        }
	}

    // Update is called once per frame
    void Update()
    {
        
	}
}
