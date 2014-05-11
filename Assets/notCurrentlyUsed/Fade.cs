using UnityEngine;
using System.Collections;


public class Fade : MonoBehaviour {

    //vars to track time

	//lerp method vars  
	public Vector3 size;
	//public GameObject openNode; 

	Vector3 startMarker, endMarker;
    public Camera cam; 
	//---------------------------------------

	//color lerp vars 
	public float fadespeed = 10f; 
	//public SpriteRenderer sr; 
	float smoothing = 1f; 
	Color color;
    public Animator anim;
    public GameObject openNode;
    public Vector3 openSize;
    public float dist;

    // Use this for initialization
	void Start () {
		//color = sr.material.color; 
		color.a = 0.4f; 
	        Vector3 s = new Vector3(1.22f, 1.22f, 1.1f);
            transform.localScale = s;
            anim.SetTrigger("Closed_fade_in");
		setupLerp();
        


	}


	// Update is called once per frame
	void Update () {

		//sr = gameObject.GetComponent<SpriteRenderer>(); 
		//sr.material.color = Color.Lerp(sr.material.color, color, fadespeed *Time.fixedDeltaTime); 
		if(Input.GetKeyDown(KeyCode.Space)){

			StartCoroutine(moveToCenter());
            Invoke("fadeOut", 2 );
            Invoke("fadeInOpen", 2);

            //anim.SetTrigger("Closed_fade_out"); 

		}
        
        if(Input.GetKeyDown(KeyCode.D)){
            anim.SetTrigger("Closed_done");
            anim.SetTrigger("Closed_fade_out"); 
            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetTrigger("Closed_fade_in");
            anim.SetTrigger("Closed_done");
        }
     //   Debug.Log(cam.WorldToViewportPoint(renderer.bounds.min) + "the left side");
     //   Debug.Log(cam.WorldToViewportPoint(renderer.bounds.max) + "the far right side");
	}


    //IEnumerator colorFadeUp(float startVal, float endVal, GameObject obj)
    //{
		
    //    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>(); 
    //    while(sr.material.color.a < endVal){
			
    //        sr.material.color = Color.Lerp(sr.material.color, color, fadespeed *Time.fixedDeltaTime); 
    //        yield return null; 
			
    //    }
    //    yield return null;
    //}

	IEnumerator moveToCenter()
	{	Debug.Log("coroutine");
		dist = Vector3.Distance(transform.position, endMarker);
		while(dist>.02f){	

			transform.position = Vector3.Lerp(transform.position, endMarker, dist);
            endMarker = offestBox(); 
            dist = Vector3.Distance(transform.position, endMarker);
			yield return null;
        }
    }
    void fadeInOpen() {
       
    }
    void fadeOut()
    {
        anim.SetTrigger("Closed_fade_out");
        openNode.GetComponent<Animator>().SetTrigger("node_open_fade_up");
       // openNode.GetComponent<Animator>().SetTrigger("node_open_fade_in");
    } 
	void setupLerp(){
		
		//get Screen size 
		float xpos = Screen.width; 
		float ypos = Screen.height; 
		
		
		//devide screen size by meter space to get possible range from center 
		xpos = (xpos/100)/2; 
		ypos = (ypos/100)/2; 
		
		float xpos_negative = xpos/2 *-1; 
		float ypos_negative = ypos/2 *-1; 
		
		//x and y now have ranges for the width and height in world space 
		
		//account for object's size 
		Vector3 size = gameObject.renderer.bounds.size;
        Debug.Log("i am size" + size.x); 
		//put the object somewhere on screen randomly 
		//figure out how to make these dynamic - this is ghetto 
        float r1 = Random.Range(xpos_negative+size.x/2, xpos - size.x/2); 
		float r2 = Random.Range(ypos_negative + size.y/2, ypos - size.y/2);
        Debug.Log(r1 + " & " + r2 + "from range");
		transform.position = new Vector3(r1,r2,-1);
       
        //get the position relative to open node 
        

		//set a marker for that oriiginal position 
		startMarker = transform.position; 
		
		//get the center of the screen in relation to our object in 
		Vector3 p = cam.ScreenToWorldPoint( new Vector2(Screen.width/2, Screen.height/2));
        Debug.Log(p+ "I am p"); 
		Vector3 newP = new Vector3(p.x, p.y, -1); 
		//create an end point for that marker
	//	endMarker = newP;
 
		
	}

    Vector3 offestBox()
    {
//move open to 
        Vector3 leftSide = cam.WorldToViewportPoint(openNode.GetComponent<Renderer>().bounds.min);
        Debug.Log("left _ done" + leftSide);
        Vector3 currentCenter = cam.WorldToViewportPoint(renderer.bounds.extents);
        Vector3 loc = new Vector3((leftSide.x), (leftSide.y), -1);

    return loc;
       
    }
    ///control how the animations loop 

    IEnumerator HitBehavior()
    {
        yield return new WaitForSeconds(2); 
    }
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("triggered");
        anim.SetTrigger("Closed_triggerEnter");
        StartCoroutine(HitBehavior());
    }   
 
    void OnTriggerExit(Collider col)
    {
        anim.SetTrigger("Closed_triggerExit");
    }
}
