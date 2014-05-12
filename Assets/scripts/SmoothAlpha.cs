using UnityEngine;
using System.Collections;

public class SmoothAlpha : MonoBehaviour {
	protected Color origionalColor;
	protected Color transparantColor;
	protected Color transitionStartColor;
	protected Color transitionEndColor;
	protected float stage;
	
	private bool m_isVisible = true;
	private bool m_isTransitioning = false;

	public float defaultDuration = 0.5f;
	private float oldAlpha = 1.0f;
	private float targetAlpha;
	private float duration;
	private bool useShader = false;


	public bool isVisible
	{
		get { return m_isVisible; }
	}
	
	// Use this for initialization
	void Start ()
	{
		targetAlpha = 0.0f;
		if (transform.renderer.material.HasProperty("_Color") ) {
			origionalColor = transform.renderer.material.color;
			transparantColor = new Color(origionalColor.r, origionalColor.g, origionalColor.b, targetAlpha);
		} else {
			useShader = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!m_isTransitioning)
			return;

		if (duration > 0.0f) {
			stage += Time.fixedDeltaTime / duration;
		} else {
			stage = 1.0f;
		}

		if (m_isVisible)
		{
			if(useShader) {
				transform.renderer.material.SetFloat("_alpha_blend", Mathf.Lerp(oldAlpha, targetAlpha, stage) );
				if (stage >= 1.0f)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			} else {
				transform.renderer.material.color = Color.Lerp(transitionStartColor, transitionEndColor, stage);
				if (transform.renderer.material.color == transitionEndColor)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			}
		}
		else
		{
			if(useShader) {
				transform.renderer.material.SetFloat("_alpha_blend", Mathf.Lerp(oldAlpha, targetAlpha, stage) );
				if (stage >= 1.0f)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			} else {
				transform.renderer.material.color = Color.Lerp(transitionStartColor, transitionEndColor, stage);
				if (transform.renderer.material.color == transitionEndColor)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			}
		}
	}
	
	public void MakeInvisible(float t = -1.0f, float alpha = 0.0f, bool force = false)
	{
		alpha = Mathf.Max(0.0f, alpha);
		alpha = Mathf.Min(1.0f, alpha);

		if( t == 0.0f ) {
			if (transform.renderer.material.HasProperty("_Color") ) {
				transitionStartColor = renderer.material.color;
				transitionEndColor = new Color(transparantColor.r, transparantColor.g, transparantColor.b, alpha);
				oldAlpha = renderer.material.color.a;
				renderer.material.color = transitionEndColor;
			} else {
				oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
				transform.renderer.material.SetFloat("_alpha_blend", alpha);
			}
		}

		duration = (t >= 0.0f ? t : defaultDuration);

//		if (m_isTransitioning && !force) {
//			Debug.Log("make invisible: is transitioning");
//			return;
//		}
		
//		if (!m_isVisible) {
//			Debug.Log("make invisible: is visible");
//			return;
//		}

//		Debug.Log ("MakeInvisible " + oldAlpha + ", " + alpha);

		stage = 0F;

		if (transform.renderer.material.HasProperty("_Color") ) {
			transitionStartColor = renderer.material.color;
			transitionEndColor = new Color(transparantColor.r, transparantColor.g, transparantColor.b, alpha);
			oldAlpha = renderer.material.color.a;
		} else {
			oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
		}
		targetAlpha = alpha;
		m_isTransitioning = true;
	}
	
	public void MakeVisible(float t = -1.0f, float alpha = 1.0f, bool force = false)
	{
		alpha = Mathf.Max(0.0f, alpha);
		alpha = Mathf.Min(1.0f, alpha);

		duration = (t >= 0.0f ? t : defaultDuration);

//		if (m_isTransitioning && !force) {
//			Debug.Log("make visible: is transitioning");
//			return;
//		}
		
//		if (m_isVisible && oldAlpha == 1.0f) {
//			Debug.Log("make visible: is visible");
//			return;
//		}

//		Debug.Log ("MakeVisible " + oldAlpha + ", " + alpha);

		stage = 0F;

		if (transform.renderer.material.HasProperty("_Color") ) {
			transitionStartColor = renderer.material.color;
			transitionEndColor = new Color(origionalColor.r, origionalColor.g, origionalColor.b, alpha);
			oldAlpha = renderer.material.color.a;
		} else {
			oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
		}
		targetAlpha = alpha;
		m_isTransitioning = true;
	}
}