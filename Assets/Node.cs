﻿using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	// ------   properties ------
	// if needed, an ID for the node
	string id = "";
	float diameter = 0.0f;
	
	public float minX = -float.MaxValue;
	public float maxX = float.MaxValue;
	public float minY = -float.MaxValue;
	public float maxY = float.MaxValue;
	public float minZ = -float.MaxValue;
	public float maxZ = float.MaxValue;
	
	public Vector3 velocity;
	public Vector3 pVelocity;
	public float maxVelocity = 2;
	
	public float damping = 1f;
	// radius of impact
	public float radius = 50;
	// strength: positive for attraction, negative for repulsion (default for Nodes)
	public float strength = -1;
	// parameter that influences the form of the function
	public float ramp = 1.0f;
	public Vector3 pos; //start pos 
	// Use this for initialization

	void rotateX(float theAngle) 
	{
		float newy = pos.y * Mathf.Cos(theAngle) - pos.z * Mathf.Sin(theAngle);
		float newz = pos.y * Mathf.Sin(theAngle) + pos.z * Mathf.Cos(theAngle);
		pos.y = newy;
		pos.z = newz; 
	}

	void rotateY(float theAngle) {
		float newx = pos.x * Mathf.Cos(-theAngle) - pos.z * Mathf.Sin(-theAngle);
		float newz = pos.x * Mathf.Sin(-theAngle) + pos.z * Mathf.Cos(-theAngle);
		pos.x = newx;
		pos.z = newz;
	}
	
	void rotateZ(float theAngle) {
		float newx = pos.x * Mathf.Cos(theAngle) - pos.y * Mathf.Sin(theAngle);
		float newy = pos.x * Mathf.Sin(theAngle) + pos.y * Mathf.Cos(theAngle);
		pos.x = newx;
		pos.y = newy;
	}
	// ------ calculate attraction ------
	void attract(ArrayList theNodes) {
		// attraction or repulsion part
		for (int i = 0; i < theNodes.Count; i++) {
			Node otherNode = theNodes[i] as Node;
			// stop when empty
			// if (otherNode == null) break;
			// not with itself
			if (otherNode == this) continue;
			
			this.attract(otherNode);
		}
	}

	void attract(Node theNode) {
		float d = Vector3.Distance(gameObject.transform.position, theNode.pos);
		
		if (d > 0 && d < radius) {
			float s = Mathf.Pow(d / radius, 1 / ramp);
			float f = s * 9 * strength * (1 / (s + 1) + ((s - 3) / 4)) / d;
			//this script is going to need to live each node
			Vector3 df = gameObject.transform.position- theNode.pos;
			df = df *f;
			
			theNode.velocity.x += df.x;
			theNode.velocity.y += df.y;
			theNode.velocity.z += df.z;
		}
	}
	// Update is called once per frame

	void Update() {
		
		//velocity.limit(maxVelocity); (gotta do this the long ass way in Unity 
		
		pVelocity = velocity;

		
		if (pos.x < minX) {
			pos.x = minX - (pos.x - minX);
			velocity.x = -velocity.x;
		}
		if (pos.x > maxX) {
			pos.x = maxX - (pos.x - maxX);
			velocity.x = -velocity.x;
		}
		
		if (pos.y < minY) {
			pos.y = minY - (pos.y - minY);
			velocity.y = -velocity.y;
		}
		if (pos.y > maxY) {
			pos.y = maxY - (pos.y - maxY);
			velocity.y = -velocity.y;
		}
		
		if (pos.z < minZ) {
			pos.z = minZ - (pos.z - minZ);
			velocity.z = -velocity.z;
		}
		if (pos.z > maxZ) {
			pos.z = maxZ - (pos.z - maxZ);
			velocity.z = -velocity.z;
		}
		
		velocity = velocity * (1.0f - damping);
	}
	
	// ------ getters and setters ------
	string getID() {
		return id;
	}
	
	void setID(string theID) {
		this.id = theID;
	}
	




}
