using UnityEngine;
using System.Collections;

public class OpenNodeController : MonoBehaviour {
	// Use this for initialization
    public Camera cam;
    public Animator anim;
    public float buffer = 1;
    public Vector3 myScale;
    public GameObject photo; 
    void Start()
    {
        Vector3 wrld_width = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        float randXLeft = Random.Range(((-wrld_width.x/2 - wrld_width.x/2+ gameObject.renderer.bounds.size.x/2)), (wrld_width.x - gameObject.renderer.bounds.size.x / 2)-buffer);
        float randYLeft = Random.Range((-wrld_width.y+gameObject.renderer.bounds.size.y/2)+ buffer, (wrld_width.y - gameObject.renderer.bounds.size.y / 2)-buffer);
        float s = Random.Range(0.4f, 0.6f);
        myScale = new Vector3(s, s, s);
        transform.localScale=myScale;
        float zpos = Random.Range(-1.0f, -9.0f);
        transform.position = new Vector3(randXLeft, randYLeft, zpos);
      
      //  setPhoto();
    }
        void Update () {
            /* things to do.
                1: set up the timer. Have it fire after 3 seconds. 
             *  2. Change the trigger on the node animation and send this puppy to the center 
             *  3. after it's at the center, trigger the fade on both this the orage node
             *  4. leave orange node open for x time. 
             *  5. trigger close and 
             *  6. fade yellow node back up 
             *  7. wait for end of this animation 
             *  8. move node back to home position 
             */

         
    }
   
}
