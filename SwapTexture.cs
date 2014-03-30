using UnityEngine;
using System.Collections;

public class SwapTexture : MonoBehaviour {
    public Texture2D hazel; 
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            renderer.material.mainTexture = hazel; 
        
        }
	}
}
