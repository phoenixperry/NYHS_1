using UnityEngine;
using System.Collections;

public class screenManager : MonoBehaviour {
    //gets data out of the script when needed as a staic var 
    public static Vector3 coords; 
    //public GUIText width_;
    //public GUIText height_; 

    //transform target to current if need be
    private Vector2 scaleRatio;
    public Camera cam;
    //public GameObject hills;
    public int targetScreenWidth;
    public int targetScreenHeight;
    //change this only if you changed it globally in Unity's settings. 
    public int defaultPixelToMeter = 100; 
	// Use this for initialization
	void Start () {
        //this just prints the text to screen for my debugging /testing 
        float x = Screen.width;
        //width_.text =x.ToString();
        float y = Screen.height;
        //height_.text = y.ToString();

        //set up the scale ratio between the target and the current screen sizes. Must be run at start to establish scale ratio.
        MultiScreen(new Vector2(targetScreenHeight, targetScreenWidth));

        //moves a game object relative to the bottom right 
       // moveRelativeToBottomLeft(gameObject, 0, 100, 0);

     
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //moves an object to this position in pixels in UNITY'S DEFAULT SCREEN COORDINATE SYSTEM. The center is  0,0. 
       // InPixels(gameObject, 10, 100, -1);

        //ADOBE TO UNITY PIXEL SPACE -- 
        //Note: How your object aligns is in relation to the spritAlignment setting in the texture importer. Set pivot to the top left corner in the sprite texture importer for behavior exactly like the adobe suite for positioning. the default is from the center of the sprite. note I am working with an orthographic camera. And yes, I just referenced Missy Elliot. Future notes: Make this a matrix. 
        //the signiture is zdepth, xposition and yposition 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 test = FlipItAndReverseIt(-1, 0, 150);
            test.z = -2;
            transform.position = test;
        }
        //Parents a child and moves it in relation to parents position in Unity's world coordinate space. 
        //the method signiture is parent, child, x,y,z location of child to parent
      //  RelativeToParent(gameObject.transform, hills, 10,0,0);

        //this moves the child from the 0,0 point of the bottom left of the screen and leaves you with parented to the parent transform
      //  RelativeToParent_bottomLeft(gameObject.transform, hills, 0, 100, 1); 
       
        
    }

    //what resolution you originally targeted with your design work 
    void MultiScreen(Vector2 targetScreen)
    {
        //ideally make your adobe's document width the width of the game. I'm going to normalize it for other screenspaces here too for any screen to work with your game design.   
        Vector2 currentScreen = new Vector2(Screen.width, Screen.height);
        float xScaleRatio = targetScreen.x / currentScreen.x;
        float yScaleRatio = targetScreen.y / currentScreen.y;
        scaleRatio = new Vector2(xScaleRatio, yScaleRatio);
        Debug.Log(scaleRatio + "is the offset");
        //returning in screen space 
    }


    //where you want it to live on the screen in pixels targeted from Unity's default screen space. 
    void InPixels(GameObject obj, int xloc, int yloc, float zDepth)
    {
       
        Vector3 loc = cam.ScreenToWorldPoint(new Vector3 (xloc, yloc,zDepth));
        Debug.Log(loc +"is the x point in world space" + "is the y point in world space");
        Vector3 testTheory = cam.WorldToScreenPoint(loc); 
        Debug.Log(testTheory + "is what happens when you take this back to pixels"); 
        //factor in any offset for screen Res change from our initial design space to support multiple screen sizes. 
        float xOff = loc.x * scaleRatio.x;
        float yOff = loc.y * scaleRatio.y;
        loc = new Vector2(xOff, yOff);
        obj.transform.position = new Vector3(loc.x, loc.y, zDepth) ;
    }

    //Parent one object to another and move them accordingly. Note after this function they are parented to each other so if you don't want that you'll need to un-parent them. This uses unity's default coord system and stuff moves from the center of the screen. 
    void RelativeToParent(Transform theParent, GameObject theChild, float xpos, float ypos, float zDepth)
    {   
        //this moves things in Unity's world space in pixels, where the center of the screen is 0,0 
        theChild.transform.parent = theParent.transform;
        Vector3 newpos = new Vector2(xpos / defaultPixelToMeter, ypos / defaultPixelToMeter);
        theChild.transform.localPosition = newpos;
    }

    //god damn it behave like adobe's screen space - you break my brain. arrghhhh. No, like seriously, you hurt us Unity. Designers are used to thinking about the upper left corner as (0,0). All of their art is laid out with those values, making the conversion a total flipping nightmare. This function lets you account for that and lay graphics out as they are in photoshop. If you're default photoshop screen width and height don't match the current screen size, it will also acccount for the difference here and do it's best to keep the layout intact.
    //this flips the screen grid and spits back a Vector3 in GUI space effectively, same as photoshop's grid
    Vector3 FlipItAndReverseIt(float targetZ, float xpos_, float ypos_ )
    {

        //xpos_ *= scaleRatio.x; 
        //ypos_ *= scaleRatio.y; 
        
        //flip the y axis to account for the different spaces 
        ypos_ = targetScreenHeight - ypos_;
        Debug.Log(targetScreenHeight + "screen" + ypos_);
        //get this point in world space
            
        Vector3 ScaledToWorld = cam.ScreenToWorldPoint(new Vector3(xpos_, ypos_, 1));
        ScaledToWorld.z = targetZ;
        Debug.Log(ScaledToWorld + "i am log");
        transform.position = ScaledToWorld; 
        return ScaledToWorld; 
    }

    //Takes game object current point and flips it for GUI space generated from OnGui. 
    void ScreenToGUI(Vector3 vals)
    {
        //save the z val so it doesn't get screwed up 
        float zpos = vals.z;
        //convert to screem space  
        vals = cam.WorldToScreenPoint(vals); 
        
        //flip the y axis to account for the different spaces 
        vals.y = Screen.height - vals.y;

        vals.z = zpos;
        coords =  vals;
    }



    void RelativeToParent_bottomLeft(Transform theParent, GameObject theChild, float xpos, float ypos, float zDepth)
    {
      
        //this moves things in Unity's world space in pixels, where the center of the screen is 0,0 
        Vector3 getParentLoc = cam.WorldToScreenPoint(theParent.position);
        Debug.Log(getParentLoc + " is the parent in screen coords");
        Vector3 toAdd = new Vector3(xpos + getParentLoc.x, ypos + getParentLoc.y, theParent.position.z + zDepth);
        Debug.Log(toAdd + "is the move in local space");
      
        toAdd = cam.ScreenToWorldPoint(toAdd);
        toAdd.z = theParent.position.z + zDepth;
        theChild.transform.position = toAdd;
        Debug.Log("this parent loc" + theParent.position + " this child loc " + theChild.transform.position);
        theChild.transform.parent = theParent.transform;


    }

    void moveRelativeToBottomLeft(GameObject g, int xpos, int ypos, int zDepth)
    {
        Vector3 currentG = cam.WorldToScreenPoint(g.transform.position);
        currentG.x = currentG.x + xpos;
        currentG.y += ypos; 
        currentG = cam.ScreenToWorldPoint(currentG);
        currentG.z = g.transform.position.z + zDepth;
        g.transform.position = currentG;
        Debug.Log(currentG);

    }

}

