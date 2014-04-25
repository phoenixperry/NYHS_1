using UnityEngine;
using System.Collections;

public class Path:ScriptableObject
{
    public Vector3 start;
    public Vector3 end;
    public float radius = 1.0f;
    public void Start()
    {
    }

}
public class AgentSinglePath : MonoBehaviour {
    public Vector3 start, end;
    Vector3 location, velocity, accel;
    float r, maxforce, maxspeed;
    public GameObject boundingBox;
    public float radius = 1.0f;
    

    void Setup(Vector3 l, float ms, float mf)
    {

        maxspeed = 0.5f;
        maxforce = 0.5f;
        start = new Vector3(-5, -5, -2);
        end = new Vector3(5, 5, -2);
        location = l;
        maxforce = mf;
        maxspeed = ms;
        r = 1.0f;
        accel = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = new Vector3(maxspeed, 0.5f, 0.0f);
      
    
    }

    void Start()
    {
        gameObject.transform.position = start; 
        Setup(gameObject.transform.position, 0.5f, 0.5f);


    } 
   

	void Update () {
        //Debug.Log(p.start + "i am start"); 
        follow(start,end);
        run();
        render();
       // transform.position += velocity *Time.deltaTime;
 
       
	}
    void follow(Vector3 s, Vector3 e)
    {
        //this gets the current location and projects forward 
        Vector3 predict = transform.position ;
        velocity.z = -2.0f; //making sure our 
        //velocity doesn't cause havac 
        predict.Normalize();
        predict += velocity;//WE DON'T WANT THIS ON ALL THREE VECTORS 
        predict.z = -2;
        Vector3 predictLoc = location + predict;


        Vector3 a = s;
        Vector3 b = e;

        Vector3 normalPoint = getNormalPoint
            (predictLoc, a, b);
        Vector3 dir = b - a;
        dir.Normalize();
        dir *= 0.05f;  //move it in a way that works with meters 
        Vector3 target = normalPoint + dir;
        float distance = Vector3.Distance(normalPoint, dir);
        if (distance > 1.0f)
            seek(target);
        //move 
       // transform.position = target; 
    }
    Vector3 getNormalPoint(Vector3 p, Vector3 a, Vector3 b) {
        Vector3 ap = p - a;
        Vector3 ab = b - a;
        ab.Normalize();
        ab *= Vector3.Dot(ap, ab); //check you this is ap dot ab 
        Vector3 normalPoint = a + ab;
        return normalPoint; 

    }
    void run()
    {
        velocity += accel; 
        //you should limit vel with clamp 
//        velocity = Mathf.Clamp(maxspeed); 
        location += velocity;
        accel *= 0.0f; 

    }

    void render() { }

    void seek(Vector3 target)
    {
        Vector3 desired = target - location;
        if (desired.magnitude == 0) return;

        desired.Normalize();
        desired *= maxspeed; 

        Vector3 steer = desired - velocity;

        //Debug.Log(width + "i am the width"); 
        Vector3 min = -1 * (boundingBox.GetComponent<Renderer>().bounds.size / 2);
        min = min * maxspeed;

        Vector3 max = boundingBox.GetComponent<Renderer>().bounds.size / 2;
        max = max * maxspeed;

        steer.x = Mathf.Clamp(steer.x, min.x, maxforce);
        steer.y = Mathf.Clamp(steer.y, min.y, maxforce);
        steer.z = Mathf.Clamp(steer.z, min.z, maxforce);
        //Debug.Log(steer + "I am where to seer slowly");
        applyForce(steer);
    }
    void applyForce(Vector3 force)
    {

        accel += force; 
    }
}
