using UnityEngine;
using System.Collections;

public class PlaneManager : MonoBehaviour {
	public GameObject plane_;
	public ArrayList planes;
	public ArrayList bgPlanes;
	public ArrayList fgPlanes;
	public int numPlanes = 10;
	public float timeBetweenHeroes = 2.0f;
	public int numBgPlanes = 10;
	public float radius = 8;
	public float radiusX = 10;
	public float startAngle, range;
	public float speed = 2.0f;
	public Quaternion rotation;
	public Vector3 rotationRadius;
	public float currentRotation = 0.0f;
	private int counter = 0;
	
	private float heroTimer;
	private GameObject heroInFocus;
	
	// Use this for initialization
	void Awake()
	{
		heroTimer = 0.0f;
		//        rotationRadius = new Vector3(0.5f, 0.0f, 0.0f);
		//
		//        planes = new ArrayList();
		//        startAngle = 360 / (numPlanes == 0 ? 1 : numPlanes);
		//        for (int i = 0; i < numPlanes; i++)
		//        {
		//            //instantiate plane rotated up
		//            Quaternion r = Quaternion.Euler(90.0f, 180.0f, 0.0f);
		//            GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
		//
		//            //radius from center
		//            float radiusRange = Random.RandomRange(0.0f, -2.0f);
		//
		//            p.GetComponent<PlaneSetup>().radius = radiusRange + radius;
		//            p.GetComponent<PlaneSetup>().radiusX = radiusRange + radiusX;
		//            float randomHeight = (float)NextGaussianDouble();
		//            randomHeight = Random.RandomRange(-5.0f, 5.0f) * randomHeight;
		//            p.transform.position = new Vector3(p.transform.position.x, randomHeight, p.transform.position.z);
		//            planes.Add(p);
		//            Debug.Log(planes.Count);
		//        }
		//update start angle after going through
		//        for (int i = 0; i < numPlanes; i++)
		//        {
		//            GameObject g = planes[i] as GameObject;
		//            g.GetComponent<PlaneSetup>().startAngle = startAngle * i;
		//
		//        }
		//        Debug.Log("num of planes " + planes.Count);
		InitBackgroundPanels();
		InitForgroundPanels(); 
	}
	
	public static double NextGaussianDouble(double mu = 0.0, double sigma = 1.0)
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
		return u * fac * sigma + mu;
	}
	public void FixedUpdate()
	{
		for (int i = 0; i < numPlanes; i++)
		{
			//            GameObject g = planes[i] as GameObject;
			
			//sine method
			
			//g.GetComponent<PlaneSetup>().pos.x = (int)g.GetComponent<PlaneSetup>().radiusX * (Mathf.Cos(Time.realtimeSinceStartup * speed + g.GetComponent<PlaneSetup>().startAngle));
			//g.GetComponent<PlaneSetup>().pos.z = (int)g.GetComponent<PlaneSetup>().radius * Mathf.Sin(Time.realtimeSinceStartup * speed + g.GetComponent<PlaneSetup>().startAngle);
			//g.GetComponent<PlaneSetup>().pos.y = g.GetComponent<Transform>().position.y;
			//////offset
			
			//g.GetComponent<PlaneSetup>().pos.z += 20;
			
			//g.GetComponent<PlaneSetup>().posLerp.x = Mathf.Lerp(g.transform.position.x, g.GetComponent<PlaneSetup>().pos.x, .5f);
			
			// g.GetComponent<PlaneSetup>().posLerp.z = Mathf.Lerp(g.transform.position.z, g.GetComponent<PlaneSetup>().pos.z, .5f);
			// g.GetComponent<PlaneSetup>().posLerp.y = g.GetComponent<Transform>().position.y;
			
			// g.transform.position = g.GetComponent<PlaneSetup>().posLerp;
			
			
		}
		counter ++;
	}
	
	public void Update()
	{
		heroTimer += Time.deltaTime;
		if(heroTimer >= timeBetweenHeroes) {
			heroTimer = 0.0f;
			if (fgPlanes.Count > 0) {
				heroInFocus = fgPlanes[0] as GameObject;
				FocusOnHero(heroInFocus);
				fgPlanes.RemoveAt(0);
			}
		}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveNodes();
        } 
        

		
	}
    public void SaveNodes()
    {
        ArrayList positions = new ArrayList(); 
        foreach (GameObject bg in bgPlanes)
        {
            Vector3 pos = bg.transform.position;
            positions.Add(pos); 
        }

        foreach (GameObject fg in fgPlanes)
        {
            Vector3 pos = fg.transform.position;
            positions.Add(pos);
        }
        Debug.Log("you have " + positions.Count + "number of positions");
        foreach(Vector3 pos in positions){
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\data_test\dataPositions.txt", true))
        {
            file.WriteLine(pos);
        }
        }
    } 
	public void FocusOnHero(GameObject hero)
	{
		hero.GetComponent<SetUpText>().moveToCenter();
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
	
	public void InitBackgroundPanels()
	{
		bgPlanes = new ArrayList();
		for( int i=0; i < numBgPlanes; i++) {
			Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
			p.SetActive(true);
			p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
			float randomY = (float)NextGaussianDouble(Random.RandomRange(-3.0f, 3.0f), 3.5);
			float randomX = (float)NextGaussianDouble(Random.RandomRange(-10.0f, 10.0f), 4.5) ;
			p.transform.position = new Vector3(randomX, randomY, Random.RandomRange(47.0f, 67.0f) );
			bgPlanes.Add(p);
		}
		Debug.Log( "BG Planes: " + bgPlanes.Count );
	}
	
	public void InitForgroundPanels()
	{
		fgPlanes = new ArrayList();
		for( int i=0; i < numPlanes; i++) {
			Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
			p.SetActive(true);
			p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
			float randomY = (float)NextGaussianDouble(Random.RandomRange(-5.0f, 5.0f), 5.5);
			float randomX = (float)NextGaussianDouble(Random.RandomRange(-12.0f, 12.0f), 5.5) ;
			p.transform.position = new Vector3(randomX, randomY, Random.RandomRange(20.0f, 45.0f) );
			fgPlanes.Add(p);
		}
		Debug.Log( "fg Planes: " + fgPlanes.Count );
	}
	
	
}