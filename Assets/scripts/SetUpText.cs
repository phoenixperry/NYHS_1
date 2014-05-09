using UnityEngine;
using System.Collections;

public class SetUpText : MonoBehaviour {
	
	//code for adding database text to screen 
	//you put the data on the camera encase you forgot 
	public PlaneManager m;
	public GameObject data;
	Person p;
	public GUIStyle descriptionStyle;
	public GUIStyle nameStyle;
	public GUIStyle locationStyle;
	public Camera cam;
	public GameObject closedNode;
	public GameObject openNode;
	public GameObject nameTextObject;
	public GameObject locationTextObject;
	public GameObject bodyTextObject;
	Vector3 scaleRatio;
	
	public Vector3 originPos;
	public Vector3 posLerp = new Vector3();
	public GameObject centerPoint;
	public float fadeTimer = 0.5f;
	public float preFadeOutDelay = 5.0f;
	public float moveToCenterDuration = 2.0f;
	public float animationDuration = 2.0f;
	public float stayOpenDuration = 2.0f;
	public float bodyTextAppearDuration = 2.0f;
	private float moveTimer = 0.0f;
	
	private bool spawnState = false;
	private bool fadeInState = false;
	private bool moveToCenterState = false;
	private bool returnToOriginState = false;
	private bool animateOpenState = false;
	private bool bodyTextAppearState = false;
	private bool colorChangeDelayState = false;
	private bool animateCloseState = false;
	private bool fadeOutDelayState = false;
	private bool fadeOutState = false;
	
	
	void Start () {
		data = GameObject.Find("Data");
		GetData();
		scaleRatio = closedNode.transform.lossyScale;
		setOrigin();
		spawnState = true;
		//scaleRatio = scaleRatio / 2;
		//        Debug.Log(scaleRatio);
		//		fadeIn();
	}
	
	void Update () {
		if (spawnState) {
			spawn();
		}
		if (fadeInState) {
			fadeIn();
		}
		if (moveToCenterState) {
			moveToCenter();
		}
		if (returnToOriginState) {
			returnToOrigin();
		}
		if (animateOpenState) {
			doOpenAnimation();
		}
		if (bodyTextAppearState) {
			doBodyTextAppear();
		}
		if (animateCloseState) {
			doCloseAnimation();
		}
		if (colorChangeDelayState) {
			colorChangeDelay();
		}
		if (fadeOutDelayState) {
			fadeOutDelay();
		}
		if (fadeOutState) {
			fadeOut();
		}
	}
	void GetData() {
		
		DataPuller.num = 2;
		//data.GetComponent<DataPuller>().dataItem();
		//p = DataPuller.currentHero;
		
		//populateData();
	}
	
	void populateData()
	{   
		//bodyTextObject.GetComponent<TextMesh>().text = p.description;
		//bodyTextObject.GetComponent<TextWrapper>().SetText();
		
		//nameTextObject.GetComponent<TextMesh>().text = p.familyName.ToUpper() + " " + p.givenName.ToUpper() + " (" + p.lifespan + ")";
		
		//locationTextObject.GetComponent<TextMesh>().text = p.location.ToUpper(); 
		
		//		Debug.Log("Name: " + nameTextObject.GetComponent<TextMesh>().text);
	}
	
	//Takes game object current point and flips it for GUI space generated from OnGui. 
	//    Vector3 ScreenToGUI(GameObject go)
	//    {   
	//        
	//        //save the z val so it doesn't get screwed up 
	//        float zpos = go.transform.position.z;
	//        //convert to screem space  
	//        Vector3 bounds = go.renderer.bounds.min; 
	//
	//        Vector3 vals = cam.WorldToScreenPoint(go.renderer.bounds.max);
	//
	//        //flip the y axis to account for the different spaces 
	//        vals.y = Screen.height - vals.y;
	//
	//        vals.z = zpos;
	//        return vals;
	//    }
	
