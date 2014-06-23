using UnityEngine;
using System.Collections;

public class LogoController : MonoBehaviour {

	protected static float pixelsToUnits = 0.00269081f;

	public float fadeDuration = 2.0f;

	// Use this for initialization
	void Start () {
		string path = @"file://" + System.IO.Directory.GetCurrentDirectory() + "/Logo/logo.png";
		Debug.Log(path);
		WWW www = new WWW(path);
		while (!www.isDone); // wait for file load to complete
		Debug.Log ("logo done");
		Texture2D img = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
		www.LoadImageIntoTexture(img);

		Debug.Log(img.width.ToString() + " " + img.height.ToString ());
		Debug.Log (transform.localScale.ToString());
		transform.localScale = new Vector3(img.width * pixelsToUnits, img.height * pixelsToUnits, 0.0f);
		Debug.Log (transform.localScale.ToString());
		GetComponent<Renderer>().material.SetTexture("_image", img);
		StartCoroutine(fadeIn());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator fadeIn() {
		float t = 0.0f;
		while (t < fadeDuration) {
			t += Time.fixedDeltaTime;
			GetComponent<Renderer>().material.SetFloat("_alpha_blend", t/fadeDuration);
			yield return 0;
		}
	}
}
