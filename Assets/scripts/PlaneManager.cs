using UnityEngine;
using System.Collections;

public class PlaneManager : MonoBehaviour {
    public GameObject plane_;
    public ArrayList planes;
    public int numPlanes = 10;
    public float radius = 8;
    public float radiusX = 10; 
    public float startAngle, range;
    public float speed = 2.0f;
    public Quaternion rotation;
    public Vector3 rotationRadius;
    public float currentRotation = 0.0f; 
    private int counter = 0;

	// Use this for initialization
    void Awake()
    {

        rotationRadius = new Vector3(0.5f, 0.0f, 0.0f);

        planes = new ArrayList();
        startAngle = 360 / numPlanes;
        for (int i = 0; i < numPlanes; i++)
        {
            //instantiate plane rotated up 
            Quaternion r = Quaternion.Euler(90.0f, 180.0f, 0.0f);
            GameObject p = Instantiate(plane_, transform.position, r) as GameObject;

            //radius from center 
            float radiusRange = Random.RandomRange(0.0f, -2.0f);

            p.GetComponent<PlaneSetup>().radius = radiusRange + radius;
            p.GetComponent<PlaneSetup>().radiusX = radiusRange + radiusX;
            float randomHeight = (float)NextGaussianDouble();
            randomHeight = Random.RandomRange(-5.0f, 5.0f) * randomHeight;
            p.transform.position = new Vector3(p.transform.position.x, randomHeight, p.transform.position.z);
            planes.Add(p);
            Debug.Log(planes.Count);
        }
        //update start angle after going through
        for (int i = 0; i < numPlanes; i++)
        {
            GameObject g = planes[i] as GameObject;
            g.GetComponent<PlaneSetup>().startAngle = startAngle * i;

        }
        Debug.Log("num of planes " + planes.Count);
    }

    public static double NextGaussianDouble()
    {
        double U, u, v, S;

        do
        {
            u = 2.0 * Random.value - 1.0;
            v = 2.0 * Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);
        float s_ = (float)S;
        float Ss = (float)(-2.0 * Mathf.Log(s_) / S);
        float fac = Mathf.Sqrt(Ss);
        return u * fac;
    }
    public void FixedUpdate()
    {
        for (int i = 0; i < numPlanes; i++)
        {
            GameObject g = planes[i] as GameObject;

            //sine method

            //g.GetComponent<PlaneSetup>().pos.x = (int)g.GetComponent<PlaneSetup>().radiusX * (Mathf.Cos(Time.realtimeSinceStartup * speed + g.GetComponent<PlaneSetup>().startAngle));
            //g.GetComponent<PlaneSetup>().pos.z = (int)g.GetComponent<PlaneSetup>().radius * Mathf.Sin(Time.realtimeSinceStartup * speed + g.GetComponent<PlaneSetup>().startAngle);
            //g.GetComponent<PlaneSetup>().pos.y = g.GetComponent<Transform>().position.y;
            //////offset

            //g.GetComponent<PlaneSetup>().pos.z += 20;

            g.GetComponent<PlaneSetup>().posLerp.x = Mathf.Lerp(g.transform.position.x, g.GetComponent<PlaneSetup>().pos.x, .5f);

            g.GetComponent<PlaneSetup>().posLerp.z = Mathf.Lerp(g.transform.position.z, g.GetComponent<PlaneSetup>().pos.z, .5f);
            g.GetComponent<PlaneSetup>().posLerp.y = g.GetComponent<Transform>().position.y;

            g.transform.position = g.GetComponent<PlaneSetup>().posLerp;


        }
        counter ++; 
    }

    public void Update()
    {
      
        
    }
    public void spin()
    {

            // for (int i = 0; i < numPlanes; i++ )
            //{   
            //    GameObject g = planes[i] as GameObject;

            //    //sine method
            //    g.GetComponent<PlaneSetup>().pos.x = g.GetComponent<PlaneSetup>().radiusX * (Mathf.Cos((Time.time*speed) + g.GetComponent<PlaneSetup>().startAngle));
            //    g.GetComponent<PlaneSetup>().pos.z = g.GetComponent<PlaneSetup>().radius * Mathf.Sin((Time.time*speed) + g.GetComponent<PlaneSetup>().startAngle);
            //    g.GetComponent<PlaneSetup>().pos.y = g.GetComponent<Transform>().position.y;
            //    ////offset
            //    g.GetComponent<PlaneSetup>().pos.z += 20;
            
            //    g.GetComponent<PlaneSetup>().posLerp.x = Mathf.Lerp(g.transform.position.x,g.GetComponent<PlaneSetup>().pos.x, .5f);

            //    g.GetComponent<PlaneSetup>().posLerp.z = Mathf.Lerp(g.transform.position.z, g.GetComponent<PlaneSetup>().pos.z, .5f);
            //    g.GetComponent<PlaneSetup>().posLerp.y = g.GetComponent<Transform>().position.y;
            //    g.transform.position = g.GetComponent<PlaneSetup>().posLerp; 

            //}
             Invoke("spin", 0.0f);
    }


}
