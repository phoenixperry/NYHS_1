using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;
//using System.Xml.Li

public class SpawnPoint {
	public Vector3  position;
	public bool		occupied;
}


public class PlaneManager : MonoBehaviour {
	public GameObject plane_;
	public ArrayList planes;
	public ArrayList bgPlanes;
	public ArrayList fgPlanes;
	
	public float timeBetweenHeroes = 2.0f;
	public float timeForFirstHero = 1.0f;
	public float spawnBgDelay_Min = 4.0f;
	public float spawnBgDelay_Max = 10.0f;
	public float removeBgDelay_Min = 4.0f;
	public float removeBgDelay_Max = 10.0f;
	public float spawnFgDelay_Min = 4.0f;
	public float spawnFgDelay_Max = 10.0f;
	public static int numBgPlanes = 30;
	public static int numPlanes = 7; //refractor to be numFgPlanes
	public float radius = 8;
	public float radiusX = 10;
	public float startAngle, range;
	public float speed = 2.0f;
	public Quaternion rotation;
	public Vector3 rotationRadius;
	public float currentRotation = 0.0f;
	private int counter = 0;

//	public Person nextPerson;

	private float heroTimer;
	private GameObject heroInFocus;
	List<Vector3> positions; 
	List<Vector3> vect3positions; 

	List<SpawnPoint> heroPositions;
	List<SpawnPoint> normalPositions;
	
	List<Texture2D> images; 
	
	
	// Use this for initialization
	void Start()
	{
		heroTimer = timeBetweenHeroes - timeForFirstHero;
		loadNodePositions();
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

	}
	
	public void Update()
	{
		heroTimer += Time.fixedDeltaTime;
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
		heroPositions = new List<SpawnPoint>();
		normalPositions = new List<SpawnPoint>();

		foreach(string pos in File.ReadAllLines("./Assets/data/dataPositions.txt"))
		{
			SpawnPoint sp = new SpawnPoint();

			sp.position = stripData(pos);
			sp.occupied = false;

			if(sp.position.z < 45.0f) {
				heroPositions.Add(sp);
			} else {
				normalPositions.Add(sp);
			}

			Debug.Log("Hero Positions: " + heroPositions.Count);
			Debug.Log("Normal Positions: " + normalPositions.Count);
		}
		
	}

	public SpawnPoint GetValidSpawnPoint( List<SpawnPoint> pointList ) {
		foreach (SpawnPoint sp in pointList) {
			if (false == sp.occupied) {
				return sp;
			}
		}
		Debug.LogWarning("There were no unoccupied spawn points in the list!");
		return null;
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
	
	public void InitBackgroundPanels()
	{
		bgPlanes = new ArrayList();
		for( int i=0; i < numBgPlanes; i++) {
			GameObject p = SpawnPanel(false);
			bgPlanes.Add(p); 
		}
		Debug.Log( "BG Planes: " + bgPlanes.Count );
		StartCoroutine(TryToSpawnBG());
		StartCoroutine(TryToRemoveBG());
	}
	
	public void InitForgroundPanels()
	{
		fgPlanes = new ArrayList();
		for( int i=0; i < numPlanes; i++) {
			GameObject p = SpawnPanel(true);
			fgPlanes.Add(p); 
		}
		Debug.Log( "fg Planes: " + fgPlanes.Count );
		StartCoroutine(TryToSpawnFG());
	}
	
	public GameObject SpawnPanel(bool isHero){
		SpawnPoint sp = isHero ? GetValidSpawnPoint(heroPositions) : GetValidSpawnPoint(normalPositions);
		if (sp == null) {
			Debug.LogWarning("SPAWN FAILED: No unoccupied spawnpoint was found. isHero=" + isHero);
			return null;
		}
		sp.occupied = true;

		Quaternion r = Quaternion.Euler(0.0f, 0.0f, 0.0f);
		GameObject p = Instantiate(plane_, transform.position, r) as GameObject;
		p.transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible();
		p.transform.position = sp.position;
		p.GetComponent<SetUpText>().isHero = isHero;
		p.GetComponent<SetUpText>().sp = sp;

		p.SetActive(true);
		return p;
	}

	public IEnumerator TryToSpawnBG() {
		float t = Random.Range(spawnBgDelay_Min, spawnBgDelay_Max);
		while (t > 0.0f) {
			t -= Time.fixedDeltaTime;
			yield return 0;
		}
		if (bgPlanes.Count < numBgPlanes) {
			GameObject p = SpawnPanel(false); 
			bgPlanes.Add(p);
//			addbgPanelData(p); 
			
		}
		StartCoroutine(TryToSpawnBG());
	}
	
	public IEnumerator TryToSpawnFG() {
		float t = Random.Range(spawnFgDelay_Min, spawnFgDelay_Max);
		while (t > 0.0f) {
			t -= Time.fixedDeltaTime;
			yield return 0;
		}
		Debug.Log("Try To Spawn FG");
		if (fgPlanes.Count < numPlanes) {
			Debug.Log ("OK to spawn FG");
			GameObject p = SpawnPanel(true);
			fgPlanes.Add(p);
		} else {
			Debug.LogWarning("Too many FG planes to add a new one!");
		}
		StartCoroutine(TryToSpawnFG());
	}
	
	public IEnumerator TryToRemoveBG() {

		float t = Random.Range(removeBgDelay_Min, removeBgDelay_Max);
		while (t > 0.0f) {
			t -= Time.fixedDeltaTime;
			yield return 0;
		}

		if (bgPlanes.Count > 0) {
			GameObject plane = bgPlanes[Random.Range(0, bgPlanes.Count-1)] as GameObject;
			plane.GetComponent<SetUpText>().fadeOut();
		}

		StartCoroutine(TryToRemoveBG());
	}

	public void RecyclePerson( Person p ) {
		if (p.hero == "no") {
			DataPuller.RemoveNormalPersonFromActiveList(p);
		} else {
			DataPuller.RemoveHeroFromActiveList(p);
		}
	}
	
}