	public void setOrigin()
	{
		originPos = transform.position;
		//		Debug.Log("originPos set: " + originPos);
	}
	
	public void spawn() {
		moveTimer += Time.deltaTime;
		if (moveTimer >= 1.0f) {
			moveTimer = 0.0f;
			spawnState = false;
			fadeIn(fadeTimer);
		}
	}
	
	public void fadeIn(float t = -1.0f) {
		if ( fadeInState == false ) {
			fadeInState = true;
			
			Component[] faders;
			faders = GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				if(fader.gameObject.name != "BodyTextMesh") {
					fader.MakeVisible(t);
				}
			}
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= fadeTimer ) {
			moveTimer = 0.0f;
			fadeInState = false;
			return;
		}
		
	}
	
	public void moveToCenter() {
		if(moveToCenterState == false) {
			moveToCenterState = true;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = centerPoint.transform.position;
			moveToCenterState = false;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = false;
			doOpenAnimation();
			return;
		}
		transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, Mathf.SmoothStep(0.0f, 1.0f, moveTimer/moveToCenterDuration));
//		gameObject.transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, moveTimer/moveToCenterDuration);
	}
	
	
	
	public void doOpenAnimation() {
		if(animateOpenState == false ) {
			animateOpenState = true;
			transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeOrange();
			transform.Find("openNode").GetComponent<AnimControl>().OpenNode();
			return;
		}
		moveTimer += Time.deltaTime;
		if( moveTimer >= animationDuration ) {
			moveTimer = 0.0f;
			animateOpenState = false;;
			doBodyTextAppear();
			return;
		}
	}
	
	public void doBodyTextAppear() {
		if (bodyTextAppearState == false) {
			bodyTextAppearState = true;
			transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeVisible();
			return;
		}
		moveTimer += Time.deltaTime;
		if( moveTimer >= bodyTextAppearDuration ) {
			moveTimer = 0.0f;
			transform.Find ("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible();
			bodyTextAppearState = false;
			doCloseAnimation();
			return;
		}
	}
	
	public void doCloseAnimation() {
		if(animateCloseState == false ) {
			animateCloseState = true;
			transform.Find("openNode").GetComponent<AnimControl>().CloseNode();
			return;
		}
		moveTimer += Time.deltaTime;
		if( moveTimer >= 2.0f ) {
			moveTimer = 0.0f;
			animateCloseState = false;
			colorChangeDelay();
			return;
		}
	}

	public void colorChangeDelay() {
		if (colorChangeDelayState == false) {
			colorChangeDelayState = true;
			transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeYellow();
			return;
		}
		moveTimer += Time.deltaTime;
		if (moveTimer >= 2.0f) {
			moveTimer = 0.0f;
			colorChangeDelayState = false;
			returnToOrigin();
			return;
		}
	}

	public void returnToOrigin() {
//		Debug.Log("returnToOrigin");
		if(returnToOriginState == false) {
			returnToOriginState = true;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = originPos;
			returnToOriginState = false;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = false;
			fadeOutDelay();
			return;
		}
		gameObject.transform.position = Vector3.Lerp(centerPoint.transform.position, originPos, moveTimer/moveToCenterDuration);
	}
	
	public void fadeOutDelay() {
		if (fadeOutDelayState == false) {
			fadeOutDelayState = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if (moveTimer >= preFadeOutDelay) {
			moveTimer = 0.0f;
			fadeOutDelayState = false;
			fadeOut (fadeTimer);
			return;
		}
	}
	
	public void fadeOut(float t = -1.0f) {
		if ( fadeOutState == false ) {
			fadeOutState = true;
			
			Component[] faders;
			faders = GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				fader.MakeInvisible(t);
			}
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= fadeTimer ) {
			moveTimer = 0.0f;
			m.fgPlanes.Remove(gameObject);
			m.bgPlanes.Remove(gameObject);

			Destroy(gameObject, 1.0f);
			fadeOutState = false;
			return;
		}
		
	}
	
}