﻿using UnityEngine;
using System.Collections;

public class SetUpText : MonoBehaviour {
	public GameObject blackBox;  

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
	public GameObject photoObject;
	Vector3 scaleRatio;

	public SpawnPoint sp;
	public Vector3 originPos;
	public Vector3 posLerp = new Vector3();
	public GameObject centerPoint;
	public float fadeInTimer = 2.0f;
	public EasingType fadeInEaseType = EasingType.Linear;
	public float fadeInScalar = 0.5f;
	public EasingType fadeInScaleEaseType = EasingType.Linear;
	public float fadeOutTimer = 2.0f;
	public EasingType fadeOutEaseType = EasingType.Linear;
	public float fadeOutScalar = 0.5f;
	public EasingType fadeOutScaleEaseType = EasingType.Linear;
	public float preFadeOutDelay = 5.0f;
	public float moveToCenterDuration = 2.0f;
	public EasingType moveToCenterEaseType = EasingType.Linear;
	public float colorChangeDelayDuration = 2.0f;
	public float animationDuration = 2.0f;
	public float stayOpenDuration = 2.0f;
	public float bodyTextAppearDuration = 2.0f;
	private float moveTimer = 0.0f;

	public bool isHero;
	
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

	private bool fadeBoxUp = false;
	private bool fadeBoxDown = false;

	private Vector3 originalScale;


	void Start () {
		originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		transform.localScale *= fadeInScalar;
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
		if(fadeBoxUp) 
		{
			fadeBoxIn(); 
		}
		if(fadeBoxDown) 
		{
			fadeBoxOut(); 
		}
	}
	private float alpha_time = 0.0f; 
	private float alpha_duration = 300.0f; 
	public void fadeBoxIn()
	{
		Color fadedUpColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); 
		Color col = blackBox.GetComponent<Renderer>().material.GetVector("_Color"); 
		if(col.a < 0.4f) 
		{
			col = Color.Lerp(col, fadedUpColor, alpha_time); 
			alpha_time += Time.deltaTime/alpha_duration; 
			blackBox.GetComponent<Renderer>().material.SetVector("_Color", col); 

		}
		else {
			fadeBoxUp = false;
			alpha_time = 0.0f; 
		} 
	}

	public void fadeBoxOut()
	{
		Color fadedOutColor = new Color(0.0f, 0.0f, 0.0f, 0.0f); 
		Color col = blackBox.GetComponent<Renderer>().material.GetVector("_Color"); 
		if(col.a > 0.0f) 
		{
			col = Color.Lerp(col, fadedOutColor, alpha_time); 
			alpha_time += Time.deltaTime/alpha_duration; 
			blackBox.GetComponent<Renderer>().material.SetVector("_Color", col); 
			
		}
		else {
			fadeBoxDown = false;
		} 
	}

	void GetData() {
		
//		DataPuller.num = 2;
//		data.GetComponent<DataPuller>().dataItem();
		if( isHero ) {
			p = DataPuller.PullNewHero();
		} else {
			p = DataPuller.PullNewNormalPerson();
		}
		populateData();
	}
	
	void populateData()
	{   
		bodyTextObject.GetComponent<TextMesh>().text = p.description;
		bodyTextObject.GetComponent<TextWrapper>().SetText();
		
		nameTextObject.GetComponent<TextMesh>().text = p.familyName.ToUpper() + " " + p.givenName.ToUpper() + " (" + System.Text.RegularExpressions.Regex.Replace( p.lifespan, @"[\(\)]", "") + ")";
		
		locationTextObject.GetComponent<TextMesh>().text = p.location.ToUpper();

		string photoPath = @"file://" + System.IO.Directory.GetCurrentDirectory() + "/photos/" + p.filename;

		photoObject.GetComponent<Renderer>().material.SetTexture("_image", LoadPhoto(photoPath));


//		string photoPath = "photos/" + p.filename.Split(new char[]{'.'})[0];
//		Debug.Log("photo: " + photoPath);
//		Texture2D img = Resources.Load(photoPath) as Texture2D;
//		photoObject.GetComponent<Renderer>().material.SetTexture("_image", img);
	}

	private Texture2D LoadPhoto(string path) {
		WWW www = new WWW(path);
		while (!www.isDone); // wait for file load to complete
		Texture2D img = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
		www.LoadImageIntoTexture(img);
		return img;
	}

	
	public void setOrigin()
	{
		originPos = transform.position;
		//		Debug.Log("originPos set: " + originPos);
	}
	
	public void spawn() {
		moveTimer += Time.fixedDeltaTime;
		if (moveTimer >= 1.0f) {
			moveTimer = 0.0f;
			spawnState = false;
			fadeIn(fadeInTimer);
		}
	}
	
	public void fadeIn(float t = -1.0f) {
		if ( fadeInState == false ) {
			fadeInState = true;
			
			Component[] faders;
			faders = GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				if(fader.gameObject.name != "BodyTextMesh") {
					fader.MakeVisible(t, 1.0f, fadeInEaseType);
				}
			}
			return;
		}
		moveTimer += Time.fixedDeltaTime;
		if ( moveTimer >= fadeInTimer ) {
			transform.localScale.Set(originalScale.x, originalScale.y, originalScale.z);
			moveTimer = 0.0f;
			fadeInState = false;
			return;
		}
		transform.localScale = Vector3.Lerp( fadeInScalar * originalScale, originalScale, Easing.EaseInOut(moveTimer/fadeInTimer, fadeInScaleEaseType));
	}
	
	public void moveToCenter() {
		if(moveToCenterState == false) {
			moveToCenterState = true;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = true;
			return;
		}
		moveTimer += Time.fixedDeltaTime;
		if ( !animateOpenState && moveTimer >= moveToCenterDuration - 0.5f ) {
			transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeOrange();
			transform.Find("openNode").GetComponent<AnimControl>().OpenNode();
		}
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = centerPoint.transform.position;
			moveToCenterState = false;
			transform.Find("GoldPlaneTiltedUp").collider.isTrigger = false;
			doOpenAnimation();
			return;
		}
		transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, Easing.EaseInOut(moveTimer/moveToCenterDuration, moveToCenterEaseType));
