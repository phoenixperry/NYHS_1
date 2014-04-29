using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	// ------   properties ------
	// if needed, an ID for the node
	string id = "";
	float diameter = 0.0f;
	
	public float minX = 0.0f;
	public float maxX = 0.0f;
	public float minY = 0.0f;
	public float maxY = 0.0f;
	public float minZ = 0.0f;
	public float maxZ = 0.0f;
	
	private Vector3 velocity;
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

	public void rotateX(float theAngle) 
	{
		float newy = pos.y * Mathf.Cos(theAngle) - pos.z * Mathf.Sin(theAngle);
		float newz = pos.y * Mathf.Sin(theAngle) + pos.z * Mathf.Cos(theAngle);
		pos.y = newy;
		pos.z = newz; 
	}

	public void rotateY(float theAngle) {
		float newx = pos.x * Mathf.Cos(-theAngle) - pos.z * Mathf.Sin(-theAngle);
		float newz = pos.x * Mathf.Sin(-theAngle) + pos.z * Mathf.Cos(-theAngle);
		pos.x = newx;
		pos.z = newz;
	}
	
	public void rotateZ(float theAngle) {
		float newx = pos.x * Mathf.Cos(theAngle) - pos.y * Mathf.Sin(theAngle);
		float newy = pos.x * Mathf.Sin(theAngle) + pos.y * Mathf.Cos(theAngle);
		pos.x = newx;
		pos.y = newy;
	}
	// ------ calculate attraction ------
	public void attract(ArrayList theNodes) {
		// attraction or repulsion part
		for (int i = 0; i < theNodes.Count; i++) {
			GameObject otherNode = theNodes[i] as GameObject;
			// stop when empty
			// if (otherNode == null) break;
			// not with itself
			if (otherNode == this) continue;
			
			attractIt(otherNode);
		}
	}

	public void attractIt(GameObject theNode) {

		float d = Vector3.Distance(gameObject.transform.position, theNode.GetComponent<Node>().pos);

		if (d > 0 && d < radius) {
			float s = Mathf.Pow(d / radius, 1 / ramp);
			float f = s * .9f * strength * (.1f / (s + .1f) + ((s - .3f) / .4f)) / d; 
			Debug.Log(f); 
			//this script is going to need to live each node
			Vector3 df = gameObject.transform.position - theNode.GetComponent<Node>().pos;
			df.Normalize();
			df = df *f;
//			
//			theNode.GetComponent<Node>().velocity.x += df.x;
//			theNode.GetComponent<Node>().velocity.y += df.y;
//			theNode.GetComponent<Node>().velocity.z += df.z;
		}
	}


	void Update() {
	//	pVelocity = velocity; 

//		velocity.x = Mathf.Clamp(velocity.x, -1, maxVelocity);
//		velocity.y = Mathf.Clamp(velocity.y, -1, maxVelocity);
//		velocity.z = Mathf.Clamp(velocity.z, -1, maxVelocity);
//
//		if (pos.x < minX) {
//			pos.x = minX - (pos.x - minX);
//			velocity.x = -velocity.x;
//		}
//		if (pos.x > maxX) {
//			pos.x = maxX - (pos.x - maxX);
//			velocity.x = -velocity.x;
//		}
//		
//		if (pos.y < minY) {
//			pos.y = minY - (pos.y - minY);
//			velocity.y = -velocity.y;
//		}
//		if (pos.y > maxY) {
//			pos.y = maxY - (pos.y - maxY);
//			velocity.y = -velocity.y;
//		}
//		
//		if (pos.z < minZ) {
//			pos.z = minZ - (pos.z - minZ);
//			velocity.z = -velocity.z;
//		}
//		if (pos.z > maxZ) {
//			pos.z = maxZ - (pos.z - maxZ);
//			velocity.z = -velocity.z;
//		}
//		
//		velocity = velocity * (1.0f - damping);
	

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
		
		if (pos.z > minZ) {
			pos.z = minZ - (pos.z - minZ);
			velocity.z = -velocity.z;
		}
		if (pos.z < maxZ) {
			pos.z = maxZ - (pos.z - maxZ);
			velocity.z = -velocity.z;
		}
		
		velocity = velocity * (1 - damping);
		//	
	}
	
	// ------ getters and setters ------
	public string getID() {
		return id;
	}
	
	public void setID(string theID) {

		id = theID;
	}

	public void setBoundary(Vector3 theMin,
	                 Vector3 theMax) {
		minX = theMin.x;
		maxX = theMax.x;
		minY = theMin.y;
		maxY = theMax.y;
		minZ = theMin.z;
		maxZ = theMax.z;
	}





}
