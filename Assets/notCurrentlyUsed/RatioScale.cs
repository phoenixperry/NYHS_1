using UnityEngine;
using System.Collections;

public class RatioScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float node_width_ratio = .52083f; 
        float node_height_ratio = .4629629f; 

        float w = Screen.width;
        float h = Screen.height;
 
        Vector3 currentSize = renderer.bounds.size;
        float currentRatio_width = currentSize.x/w; 
        float currentRatio_height = currentSize.y/h;

        float newRatio_width = currentRatio_width - node_width_ratio;
        float newRatio_height = currentRatio_height - node_height_ratio;
        newRatio_width = Mathf.Abs(newRatio_width);
        newRatio_height = Mathf.Abs(newRatio_height);
        gameObject.transform.localScale = new Vector3(newRatio_width, newRatio_height, 1); 
             
      
     

         
    }
	
	// Update is called once per frame
	void Update () {
        float node_width_ratio = .2292f;
        float node_height_ratio = .4630f;

        float w = Screen.width;
        float h = Screen.height;

        Vector3 currentSize = renderer.bounds.size;
        float currentRatio_width = currentSize.x / w;
        float currentRatio_height = currentSize.y / h;

        float newRatio_width = currentRatio_width - node_width_ratio;
        float newRatio_height = currentRatio_height - node_height_ratio;
        newRatio_width = Mathf.Abs(newRatio_width);
        newRatio_height = Mathf.Abs(newRatio_height);
        Debug.Log(newRatio_height+  "is the height " + newRatio_width + "is the width");
       // gameObject.transform.localScale = new Vector3(newRatio_width+1, newRatio_height+1, 1); 
             
	}
}
