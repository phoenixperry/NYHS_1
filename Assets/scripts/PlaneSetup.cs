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


    /*
     * Ben - the way we should do the hero and nonhero nodes should be the same except each "layer" should have a z axis min and max. All of the functions should work for each. 
     */
    public Vector3 heroNodeLayerWidth; 
    public Vector3 nodeLayerWidth;

    //also there should be a way to track origin 
    public Vector3 originPos;

    //each note should set up the data 
    public void setData()
    {
        //Phoenix 
    }

    //only heros fade orange 
    public void fadeOrange() {
        //Phoenix 

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
