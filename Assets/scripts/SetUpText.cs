using UnityEngine;
using System.Collections;

public class SetUpText : MonoBehaviour {

    //code for adding database text to screen 
    public string name;
    public string location;
    public string description;
    //you put the data on the camera encase you forgot 
    public GameObject data;
    Person p;
    public GUIStyle descriptionStyle;
    public GUIStyle nameStyle;
    public GUIStyle locationStyle;
    public Camera cam;
    public GameObject closedNode;
    public GameObject openNode;
    Vector3 scaleRatio; 


	void Start () {

        data = GameObject.Find("Data");
        GetData();
        scaleRatio = closedNode.transform.lossyScale;
        //scaleRatio = scaleRatio / 2;
        Debug.Log(scaleRatio);
        
	}

	void Update () {

        //the game object scale must happen in update b/c unity 
        closedNode.transform.localScale = new Vector3(scaleRatio.x, scaleRatio.y, scaleRatio.z);
	}
    void GetData() {
       
        DataPuller.num = 2;
        data.GetComponent<DataPuller>().dataItem();
        p = DataPuller.currentHero;
    
        
    }
    void OnGUI()
    {
        Vector3 currentPos = ScreenToGUI(openNode);
        Debug.Log(closedNode.renderer.bounds.min + "I am the min");
        Debug.Log(currentPos + "prepped");
        currentPos.x = currentPos.x - 460*scaleRatio.x; // this value is computed by getting to the end of the box and the subtracting out the width - so 1
        currentPos.y = currentPos.y + 45*scaleRatio.y; // there's no need to do the subtraction here b/c the bounds y pos is the same 
        descriptionStyle.fixedWidth = 450*scaleRatio.x;
        descriptionStyle.fixedHeight = 439*scaleRatio.y;
; 

        int x= (int)(descriptionStyle.fontSize * scaleRatio.x);
        descriptionStyle.fontSize = x;



        descriptionStyle.wordWrap = true; 
 
        description = GUI.TextArea(new Rect(currentPos.x, currentPos.y, 506, 309), description, descriptionStyle);

        name = name.ToUpper();
        Vector3 pos = ScreenToGUI(closedNode);
        pos.x = pos.x - 340 ;
        pos.y = pos.y + 335; 
        location = location.ToUpper(); 
        //need to adjust for scale 
        name = GUI.TextArea(new Rect(pos.x*scaleRatio.x, pos.y*scaleRatio.y, 390*scaleRatio.x, 50*scaleRatio.y), name, nameStyle);


        Vector3 pos1 = ScreenToGUI(closedNode);
        pos1.x = pos1.x - 340;
        pos1.y = pos1.y + 365; 
        location = GUI.TextArea(new Rect(pos1.x*scaleRatio.x, pos1.y*scaleRatio.y,390*scaleRatio.x,50*scaleRatio.y),location, locationStyle);

        /* 
         1. Get object's current position in screen space. 
         * 2. Flip it and reverse it! 
         * 3. account for the new positioning as an offset 
         * 4. add the offset to the size you get from the 0,0 of the game object 
         * 5. generate the right rect 
         */
        
  

        populateData();
    }
    void populateData()
    {
        Debug.Log(p.description);
        
        description = p.description;
        name = p.familyName + " " + p.givenName + " " + p.lifespan;
        location = p.location; 

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

}
