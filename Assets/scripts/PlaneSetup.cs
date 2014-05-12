using UnityEngine;
using System.Collections;

public class PlaneSetup : MonoBehaviour {

	//this is the current data holder for the gold plane's positional data
    public float radius = 6;
    public float radiusX = 8;
    public float startAngle, range;
    public Vector3 pos = new Vector3();

    public GameObject data;
    public GameObject photo;

    public float colorFadeDuration = 2.000f;
	public EasingType colorFadeEaseType = EasingType.Linear;
    private float timeOfColorFade = 0.0f;

    public float alphaFadeDuration = 2.0f;


    public GameObject OpenNode;
    private Person p;
	
    //code for adding database text to screen 
    public string person_name;
    public string location;
    public string description;
    public GUISkin mySkin; 

    public Camera cam;
    //GUI space to world space conversion variable 
    Vector3 scaleRatio; 

    /*
     * Ben - the way we should do the hero and nonhero nodes should be the same except each "layer" should have a z axis min and max. All of the functions should work for each.
     */
    public Vector3 heroNodeLayerWidth;
    public Vector3 nodeLayerWidth;

    //also there should be a way to track origin
    

    //different possible states
    private bool fadeInState = false;
    
    private bool animateState = false;
	private bool fadeOutState = false;

	private bool fadeOrangeState = false;
	private bool fadeYellowState = false;
    	
	public float chip_fade;


    public void Start()
    {
//		GetComponent<SmoothAlpha>().MakeInvisible(0.0f);
      
        scaleRatio = gameObject.transform.lossyScale;
 
        cam = Camera.main; 
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


 
   

    public void Update()
    {
//		this.gameObject.renderer.material.SetFloat("_alpha_blend", chip_fade);
        //this should be one big arse state machine.
        if (fadeInState) {
            fadeIn();
        }
        
        if (animateState)
        {
            AnimateHero();

        }
        if (fadeOutState)
        {
            fadeOut();
        }

		if (fadeOrangeState){
			fadeOrange();
		}

		if (fadeYellowState) {
			fadeYellow();
		}
    }
	
    //each note should set up the data
    public void setData()
    {
        //Phoenix -- this still needs to happen for real
        description = p.description;
        name = p.familyName + " " + p.givenName + " " + p.lifespan;
        location = p.location; 

        //Phoenix

    }

    public void fadeYellow()
    {
//		Debug.Log("fade to yellow");
		if(fadeYellowState == false) {
			fadeYellowState = true;
			return;
		}
		timeOfColorFade += Time.fixedDeltaTime;
		renderer.material.SetFloat("_Blend", Mathf.Lerp(1.0f, 0.0f, Easing.EaseIn(timeOfColorFade/colorFadeDuration, colorFadeEaseType ) ) );
		if (timeOfColorFade >= colorFadeDuration) {
			timeOfColorFade = 0.0f;
			fadeYellowState = false;
		}
//        float a = Mathf.Lerp(1.0f, 0.0f, timeOfColorFade);
//        Debug.Log(a);
//        renderer.material.SetFloat("_Blend", timeOfColorFade);
//
//        if (timeOfColorFade > 0)
//        {
//            timeOfColorFade -= Time.fixedDeltaTime / colorFadeDuration;
//        }

    }
    public void fadeOrange() {
        //Phoenix
//		Debug.Log("fade to orange");
		if(fadeOrangeState == false) {
			fadeOrangeState = true;
			return;
		}
		timeOfColorFade += Time.fixedDeltaTime;
		renderer.material.SetFloat("_Blend", Mathf.Lerp(0.0f, 1.0f, Easing.EaseIn(timeOfColorFade/colorFadeDuration, colorFadeEaseType )) );
		if (timeOfColorFade >= colorFadeDuration) {
			timeOfColorFade = 0.0f;
			fadeOrangeState = false;
		}

//        float a = Mathf.Lerp(0.0f, 1.0f, timeOfColorFade);
//        renderer.material.SetFloat("_Blend", timeOfColorFade);
//
//        if (timeOfColorFade < 1)
//        {
//            timeOfColorFade += Time.fixedDeltaTime / colorFadeDuration;
//            Debug.Log(timeOfColorFade + "i am time of fade");
//        }

    }


    


    public void fadeIn()
    {
        var color = renderer.material.color;

        color.a += Time.fixedDeltaTime/alphaFadeDuration;
        if ( color.a >= 1.0f ) {
          color.a = 1.0f;
          fadeInState = false;
        }
        renderer.material.color = color;
    }

    public void fadeOut()
    {
        var color = renderer.material.color;

        color.a -= Time.fixedDeltaTime/alphaFadeDuration;
        if ( color.a <= 0.0f ) {
          color.a = 0.0f;
          fadeOutState = false;
        }
        renderer.material.color = color;
    }

    public void AnimateHero()
    {
        //Phoenix
    }

    public void returnToOrigin()
    {
        //Phoenix
    }






}
