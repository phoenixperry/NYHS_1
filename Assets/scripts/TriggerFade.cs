using UnityEngine;
using System.Collections;

public class TriggerFade : MonoBehaviour {
	
	public Transform parent;
	public SmoothAlpha smoothAlphaScript;
	public float fadeDuration;

	private int touchCount;
	public bool justEntered;
	public bool justExited;

	// Use this for initialization
	void Start () {
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
		other.gameObject.GetComponent<TriggerFade>().justEntered = true;
		if (touchCount == 0) {
			justEntered = true;
		}
		touchCount++;
		Debug.Log("Enter: " + touchCount + ", " + justEntered);
	}

	void OnTriggerExit(Collider other) {
		other.gameObject.GetComponent<TriggerFade>().justExited = true;
		touchCount--;
		if (touchCount == 0) {
			justExited = true;
		}
		Debug.Log("Exit: " + touchCount + ", " + justExited);
	}

	public void Reset() {
		touchCount = 0;
		justEntered = false;
		justExited = false;
	}
}
