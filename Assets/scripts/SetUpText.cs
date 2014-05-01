﻿using UnityEngine;
using System.Collections;

public class SetUpText : MonoBehaviour {

    //code for adding database text to screen 
    //you put the data on the camera encase you forgot 
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
	public float moveToCenterDuration = 2.0f;
	public float animationDuration = 2.0f;
	public float stayOpenDuration = 2.0f;
	public float bodyTextAppearDuration = 2.0f;
	private float moveTimer = 0.0f;

	private bool fadeInState = false;
	private bool moveToCenterState = false;
	private bool returnToOriginState = false;
	private bool animateOpenState = false;
	private bool bodyTextAppearState = false;
	private bool animateCloseState = false;
	private bool fadeOutState = false;


	void Start () {
        data = GameObject.Find("Data");
        GetData();
        scaleRatio = closedNode.transform.lossyScale;
		setOrigin();
        //scaleRatio = scaleRatio / 2;
//        Debug.Log(scaleRatio);
//		fadeIn();
	}

	void Update () {
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
		if (fadeOutState) {
			fadeOut();
		}
	}
    void GetData() {
       
        DataPuller.num = 2;
        data.GetComponent<DataPuller>().dataItem();
        p = DataPuller.currentHero;
    
		populateData();
    }

    void populateData()
    {   
        bodyTextObject.GetComponent<TextMesh>().text = p.description;
		bodyTextObject.GetComponent<TextWrapper>().SetText();

        nameTextObject.GetComponent<TextMesh>().text = p.familyName + " " + p.givenName + " " + p.lifespan;

        locationTextObject.GetComponent<TextMesh>().text = p.location; 

		Debug.Log("Name: " + nameTextObject.GetComponent<TextMesh>().text);
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

	public void fadeIn(float t = -1.0f) {
		if ( fadeInState == false ) {
			fadeInState = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= fadeTimer ) {
			moveTimer = 0.0f;

			fadeInState = false;
			return;
		}
		Component[] faders;
		faders = GetComponentsInChildren<SmoothAlpha>();
		foreach (SmoothAlpha fader in faders) {
			fader.MakeVisible(t);
		}
	}

	public void moveToCenter() {
		if(moveToCenterState == false) {
			moveToCenterState = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = centerPoint.transform.position;
			moveToCenterState = false;
			doOpenAnimation();
			return;
		}
		gameObject.transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, moveTimer/moveToCenterDuration);
	}

	public void returnToOrigin() {
		Debug.Log("returnToOrigin");
		if(returnToOriginState == false) {
			returnToOriginState = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = originPos;
			returnToOriginState = false;
			fadeOut();
			return;
		}
		gameObject.transform.position = Vector3.Lerp(centerPoint.transform.position, originPos, moveTimer/moveToCenterDuration);
	}

	public void doOpenAnimation() {
		if(animateOpenState == false ) {
			animateOpenState = true;
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
		if( moveTimer >= 4.0f ) {
			moveTimer = 0.0f;
			animateCloseState = false;
			returnToOrigin();
			return;
		}
	}

	public void fadeOut(float t = -1.0f) {
		if ( fadeOutState == false ) {
			fadeOutState = true;
			return;
		}
		moveTimer += Time.deltaTime;
		if ( moveTimer >= fadeTimer ) {
			moveTimer = 0.0f;
			
			fadeOutState = false;
			return;
		}
		Component[] faders;
		faders = GetComponentsInChildren<SmoothAlpha>();
		foreach (SmoothAlpha fader in faders) {
			fader.MakeInvisible(t);
		}
	}

}
