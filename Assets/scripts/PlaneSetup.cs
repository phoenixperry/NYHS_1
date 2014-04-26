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
   
    public float colorFadeDuration = 2.0f;
    private Color startColor;
    private Color endColor;
    private float timeOfColorFade = 0.0f;

    public GameObject OpenNode; 


    /*
     * Ben - the way we should do the hero and nonhero nodes should be the same except each "layer" should have a z axis min and max. All of the functions should work for each. 
     */
    public Vector3 heroNodeLayerWidth; 
    public Vector3 nodeLayerWidth;

    //also there should be a way to track origin 
    public Vector3 originPos;

    //different possible states 
    private bool fadeInState;
    private bool moveToCenterState;
    private bool animateState;
    private bool fadeOutState;



    public void Setup()
    {
        startColor = gameObject.renderer.material.GetColor("_Color");
        endColor = new Color(0.768f, 0.392f, 0.188f);

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
            
    }
    //each note should set up the data 
    public void setData()
    {
        //Phoenix
        //data.GetComponent<DataPuller>().
    }

    //only heros fade orange 
    public void fadeOrange() {
        //Phoenix 

        renderer.material.color = Color.Lerp(startColor, endColor, timeOfColorFade);
        if (timeOfColorFade < 1)
        {
            timeOfColorFade += Time.deltaTime / colorFadeDuration; 
        }
    }


    public void setOrigin()
    {
        //ben 
        //position on z axis to archive scale 
    }


    public void fadeIn()
    {
        //ben 
    }

    public void fadeOut()
    {
        //ben
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
