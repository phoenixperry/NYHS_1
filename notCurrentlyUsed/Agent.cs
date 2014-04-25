using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
    Vector3 location, velocity, accel;
    public float r, maxforce, maxspeed;
    public Camera cam;
    public GameObject m;
    public GameObject boundingBox; 
    void Setup(Vector3 loc)
    {
        accel = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
        location = loc;
 
        maxforce = 0.5f;
        maxspeed = 0.05f;
        Debug.Log(location + "location in viewport" + maxforce + " maxforce" + maxspeed + " maxspeed");
    }
	// Use this for initialization
	void Start () {
        Setup(transform.position); 
	}

	// Update is called once per frame
	void Update () {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
       
        seek(m.transform.position);
        mouseWorld.z = -2.0f;
    //        Debug.Log(mouseWorld);

        velocity += accel;
        //velocity.x = Mathf.Clamp(velocity.x, 0.0f, maxspeed);
        //velocity.y = Mathf.Clamp(velocity.y, 0.0f, maxspeed);
        //velocity.z = Mathf.Clamp(velocity.z, 0.0f, maxspeed);
        location += velocity;
        accel *= 0.0f;
        location.z = -2.0f;

        transform.position = location;
        Debug.Log(location + "I am where you want to go");
       // transform.Translate(location);  
	}
     
    void applyForce(Vector3 force)
    {
        accel += force; 
    }
    // A method that calculates a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
   void seek(Vector3 target)
    {   
      
        Vector3 desired = target - location;
        desired.z = -1; 
        float d = desired.magnitude;
        desired = desired.normalized;
        Debug.Log(d + " am the mag");
        //slow down on arrival 
        if (d < 1.8f)
        {
            //move half the speed if you are in the range 
            //in procecssing you can map this because the value is going from 0 to 100 and you can rescale that to be a 
            //0 to maxspeed but because of how unity's meter system works if you make max speed a whole number
            //stuff flys around like crazy and this is useless. Just compensate. 
            desired *= d*maxspeed * 0.5f;
        }
        else
        {
            desired *= maxspeed;
        }
            Vector3 steer = desired - velocity;
        
        //Debug.Log(width + "i am the width"); 
        Vector3 min = -1*(boundingBox.GetComponent<Renderer>().bounds.size/2);
        min = min * maxspeed;

        Vector3 max = boundingBox.GetComponent<Renderer>().bounds.size / 2;
        max = max * maxspeed; 

        steer.x = Mathf.Clamp(steer.x, min.x, maxforce);
        steer.y = Mathf.Clamp(steer.y, min.y, maxforce);
       steer.z = Mathf.Clamp(steer.z, min.z, maxforce);
        applyForce(steer);
        //Debug.Log(steer + "I am where to seer slowly");

   }

   float SuperLerp(float from, float to, float from2, float to2, float value)
   {

       if (value <= from2)

           return from;

       else if (value >= to2)

           return to;

       return (to - from) * ((value - from2) / (to2 - from2)) + from;
   }
}
