using UnityEngine;
using System.Collections;

public class TriggerFade : MonoBehaviour {
	
	public Transform parent;
	public SmoothAlpha smoothAlphaScript;
	public float fadeDuration;

	private ArrayList touchList;
	private int touchCount = 0;
	public bool justEntered = false;
	public bool justExited = false;

	// Use this for initialization
	void Start () {
		touchList = new ArrayList();
//		collider.isTrigger = false;
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		if (justEntered) {
			Component[] faders;
			faders = parent.GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				if(fader.gameObject.name != "BodyTextMesh") {
					fader.MakeInvisible(fadeDuration, 0.5f, true);
				}
			}
			justEntered = false;
		}

		if (justExited) {
			Component[] faders;
			faders = parent.GetComponentsInChildren<SmoothAlpha>();
			foreach (SmoothAlpha fader in faders) {
				if(fader.gameObject.name != "BodyTextMesh") {
					fader.MakeVisible(fadeDuration, 1.0f, true);
				}
			}
			justExited = false;
		}
	}

	void OnTriggerEnter(Collider other) {
//		if (other) {
			if (other.gameObject.GetComponent<TriggerFade>()) {
				other.gameObject.GetComponent<TriggerFade>().justEntered = true;
			}
			if (touchCount == 0) {
				justEntered = true;
			}
			if ( touchList.Contains( other.transform ) == false ) {
				touchList.Add(other.transform);
				touchCount++;
			}
//		}
//		Debug.Log("Enter: " + touchCount + ", " + justEntered);
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<TriggerFade>()) {
			other.gameObject.GetComponent<TriggerFade>().justExited = true;
		}
		touchList.Remove(other.transform);
		touchCount--;
		if (touchCount == 0) {
			justExited = true;
		}
//		Debug.Log("Exit: " + touchCount + ", " + justExited);
	}

	public void Reset() {
		foreach ( Transform t in touchList ) {
			Transform plane = t.Find ("GoldPlaneTiltedUp");
			if (plane) {
				Transform p = plane.GetComponent<TriggerFade>().parent;
				Component[] faders = p.GetComponentsInChildren<SmoothAlpha>();
				foreach (SmoothAlpha fader in faders) {
					if(fader.gameObject.name != "BodyTextMesh") {
						fader.MakeVisible(fadeDuration, 1.0f, true);
					}
				}
			}
		}
		touchList.Clear();
		touchCount = 0;
		justEntered = false;
		justExited = false;
	}
}
