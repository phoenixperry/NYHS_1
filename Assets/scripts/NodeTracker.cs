using UnityEngine;
using System.Collections;

public class NodeTracker : MonoBehaviour {

    //BEN this is for you 
    //this file's only task should be to keep a list of the current active nodes 
    //this file should also give the plane object a number it should use for the node it should pull out of the data puller 
    //see dataFlow.png in our dropbox at the root of this project's folder for visualization

    //temp only using till logic is in place to get something to work with 
    public static Person p; 

	// Use this for initialization
	void Start () {

	    //set dummy hero node up for now for dev purposes 
        DataPuller.num = 0;
//        gameObject.GetComponent<DataPuller>().dataItem(); 
   //     p = DataPuller.currentHero; 


 

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}