//		gameObject.transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, moveTimer/moveToCenterDuration);
	}
	
	
	
	public void doOpenAnimation() {
		if(animateOpenState == false ) {
			fadeBoxUp = true; 
			animateOpenState = true;
//			transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeOrange();
//			transform.Find("openNode").GetComponent<AnimControl>().OpenNode();
			return;
		}
		moveTimer += Time.fixedDeltaTime;
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
		moveTimer += Time.fixedDeltaTime;
		if( moveTimer >= bodyTextAppearDuration ) {
			moveTimer = 0.0f;
			transform.Find ("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible();
			bodyTextAppearState = false;
			doCloseAnimation();
			return;
		}
	}
	
	public void doCloseAnimation() {
		fadeBoxDown = true; 
		if(animateCloseState == false ) {
			animateCloseState = true;
			transform.Find("openNode").GetComponent<AnimControl>().CloseNode();
			return;
		}
		moveTimer += Time.fixedDeltaTime;
		if( moveTimer >= 1.0f ) {
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
		moveTimer += Time.fixedDeltaTime;
		if (moveTimer >= colorChangeDelayDuration) {
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
		moveTimer += Time.fixedDeltaTime;
		if ( moveTimer >= moveToCenterDuration ) {
			moveTimer = 0.0f;
			gameObject.transform.position = originPos;
			returnToOriginState = false;
			Transform t = transform.Find("GoldPlaneTiltedUp").transform;
			t.GetComponent<TriggerFade>().Reset();
			t.collider.isTrigger = false;
			fadeOutDelay();
			return;
		}
		gameObject.transform.position = Vector3.Lerp(centerPoint.transform.position, originPos, Easing.EaseInOut(moveTimer/moveToCenterDuration, moveToCenterEaseType));
	}
	
	public void fadeOutDelay() {
		if (fadeOutDelayState == false) {
			fadeOutDelayState = true;
			;
			return;
		}
		moveTimer += Time.fixedDeltaTime;
		if (moveTimer >= preFadeOutDelay) {
			moveTimer = 0.0f;
			fadeOutDelayState = false;
			fadeOut (fadeOutTimer);
			return;
		}
	}
	
	public void fadeOut(float t = -1.0f) {
		if ( fadeOutState == false ) {
			fadeOutState = true;
			
			Component[] faders;
			faders = GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				fader.MakeInvisible(t, 0.0f, fadeOutEaseType);
			}
			return;
		}
		moveTimer += Time.fixedDeltaTime;
		if ( moveTimer >= fadeOutTimer ) {
			moveTimer = 0.0f;
			m.fgPlanes.Remove(gameObject);
			m.bgPlanes.Remove(gameObject);
			m.RecyclePerson(p);
			sp.occupied = false;
			Destroy(gameObject, 1.0f);
			fadeOutState = false;
			return;
		}
		transform.localScale = Vector3.Lerp (originalScale, fadeOutScalar * originalScale, Easing.EaseIn( moveTimer/fadeOutTimer, fadeOutScaleEaseType) );
	}
	
}