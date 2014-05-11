using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;

//using System.Xml.Li
public class PlaneManager : MonoBehaviour {
	public GameObject plane_;
	public ArrayList planes;
	public ArrayList bgPlanes;
	public ArrayList fgPlanes;
	
	public float timeBetweenHeroes = 2.0f;
	public float timeForFirstHero = 1.0f;
	public static int numBgPlanes = 10;
	public static int numPlanes = 10; //refractor to be numFgPlanes
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
	List<Vector3> positions; 
	List<Vector3> vect3positions; 
	
	List<Texture2D> images; 
	
	
	// Use this for initialization
	void Start()
	{
		heroTimer = timeBetweenHeroes - timeForFirstHero;
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
		loadNodePositions(); 
		
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
		//		for (int i = 0; i < numPlanes; i++)
		//		{
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
		
		
		//		}
		//		counter ++;
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
			//loadNodePositions(); 
			//testAddfgPlaneData();
		}
		//will allow for new positions to be written while Unity is running 
		//		if(Input.GetKeyDown(KeyCode.R))
		//			 SaveNodes();
		
	}
	
	public void loadNodePositions() 
	{	 
		positions = new List<Vector3>(); 
		vect3positions = new List<Vector3>(); 
		foreach(string pos in File.ReadAllLines("./Assets/data/dataPositions.txt"))
		{
			Debug.Log(pos); 
			Vector3 num = stripData(pos);
			positions.Add(num); 
		}
		IEnumerable<Vector3> sorted = positions.OrderBy(v => v.z);
		foreach(Vector3 vect in sorted)
		{		
			Debug.Log(vect.z + "I am sorted"); 
			vect3positions.Add(vect); 
		}
		//vect3positions = positions.OrderBy(v =>v.z).ToArray<Vector3>(); 
		
		//testing
		//		for(int i =0; i <vect3positions.Count; i++) 
		//		{
		//			Debug.Log(vect3positions[i]);
		//			Vector3 testVect = (Vector3)positions[i]; 
		//		} 
		
		for(int i = 0; i <fgPlanes.Count; i++)
		{
			GameObject temp = fgPlanes[i] as GameObject; 
			temp.transform.position= vect3positions[i];
			
		}
		for(int i=0; i<bgPlanes.Count; i++)
		{
			GameObject temp = bgPlanes[i] as GameObject;	
			temp.transform.position = vect3positions[i+numPlanes];
		}
		
	}
	public Vector3 stripData(string sourceString) 
	{
		
		Vector3 outVector3; 
		string outString;
		string[] splitString; 
		//trim parenthesis 
		outString = sourceString.Substring(1,sourceString.Length -2); 
		
		//split delimted values into an array 
		splitString = outString.Split("," [0]); 
		outVector3.x = float.Parse(splitString[0]); 
		outVector3.y = float.Parse(splitString[1]); 
		outVector3.z = float.Parse(splitString[2]); 
		int index = 0; 
		return outVector3; 		
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
			using (System.IO.StreamWriter file = new System.IO.StreamWriter("./Assets/data/dataPositions.txt", true))
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
			//			Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			//			GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
			//			p.SetActive(true);
			//			p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
			//			float randomY = (float)NextGaussianDouble(Random.RandomRange(-3.0f, 3.0f), 3.5);
			//			float randomX = (float)NextGaussianDouble(Random.RandomRange(-10.0f, 10.0f), 4.5) ;
			//			p.transform.position = new Vector3(randomX, randomY, Random.RandomRange(47.0f, 67.0f) );
			GameObject p = SpawnPanel(Random.Range(47.0f, 67.0f));
			p = addbgPanelData(p); 
			bgPlanes.Add(p); 
		}
		Debug.Log( "BG Planes: " + bgPlanes.Count );
		StartCoroutine(TryToSpawnBG());
	}
	
	public void InitForgroundPanels()
	{
		fgPlanes = new ArrayList();
		for( int i=0; i < numPlanes; i++) {
			//			Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			//			GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
			//			p.SetActive(true);
			//			p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
			//			float randomY = (float)NextGaussianDouble(Random.RandomRange(-5.0f, 5.0f), 5.5);
			//			float randomX = (float)NextGaussianDouble(Random.RandomRange(-12.0f, 12.0f), 5.5) ;
			//			p.transform.position = new Vector3(randomX, randomY, Random.RandomRange(20.0f, 45.0f) );
			GameObject p = SpawnPanel(Random.Range(20.0f, 45.0f));
			p = addfgPanelData(p); 
//			Debug.Log(p + "should totally exhist"); 
			fgPlanes.Add(p); 
		}
		Debug.Log( "fg Planes: " + fgPlanes.Count );
		StartCoroutine(TryToSpawnFG());
	}
	
	public GameObject SpawnPanel(float depth){
		Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
		GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
		p.SetActive(true);
		p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
		float randomY = (float)NextGaussianDouble(Random.RandomRange(-5.0f, 5.0f), 5.5);
		float randomX = (float)NextGaussianDouble(Random.RandomRange(-12.0f, 12.0f), 5.5) ;
		p.transform.position = new Vector3(randomX, randomY, depth );
		return p;
	}
	
	//function for adding foreground data 
	public GameObject addfgPanelData(GameObject plane)
	{
//		Debug.Log(plane+" is a fg"); 
		Person p = DataPuller.PullNewHero(); 
		Transform[] ts = plane.GetComponentsInChildren<Transform>();
		foreach(Transform t in ts) 
		{
			if (t.gameObject.name == "NameText")
			{
//				Debug.Log(p.familyName +"just pulled onto a chip"); 
				t.gameObject.GetComponent<TextMesh>().text =  p.familyName.ToUpper() + " " + p.givenName.ToUpper() + " (" + p.lifespan + ")";
			}
			if (t.gameObject.name == "LocationText") 
			{
				t.gameObject.GetComponent<TextMesh>().text = p.location.ToUpper(); 
			}
			if(t.gameObject.name == "BodyTextMesh") 
			{
				t.gameObject.GetComponent<TextMesh>().text = p.description; 
			}
			if(t.gameObject.name == "Photo") 
			{
				//handle stupid file name. trunkake it for Unity 
				string s = p.filename;
				int index = s.IndexOf('.'); 
				string sn = ""; 
				//make a substring w/out file name 
				if(index >=0)
				{
					sn = s.Substring(0,index); 
				}
				
				//Debug.Log (sn + " should be file name"); 
				string sl = "photos/" + sn; 
				//Debug.Log(sl);
				Texture2D image = Resources.Load(sl) as Texture2D; 
				//Debug.Log(image + "is the file");
				t.gameObject.GetComponent<Renderer>().material.SetTexture("_image", image); 
				
			}
		}
		return plane; 
	}
	
	public GameObject addbgPanelData(GameObject plane)
	{
		Person p = DataPuller.PullNewNormalPerson(); 

		Transform[] ts = plane.GetComponentsInChildren<Transform>();
		foreach(Transform t in ts) 
		{
			if (t.gameObject.name == "NameText")
			{
			//	Debug.Log(p.familyName +"just pulled onto a chip"); 
				t.gameObject.GetComponent<TextMesh>().text =  p.familyName.ToUpper() + " " + p.givenName.ToUpper() + " (" + p.lifespan + ")";
			}
			if (t.gameObject.name == "LocationText") 
			{
				t.gameObject.GetComponent<TextMesh>().text = p.location.ToUpper(); 
			}
			//			if(t.gameObject.name == "BodyTextMesh") 
			//			{
			//				t.gameObject.GetComponent<TextMesh>().text = p.description; 
			//			}
			if(t.gameObject.name == "Photo") 
			{
				//handle stupid file name. trunkake it for Unity 
				string s = p.filename;
				int index = s.IndexOf('.'); 
				string sn = ""; 
				//make a substring w/out file name 
				if(index >=0)
				{
					sn = s.Substring(0,index); 
				}
				
				//Debug.Log (sn + " should be file name"); 
				string sl = "photos/" + sn; 
				//Debug.Log(sl);
				Texture2D image = Resources.Load(sl) as Texture2D; 
				//Debug.Log(image + "is the file");
				t.gameObject.GetComponent<Renderer>().material.SetTexture("_image", image); 
				
			}

			//track person 
		}
		plane = SetDataBaseNumber(plane, p); 
		return plane;
			
	}
	public GameObject SetDataBaseNumber(GameObject plane, Person p) 
	{
		plane.GetComponent<SetUpText>().trackDatabasePostition = p.id;  
		return plane; 
	}
	//test data case 
	//	public void testAddfgPlaneData()
	//	{
	//		Vector3 pos = new Vector3(0.0f, 0.0f, -10.0f); 
	//		GameObject g = Instantiate(plane_, pos, Quaternion.identity) as GameObject; 
	//		g.SetActive(true); 
	//		addbgPanelData(g);  
	//	}
	//	
	
	public IEnumerator TryToSpawnBG() {
		float t = Random.Range(4.0f, 10.0f);
		while (t > 0.0f) {
			t -= Time.deltaTime;
			yield return 0;
		}
		if (bgPlanes.Count < numBgPlanes) {
			GameObject p = SpawnPanel(Random.Range(47.0f, 67.0f)); 
			addbgPanelData(p); 
			
		}
		StartCoroutine(TryToSpawnBG());
		StartCoroutine(TryToRemoveBG());
	}
	
	public IEnumerator TryToSpawnFG() {
		//		Debug.Log("TryToSpawn");
		float t = Random.Range(5.0f, 15.0f);
		while (t > 0.0f) {
			t -= Time.deltaTime;
			yield return 0;
		}
		if (fgPlanes.Count < numPlanes) {
			//			Debug.Log("spawning new FG panel");
			
			GameObject p =SpawnPanel(Random.Range(20.0f, 45.0f)); 
			addfgPanelData(p); 

		}
		StartCoroutine(TryToSpawnFG());
	}
	
	public IEnumerator TryToRemoveBG() {
		float t = Random.Range(4.0f, 10.0f);
		while (t > 0.0f) {
			t -= Time.deltaTime;
			yield return 0;
		}
		if (bgPlanes.Count > 0) {
			GameObject plane = bgPlanes[Random.Range(0, bgPlanes.Count-1)] as GameObject;
			int id = plane.GetComponent<SetUpText>().trackDatabasePostition; 
			Person p = DataPuller.findCurrentPerson(id); 
			if(p.hero == "no")
			{
				Debug.Log("not a hero to remove" + p.id); 
				DataPuller.RemoveNormalPersonFromActiveList(p); 
			}
			else if(p.hero == "yes")
			{
				Debug.Log("hero to remove" + p.id); 
				DataPuller.RemoveHeroFromActiveList(p); 
			}
			plane.GetComponent<SetUpText>().fadeOut();
		}
		StartCoroutine(TryToRemoveBG());
	}
	
}