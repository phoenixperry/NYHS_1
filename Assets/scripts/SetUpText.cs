using UnityEngine;
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


	void Start () {

        data = GameObject.Find("Data");
        GetData();
        scaleRatio = closedNode.transform.lossyScale;
        //scaleRatio = scaleRatio / 2;
        Debug.Log(scaleRatio);
        
	}

	void Update () {

        //the game object scale must happen in update b/c unity 
//        closedNode.transform.localScale = new Vector3(scaleRatio.x, scaleRatio.y, scaleRatio.z);
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
