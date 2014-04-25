using UnityEngine;
using System.Collections;

public abstract class Node
{
	public GameObject node; 
	public float xpos, ypos, zpos ; 
	public Vector3 loc; 
	public bool heroStatus; 
	
	Node (bool hero)
	{
		heroStatus = hero;
	}


	public void setPosition ()
	{

	}
	public void display ()
	{

		Transform t = node.transform.GetComponent<Transform> ();
		t.position = loc; 
	}




}
