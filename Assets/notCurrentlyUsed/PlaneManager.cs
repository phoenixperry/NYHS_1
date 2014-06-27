using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;

public class SpawnPoint {
	public Vector3  position;
	public bool		occupied;
}


public class PlaneManager : MonoBehaviour {
	public GameObject plane_;
	public ArrayList planes;
	public ArrayList bgPlanes;
	public ArrayList fgPlanes;
	public ArrayList fadingPlanes;
	public float foregroundZCutoff = 45.0f;
	
	public Color dimColor = Color.grey;
	public float timeBetweenHeroes = 2.0f;
	public float timeForFirstHero = 1.0f;
	public float spawnBgDelay_Min = 4.0f;
	public float spawnBgDelay_Max = 10.0f;
	public float removeBgDelay_Min = 4.0f;
	public float removeBgDelay_Max = 10.0f;
	public float spawnFgDelay_Min = 4.0f;
	public float spawnFgDelay_Max = 10.0f;
	public static int numBgPlanes = 39;
	public int minimumBgPlanes = 30;
	public static int numPlanes = 5; //refractor to be numFgPlanes
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
	private bool dimmedBackgroundNodes = false;
	List<Vector3> positions; 
	List<Vector3> vect3positions; 
	
	List<SpawnPoint> heroPositions;
	List<SpawnPoint> normalPositions;
	
	List<Texture2D> images; 
	
	
	// Use this for initialization
	void Start()
	{
		fadingPlanes = new ArrayList();
		Screen.showCursor = false;
		heroTimer = timeBetweenHeroes - timeForFirstHero;
		loadNodePositions();
		StartCoroutine(InitBackgroundPanels());
		StartCoroutine(InitForgroundPanels()); 
	}
	
//	public static double NextGaussianDouble(double mu = 0.0, double sigma = 1.0)
//	{
//		double U, u, v, S;
//		
//		do
//		{
//			u = 2.0 * Random.value - 1.0;
//			v = 2.0 * Random.value - 1.0;
//			S = u * u + v * v;
//		}
//		while (S >= 1.0);
//		float s_ = (float)S;
//		float Ss = (float)(-2.0 * Mathf.Log(s_) / S);
//		float fac = Mathf.Sqrt(Ss);
//		return u * fac * sigma + mu;
//	}
	
//	public void FixedUpdate()
//	{
//		
//	}
	
	public void Update()
	{
		heroTimer += Time.fixedDeltaTime;
		if(heroTimer >= timeBetweenHeroes) {
			heroTimer = 0.0f;
			if (fgPlanes.Count > 0) {
				heroInFocus = fgPlanes[0] as GameObject;
				FocusOnHero(heroInFocus);
				fgPlanes.RemoveAt(0);
				fgPlanes.Insert(fgPlanes.Count, heroInFocus);
			}
		}
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			loadNodePositions(); 
//			testAddfgPlaneData();
//		}
		//will allow for new positions to be written while Unity is running 
		if(Input.GetKeyDown(KeyCode.R))
			SaveNodes();
		
	}

	// load spawn point locations from external file
	public void loadNodePositions() 
	{	  
		heroPositions = new List<SpawnPoint>();
		normalPositions = new List<SpawnPoint>();
		
		foreach(string pos in File.ReadAllLines("./Assets/data/dataPositions.txt"))
		{
			SpawnPoint sp = new SpawnPoint();
			
			sp.position = stripData(pos);
			sp.occupied = false;
			
			if(sp.position.z < foregroundZCutoff) {
				heroPositions.Add(sp);
				normalPositions.Add (sp);
			} else {
				normalPositions.Add(sp);
			}
		}
		Debug.Log("Hero Positions: " + heroPositions.Count);
		Debug.Log("Normal Positions: " + normalPositions.Count);
	}

	// Parses the external list of spawn point locations and turns them into usable data
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

	// Writes the current panel positions out to the external spawn point file
	public void SaveNodes()
	{
		ArrayList positions = new ArrayList();
		for (int i=0; i<bgPlanes.Count; i++) {
			Vector3 pos = (bgPlanes[i] as GameObject).transform.position;
			positions.Add(pos);
		}
//		foreach (GameObject bg in bgPlanes)
//		{
//			Vector3 pos = bg.transform.position;
//			positions.Add(pos); 
//		}
		for (int i=0; i<fgPlanes.Count; i++){
			Vector3 pos = (fgPlanes[i] as GameObject).transform.position;
			positions.Add(pos);
		}
//		foreach (GameObject fg in fgPlanes)
//		{
//			Vector3 pos = fg.transform.position;
//			positions.Add(pos);
//		}
		Debug.Log("you have " + positions.Count + "number of positions");
		for (int i=0; i<positions.Count; i++) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter("./Assets/data/dataPositions.txt", true))
			{
				file.WriteLine(positions[i]);
			}
		}
