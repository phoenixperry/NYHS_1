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
	public GameObject photoObject;
	public TextureFormat photoImportFormat = TextureFormat.ARGB32;
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
	public float openJumpStart = 0.5f;
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

//	private float alpha_time = 0.0f; 
//	private float alpha_duration = 300.0f;

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

		transform.FindChild("GoldPlaneTiltedUp").renderer.material.renderQueue = 3000 - (int)transform.position.z;
		transform.FindChild("BodyTextMesh").renderer.material.renderQueue = 3001 - (int)transform.position.z;
		transform.FindChild("LocationText").renderer.material.renderQueue = 3001 - (int)transform.position.z;
		transform.FindChild("NameText").renderer.material.renderQueue = 3001 - (int)transform.position.z;
		transform.FindChild("Photo").renderer.material.renderQueue = 3001 - (int)transform.position.z;

		if (spawnState) {
			spawn();
		}
	}

	
	// Get Data for this panel from the database
	void GetData() {
		if( isHero ) {
			p = DataPuller.PullNewHero();
		} else {
			p = DataPuller.PullNewNormalPerson();
		}
//		Debug.Log(p.givenName);
		populateData();
	}

	// Do panel layout
	void populateData()
	{   
		bodyTextObject.GetComponent<TextMesh>().text = p.description;
		bodyTextObject.GetComponent<TextWrapper>().SetText();

		DynamicText dt = nameTextObject.GetComponent<DynamicText>();
		dt.textSB.Remove(0, dt.textSB.Length);
		dt.textSB.Append( p.givenName.ToUpper() + " " + p.familyName.ToUpper() + " " + p.lifespan);
		dt.FinishedTextSB();

		dt = locationTextObject.GetComponent<DynamicText>();
		dt.textSB.Remove(0, dt.textSB.Length);
		dt.textSB.Append( p.location.ToUpper() );
		dt.FinishedTextSB();

		photoObject.GetComponent<Renderer>().material.SetTexture("_image", p.photo);
	}
	
	public void setOrigin()
	{
		originPos = transform.position;
	}

	// Prepare panel for fade-in.
	public void spawn() {
		moveTimer += Time.fixedDeltaTime;
		if (moveTimer >= 1.0f) {
			moveTimer = 0.0f;
			spawnState = false;
			locationTextObject.GetComponent<DynamicText>().enabled = true;
			nameTextObject.GetComponent<DynamicText>().enabled = true;
			StartCoroutine(fadeIn (fadeInTimer));
		}
	}

	// Fade-in co-routine
	public IEnumerator fadeIn( float duration = -1.0f ) {
		fadeInState = true;
		Component[] faders;
		faders = GetComponentsInChildren<SmoothAlpha>();
		for (int i=0; i<faders.Length; i++){
			if(faders[i].gameObject.name != "BodyTextMesh") {
				(faders[i] as SmoothAlpha).MakeVisible(duration, 1.0f, fadeInEaseType);
			}
		}
		float t = 0.0f;
		while (t < duration) {
			t += Time.fixedDeltaTime;
			transform.localScale = Vector3.Lerp( fadeInScalar * originalScale, originalScale, Easing.EaseInOut(t/duration, fadeInScaleEaseType));
			yield return 0;
		}
		DynamicText dt = nameTextObject.GetComponent<DynamicText>();
		dt.color = new Color(dt.color.r, dt.color.g, dt.color.b, 1.0f);

		transform.localScale.Set(originalScale.x, originalScale.y, originalScale.z);
		fadeInState = false;
	}

	// Tint panel and its component parts to a given color in a given time
	public void Tint( Color color, float duration ) {
		transform.Find ("GoldPlaneTiltedUp").GetComponent<TintController>().StartTint(color, duration);
		transform.Find ("NameText").GetComponent<TintController>().StartTint(color, duration);
		transform.Find ("LocationText").GetComponent<TintController>().StartTint(color, duration);
		transform.Find ("Photo").GetComponent<TintController>().StartTint(color, duration);
	}

	// Set panel tint back to normal in a given time
	public void UnTint( float duration ) {
		transform.Find ("GoldPlaneTiltedUp").GetComponent<TintController>().UnTint(duration);
		transform.Find ("NameText").GetComponent<TintController>().UnTint(duration);
		transform.Find ("LocationText").GetComponent<TintController>().UnTint(duration);
		transform.Find ("Photo").GetComponent<TintController>().UnTint(duration);
	}

	// Co-routine for moving a panel into the spotlight point
	public IEnumerator moveToCenter() {
		moveToCenterState = true;
		transform.Find("GoldPlaneTiltedUp").collider.isTrigger = true;
		float t = 0.0f;
		while (t < moveToCenterDuration) {
			t += Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(originPos, centerPoint.transform.position, Easing.EaseInOut(t/moveToCenterDuration, moveToCenterEaseType));
			if ( !animateOpenState && t >= moveToCenterDuration - openJumpStart ) {
				transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeOrange();
				StartCoroutine(doOpenAnimation());
			}
			yield return 0;
		}
		transform.position = centerPoint.transform.position;
		transform.Find("GoldPlaneTiltedUp").collider.isTrigger = false;
		moveToCenterState = false;
	}

	// Co-routine for opening the panel
	public IEnumerator doOpenAnimation() {
		animateOpenState = true;
		float t = 0.0f;
		m.TintNonFocusedNodes();
		while (t < animationDuration) {
			t += Time.fixedDeltaTime;
			transform.FindChild("GoldPlaneTiltedUp").renderer.material.SetFloat("_open", Mathf.Lerp(0, 1, t/animationDuration ));
			yield return 0;
		}
		animateOpenState = false;;
		StartCoroutine(doBodyTextAppear());
	}

	// Co-routine for fading in the body text
	public IEnumerator doBodyTextAppear() {
		bodyTextAppearState = true;
//		float t = 0.0f;
		transform.Find("BodyTextMesh").GetComponent<SmoothAlpha>().MakeVisible();
		yield return new WaitForSeconds(bodyTextAppearDuration);
		transform.Find ("BodyTextMesh").GetComponent<SmoothAlpha>().MakeInvisible();
		bodyTextAppearState = false;
		StartCoroutine( doCloseAnimation() );
		StartCoroutine( colorChangeDelay() );
		StartCoroutine( returnToOrigin() );
	}

	// Co-routine for closing the panel
	public IEnumerator doCloseAnimation() {
		animateCloseState = true;
		m.UnTintNonFocusedNodes();
		float t = 0.0f;
		while (t < animationDuration) {
			t += Time.deltaTime;
			transform.FindChild("GoldPlaneTiltedUp").renderer.material.SetFloat("_open", Mathf.Lerp(1, 0, t/animationDuration ));
			yield return 0;
		}
		animateCloseState = false;
	}

	// Co-routine for triggering the color change after closing
	public IEnumerator colorChangeDelay() {
		colorChangeDelayState = true;
		transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeYellow();
		yield return new WaitForSeconds(colorChangeDelayDuration);
		colorChangeDelayState = false;
	}

	// Co-routine for moving a panel back to its starting point from the spotlight point
	public IEnumerator returnToOrigin() {
		returnToOriginState = true;
		transform.Find("GoldPlaneTiltedUp").collider.isTrigger = true;
		float t = 0.0f;
		while (t < moveToCenterDuration) {
			t += Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(centerPoint.transform.position, originPos, Easing.EaseInOut(t/moveToCenterDuration, moveToCenterEaseType));
//			if ( !animateOpenState && t >= moveToCenterDuration - 0.5f ) {
//				transform.Find ("GoldPlaneTiltedUp").GetComponent<PlaneSetup>().fadeOrange();
//			}
			yield return 0;
		}
		transform.position = originPos;
		transform.Find("GoldPlaneTiltedUp").collider.isTrigger = false;
		returnToOriginState = false;
		Transform tf = transform.Find("GoldPlaneTiltedUp").transform;
		tf.GetComponent<TriggerFade>().Reset();
		tf.collider.isTrigger = false;
		StartCoroutine(fadeOutDelay());
	}

	// Co-routine for waiting to trigger the fade-out
	public IEnumerator fadeOutDelay() {
		fadeOutDelayState = true;
		yield return new WaitForSeconds(preFadeOutDelay);
		fadeOutDelayState = false;
		StartCoroutine(fadeOut (fadeOutTimer));
	}

	// Co-routine for panel fade-out and triggering object destruction
	public IEnumerator fadeOut( float duration = -1.0f) {
		fadeOutState = true;
		m.fadingPlanes.Add(gameObject);
		m.fgPlanes.Remove(gameObject);
		m.bgPlanes.Remove(gameObject);
		m.RecyclePerson(p);
		Component[] faders;
		faders = GetComponentsInChildren<SmoothAlpha>();
		for (int i=0; i<faders.Length; i++) {
			(faders[i] as SmoothAlpha).MakeInvisible(duration, 0.0f, fadeOutEaseType);
		}
		float t = 0.0f;
		while ( t < duration ) {
			t += Time.fixedDeltaTime;
			transform.localScale = Vector3.Lerp (originalScale, fadeOutScalar * originalScale, Easing.EaseIn( t/duration, fadeOutScaleEaseType) );
			yield return 0;
		}
		m.fadingPlanes.Remove(gameObject);
		sp.occupied = false;
		Destroy(gameObject, 1.0f);
		fadeOutState = false;
	}

}