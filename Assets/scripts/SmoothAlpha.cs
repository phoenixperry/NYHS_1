﻿using UnityEngine;
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
	private EasingType alphaEaseType = EasingType.Linear;
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
			if (GetComponent<DynamicText>() != null) {
				origionalColor = GetComponent<DynamicText>().color;

			} else {
				origionalColor = transform.renderer.material.color;
			}
			transparantColor = new Color(origionalColor.r, origionalColor.g, origionalColor.b, 0.0f);
			
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

		// For Fade-out
		if (m_isVisible)
		{
			// For objects with shaders
			if(useShader) {
				transform.renderer.material.SetFloat("_alpha_blend", Mathf.Lerp(oldAlpha, targetAlpha, Easing.EaseIn(stage, alphaEaseType) ));
				if (stage >= 1.0f)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			}
			// For objects without shaders (text, mostly)
			else {
				if (GetComponent<DynamicText>() != null) {
					DynamicText dt = GetComponent<DynamicText>();
					Color c = dt.color;
					dt.color = Color.Lerp(new Color(c.r, c.g, c.b, transitionStartColor.a), new Color(c.r, c.g, c.b, transitionEndColor.a), Easing.EaseIn(stage, alphaEaseType));
					if (dt.color == transitionEndColor) {
						m_isTransitioning = false;
						m_isVisible = targetAlpha > 0.0f;
					}
				} else {
					Color c = transform.renderer.material.color;
					transform.renderer.material.color = Color.Lerp(new Color(c.r, c.g, c.b, transitionStartColor.a), new Color(c.r, c.g, c.b, transitionEndColor.a), Easing.EaseIn(stage, alphaEaseType));
					if (transform.renderer.material.color == transitionEndColor)
					{
						m_isTransitioning = false;
						m_isVisible = targetAlpha > 0.0f;
					}
				}
			}
		}
		// For Fade-in
		else
		{
			// For objects with shaders
			if(useShader) {
				transform.renderer.material.SetFloat("_alpha_blend", Mathf.Lerp(oldAlpha, targetAlpha, Easing.EaseIn(stage, alphaEaseType)) );
				if (stage >= 1.0f)
				{
					m_isTransitioning = false;
					m_isVisible = targetAlpha > 0.0f;
				}
			}
			// For objects without shaders (text, mostly)
			else {
				if (GetComponent<DynamicText>() != null) {
					DynamicText dt = GetComponent<DynamicText>();
					Color c = dt.color;
					dt.color = Color.Lerp(new Color(c.r, c.g, c.b, transitionStartColor.a), new Color(c.r, c.g, c.b, transitionEndColor.a), Easing.EaseIn(stage, alphaEaseType));
					if (dt.color == transitionEndColor)
					{
						m_isTransitioning = false;
						m_isVisible = targetAlpha > 0.0f;
					}
				} else {
					Color c = transform.renderer.material.color;
					transform.renderer.material.color = Color.Lerp(new Color(c.r, c.g, c.b, transitionStartColor.a), new Color(c.r, c.g, c.b, transitionEndColor.a), Easing.EaseIn(stage, alphaEaseType));
					if (transform.renderer.material.color == transitionEndColor)
					{
						m_isTransitioning = false;
						m_isVisible = targetAlpha > 0.0f;
					}
				}
			}
		}
	}

	// Set this object up to fade-out
	public void MakeInvisible(float t = -1.0f, float alpha = 0.0f, EasingType easingType=EasingType.Linear, bool force = false)
	{
		alphaEaseType = easingType;
		alpha = Mathf.Max(0.0f, alpha);
		alpha = Mathf.Min(1.0f, alpha);

		if( t == 0.0f ) {
			if (transform.renderer.material.HasProperty("_Color") ) {
				if (GetComponent<DynamicText>() != null) {
					DynamicText dt = GetComponent<DynamicText>();
					transitionStartColor = dt.color;
					transitionEndColor = new Color(transparantColor.r, transparantColor.g, transparantColor.b, alpha);
					oldAlpha = dt.color.a;
					dt.color = transitionEndColor;
				} else {
				transitionStartColor = renderer.material.color;
					transitionEndColor = new Color(transparantColor.r, transparantColor.g, transparantColor.b, alpha);
					oldAlpha = renderer.material.color.a;
					renderer.material.color = transitionEndColor;
				}
			} else {
				oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
				transform.renderer.material.SetFloat("_alpha_blend", alpha);
			}
			m_isVisible = alpha > 0.0f;
//			return;
		}

		duration = (t >= 0.0f ? t : defaultDuration);

		stage = 0F;

		if (transform.renderer.material.HasProperty("_Color") ) {
			if (GetComponent<DynamicText>() != null) {
				transitionStartColor = GetComponent<DynamicText>().color;
				oldAlpha = transitionStartColor.a;
			} else {
				transitionStartColor = renderer.material.color;
				oldAlpha = renderer.material.color.a;
			}
			transitionEndColor = new Color(transparantColor.r, transparantColor.g, transparantColor.b, alpha);
		} else {
			oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
		}
		targetAlpha = alpha;
		m_isTransitioning = true;
	}

	// Set this object up to fade-in
	public void MakeVisible(float t = -1.0f, float alpha = 1.0f, EasingType easingType=EasingType.Linear, bool force = false)
	{
		alphaEaseType = easingType;
		alpha = Mathf.Max(0.0f, alpha);
		alpha = Mathf.Min(1.0f, alpha);

		duration = (t >= 0.0f ? t : defaultDuration);

		stage = 0F;

		if (transform.renderer.material.HasProperty("_Color") ) {
			if (GetComponent<DynamicText>() != null) {
				transitionStartColor = GetComponent<DynamicText>().color;
				oldAlpha = transitionStartColor.a;
			} else {
				transitionStartColor = renderer.material.color;
				oldAlpha = renderer.material.color.a;
			}
			transitionEndColor = new Color(origionalColor.r, origionalColor.g, origionalColor.b, alpha);
		} else {
			oldAlpha = transform.renderer.material.GetFloat("_alpha_blend");
		}
		targetAlpha = alpha;
		m_isTransitioning = true;
	}
}