//		foreach(Vector3 pos in positions){
//			using (System.IO.StreamWriter file = new System.IO.StreamWriter("./Assets/data/dataPositions.txt", true))
//			{
//				file.WriteLine(pos);
//			}
//		}
	}

	// Finds the first available spawn point, moves it to the end of the list, and returns it
	public SpawnPoint GetValidSpawnPoint( List<SpawnPoint> pointList ) {
//		for (int i=0; i<pointList.Count; i++) {
//			if (false == pointList[i].occupied) {
//				pointList.Remove(pointList[i]);
//				pointList.Insert(pointList.Count, pointList[i]);
//				return pointList[i];
//			}
//		}
		foreach (SpawnPoint sp in pointList) {
			if (false == sp.occupied) {
				pointList.Remove(sp);
				pointList.Insert(pointList.Count, sp);
				return sp;
			}
		}
		Debug.LogWarning("There were no unoccupied spawn points in the list!");
		return null;
	}

	// Starts the process of bringing a panel into the spotlight point.
	public void FocusOnHero(GameObject hero)
	{
		StartCoroutine(hero.GetComponent<SetUpText>().moveToCenter());
	}

	// Tells every node except the hero moving to the spotlight point to tint itself
	public void TintNonFocusedNodes() {
		dimmedBackgroundNodes = true;
		int i;
		for (i = 0; i < bgPlanes.Count; i++) {
			(bgPlanes[i] as GameObject).GetComponent<SetUpText>().Tint(dimColor, 1.0f);
		}
		for (i = 0; i < fgPlanes.Count; i++) {
			if(fgPlanes[i] != heroInFocus) {
				(fgPlanes[i] as GameObject).GetComponent<SetUpText>().Tint(dimColor, 1.0f);
			}
		}
		for (i = 0; i < fadingPlanes.Count; i++) {
			if(fadingPlanes[i] != heroInFocus) {
				(fadingPlanes[i] as GameObject).GetComponent<SetUpText>().Tint(dimColor, 1.0f);
			}
		}
	}

	// Tells every node except the hero leaving the spotlight point to untint itself
	public void UnTintNonFocusedNodes() {
		int i;
		for (i = 0; i < bgPlanes.Count; i++) {
			(bgPlanes[i] as GameObject).GetComponent<SetUpText>().UnTint(1.0f);
		}
		for (i = 0; i < fgPlanes.Count; i++) {
			if(fgPlanes[i] != heroInFocus) {
				(fgPlanes[i] as GameObject).GetComponent<SetUpText>().UnTint(1.0f);
			}
		}
		dimmedBackgroundNodes = false;
	}

	// Do initial populating of normal people (spawn faster than normal)
	public IEnumerator InitBackgroundPanels()
	{
		bgPlanes = new ArrayList();
		for( int i=0; i < numBgPlanes; i++) {
			GameObject p = SpawnPanel(false);
			if (p != null) {
				bgPlanes.Add(p); 
			}
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
		}
		Debug.Log( "BG Planes: " + bgPlanes.Count );
		StartCoroutine(TryToSpawnBG());
		StartCoroutine(TryToRemoveBG());
	}

	// Do initial populating of hero people (spawn faster than normal)
	public IEnumerator InitForgroundPanels()
	{
		fgPlanes = new ArrayList();
		for( int i=0; i < numPlanes; i++) {
			GameObject p = SpawnPanel(true);
			if (p != null) {
				fgPlanes.Add(p); 
			}
			yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
		}
		Debug.Log( "fg Planes: " + fgPlanes.Count );
		StartCoroutine(TryToSpawnFG());
	}

	// Spawn a new panel if there is an available spawn point
	public GameObject SpawnPanel(bool isHero){
		SpawnPoint sp = isHero ? GetValidSpawnPoint(heroPositions) : GetValidSpawnPoint(normalPositions);
		if (sp == null) {
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

	// Co-routine that tries to spawn a new normal person every few seconds if the current population
	// of normal people is below the population cap
	public IEnumerator TryToSpawnBG() {
//		if (bgPlanes.Count < minimumBgPlanes) {
//			Debug.LogWarning("Slow removal rate.");
//			yield return new WaitForSeconds(Random.Range(spawnBgDelay_Min*2.0f, spawnBgDelay_Max*2.0f));
//		}
//		else {
			yield return new WaitForSeconds(Random.Range(spawnBgDelay_Min, spawnBgDelay_Max));
//		}
		if (bgPlanes.Count < numBgPlanes && !dimmedBackgroundNodes) {
			GameObject p = SpawnPanel(false);
			if (p != null) {
				bgPlanes.Add(p);
			}
		} else {
			Debug.LogWarning("Too many BG planes to add a new one!");
		}
		StartCoroutine(TryToSpawnBG());
	}

	// Co-routine that tries to spawn a new hero every few seconds if the current hero population
	// is below the population cap
	public IEnumerator TryToSpawnFG() {
		yield return new WaitForSeconds(Random.Range(spawnFgDelay_Min, spawnFgDelay_Max));
		if (fgPlanes.Count < numPlanes && !dimmedBackgroundNodes) {
			GameObject p = SpawnPanel(true);
			if (p != null) {
				fgPlanes.Add(p);
			}
		} else {
			Debug.LogWarning("Too many FG planes to add a new one!");
		}
		StartCoroutine(TryToSpawnFG());
	}

	// Co-routine that tries to remove a normal person every few seconds. Will wait longer if
	// the population of normal people is below a threshold value
	public IEnumerator TryToRemoveBG() {
		if (bgPlanes.Count < minimumBgPlanes) {
			Debug.LogWarning("Slow removal rate.");
			yield return new WaitForSeconds(Random.Range(spawnBgDelay_Min*2.0f, spawnBgDelay_Max*2.0f));
		}
		else {
			yield return new WaitForSeconds(Random.Range(removeBgDelay_Min, removeBgDelay_Max));
		}
		
		if (bgPlanes.Count > 0 && !dimmedBackgroundNodes) {
			GameObject plane = bgPlanes[0] as GameObject;
			StartCoroutine(plane.GetComponent<SetUpText>().fadeOut(plane.GetComponent<SetUpText>().fadeOutTimer));
		}
		
		StartCoroutine(TryToRemoveBG());
	}

	// Informs the system that a given person is no longer on screen
	public void RecyclePerson( Person p ) {
		if (!p.hero) {
			DataPuller.RemoveNormalPersonFromActiveList(p);
		} else {
			DataPuller.RemoveHeroFromActiveList(p);
		}
	}
	
}