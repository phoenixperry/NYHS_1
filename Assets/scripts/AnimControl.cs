using UnityEngine;
using System.Collections;

public class AnimControl : MonoBehaviour {
    Animator anim; 
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>(); 
	}
	
	// Update is called once per frame
	void Update () {
//        if (Input.GetKey(KeyCode.C))
//        {
//            anim.SetTrigger("node_open_fade_in"); 
//
//        }
//        if (Input.GetKey(KeyCode.D))
//        {
//            anim.SetTrigger("node_close");
//        }
	}

	public void OpenNode() {
		anim.SetTrigger("node_open_fade_in");
	}

	public void CloseNode() {
		anim.SetTrigger("node_close");
	}

}
