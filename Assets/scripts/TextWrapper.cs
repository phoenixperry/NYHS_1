using UnityEngine;
using System.Collections;

public class TextWrapper : MonoBehaviour {

	public TextSize ts;
	public float lineWidth = 10.0f;

	// Use this for initialization
	void Start () {
		ts = new TextSize(gameObject.GetComponent<TextMesh>());
		Debug.Log("Initial width of string: " + ts.width);
		ts.FitToWidth(lineWidth);
	}

	void Awake() {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetText() {
		if(ts != null) {
			Debug.Log("Initial width of string: " + ts.width);
			ts.FitToWidth(lineWidth);
		}
	}
}
