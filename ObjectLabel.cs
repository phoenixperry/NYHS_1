using UnityEngine;
using System.Collections;
[RequireComponent (typeof (GUIText))] 

public class ObjectLabel : MonoBehaviour {
    public Transform target;
    public Vector3 offset = Vector3.up; //// Units in world space to offset; 1 unit above object by default
    public bool clampToScreen = false; //if true label will be visible even if object is off screen 
    public float clampBorderSize = 0.05f;// How much viewport space to leave at the borders when a label is being clamped
    public bool useMainCamera = true; // Use the camera tagged MainCamera
    public Camera cameraToUse;  //main camera 
    Camera cam;
    Transform thisTransform;
    Transform camTransform; 


	// Use this for initialization
	void Start () {
        thisTransform = transform;
        if (useMainCamera)
            cam = cameraToUse;
        else    
            camTransform = cam.transform;
        camTransform = cam.transform; 
	}
	
	// Update is called once per frame
	void Update () {
        if (clampToScreen)
        {
            Vector3 relativePosition = camTransform.InverseTransformPoint(target.position);
            relativePosition.z = Mathf.Max(relativePosition.z, 1.0f);
            //Transforms position from world space to local space. The opposite of Transform.TransformPoint.

            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition + offset));
            //Viewport space is normalized and relative to the camera. The bottom-left of the camera is (0,0); the top-right is (1,1). The z position is in world units from the camera.
            relativePosition.z = Mathf.Max(relativePosition.z, 1.0f);
            thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(relativePosition + offset));
            thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1.0f - clampBorderSize), Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1.0f - clampBorderSize), thisTransform.position.z);
        }
        else
        {
            thisTransform.position = cam.WorldToViewportPoint(target.position + offset);
        }
	}
}
