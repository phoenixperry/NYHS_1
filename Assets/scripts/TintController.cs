using UnityEngine;
using System.Collections;

public class TintController : MonoBehaviour {

	Color originalColor = Color.white;

	// Use this for initialization
	void Start () {
		if (renderer.material.HasProperty("_Color")) {
			originalColor = renderer.material.GetColor("_Color");
			originalColor = new Color (originalColor.r, originalColor.g, originalColor.b, 1.0f);
		} else {
			originalColor = renderer.material.GetColor("_TintColor");
			originalColor = new Color (originalColor.r, originalColor.g, originalColor.b, 1.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTint( Color color, float duration){
		StartCoroutine(Tint (color, duration));
	}

	public void UnTint( float duration) {
		StartCoroutine(Tint (originalColor, duration));
	}

	IEnumerator Tint( Color color, float duration) {
		float t = 0.0f;
		if (renderer.material.HasProperty("_Color")) {
			Color startColor = renderer.material.GetColor("_Color");
//			Color destColor = originalColor * color;
			while( t < duration) {
				t += Time.fixedDeltaTime;
				renderer.material.SetColor("_Color", Vector4.Lerp(new Color(startColor.r, startColor.g, startColor.b, renderer.material.GetColor("_Color").a),
				                                                  new Color(color.r, color.g, color.b, renderer.material.GetColor("_Color").a),
				                                                  t/duration));
				yield return 0;
			}
			renderer.material.SetColor("_Color", color);
		} else {
			Color startColor = renderer.material.GetColor("_TintColor");
//			Color destColor = originalColor * color;
			while( t < duration) {
				t += Time.fixedDeltaTime;
				renderer.material.SetColor("_TintColor", Vector4.Lerp(new Color(startColor.r, startColor.g, startColor.b, renderer.material.GetColor("_TintColor").a),
				                                                      new Color(color.r, color.g, color.b, renderer.material.GetColor("_TintColor").a),
				                                                  	  t/duration));
				yield return 0;
			}
			renderer.material.SetColor("_TintColor", color);
		}
	}
}
