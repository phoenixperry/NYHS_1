using UnityEngine;
using System.Collections;

public class PlaneSetup : MonoBehaviour {

	//this is the current data holder for the gold plane's positional data
    public float radius = 6;
    public float radiusX = 8;
    public float startAngle, range;
    public Vector3 pos = new Vector3();

    public Vector3 posLerp = new Vector3();


    public GameObject data;
    public GameObject photo;

    public float colorFadeDuration = 2.000f;
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
    public Vector3 originPos;

    //different possible states
    private bool fadeInState = false;
    private bool moveToCenterState = false;
    private bool animateState = false;
    private bool fadeOutState = false;



    public void Start()
    {

      
        scaleRatio = gameObject.transform.lossyScale;
 
        cam = Camera.main; 
    }
  

    //Takes game object current point and flips it for GUI space generated from OnGui. 
    Vector3 ScreenToGUI(GameObject go)
    {

        //save the z val so it doesn't get screwed up 
        float zpos = go.transform.position.z;
        //convert to screem space  
        Vector3 bounds = go.renderer.bounds.min;

        Vector3 vals = cam.WorldToScreenPoint(go.renderer.bounds.max);

        //flip the y axis to account for the different spaces 
        vals.y = Screen.height - vals.y;

        vals.z = zpos;
        return vals;
    }


 
   

    public void Update()
    {

        //this should be one big arse state machine.
        if (fadeInState) {
            fadeIn();
        }
        if (moveToCenterState) {
            moveToCenter();
        }
        if (animateState)
        {
            AnimateHero();

        }
        if (fadeOutState)
        {
            fadeOut();
        }

     
        //keeps GUI at scale ratio of chip 
        gameObject.transform.localScale = new Vector3(scaleRatio.x, scaleRatio.y, scaleRatio.z);
     
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
        float a = Mathf.Lerp(1.0f, 0.0f, timeOfColorFade);
        Debug.Log(a);
        renderer.material.SetFloat("_Blend", timeOfColorFade);

        if (timeOfColorFade > 0)
        {
            timeOfColorFade -= Time.deltaTime / colorFadeDuration;
        }

    }
    public void fadeOrange() {
        //Phoenix

        float a = Mathf.Lerp(0.0f, 1.0f, timeOfColorFade);
        renderer.material.SetFloat("_Blend", timeOfColorFade);

        if (timeOfColorFade < 1)
        {
            timeOfColorFade += Time.deltaTime / colorFadeDuration;
            Debug.Log(timeOfColorFade + "i am time of fade");
        }

    }


    public void setOrigin()
    {
		originPos = transform.position;
        Debug.Log("originPos set: " + originPos);
    }


    public void fadeIn()
    {
        var color = renderer.material.color;

        color.a += Time.deltaTime/alphaFadeDuration;
        if ( color.a >= 1.0f ) {
          color.a = 1.0f;
          fadeInState = false;
        }
        renderer.material.color = color;
    }

    public void fadeOut()
    {
        var color = renderer.material.color;

        color.a -= Time.deltaTime/alphaFadeDuration;
        if ( color.a <= 0.0f ) {
          color.a = 0.0f;
          fadeOutState = false;
        }
        renderer.material.color = color;
    }



    public void moveToCenter() {
        //Phoenix
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
