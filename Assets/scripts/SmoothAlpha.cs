﻿using UnityEngine;
using System.Collections;

public class SmoothAlpha : MonoBehaviour {
	protected Color origionalColor;
	protected Color transparantColor;
	protected float stage;
	
	private bool m_isVisible = true;
	private bool m_isTransitioning = false;

	public float defaultDuration = 0.5f;
	private float duration;
	
	public bool isVisible
	{
		get { return m_isVisible; }
	}
	
	// Use this for initialization
	void Start ()
	{
		origionalColor = transform.renderer.material.color;
		transparantColor = new Color(origionalColor.r, origionalColor.g, origionalColor.b, Color.clear.a);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!m_isTransitioning)
			return;

		if (duration > 0.0f) {
			stage += Time.deltaTime / duration;
		} else {
			stage = 1.0f;
		}

		if (m_isVisible)
		{
			transform.renderer.material.color = Color.Lerp(origionalColor, transparantColor, stage);
			if (transform.renderer.material.color == transparantColor)
			{
				m_isTransitioning = false;
				m_isVisible = false;
			}
		}
		else
		{
			transform.renderer.material.color = Color.Lerp(transparantColor, origionalColor, stage);
			if (transform.renderer.material.color == origionalColor)
			{
				m_isTransitioning = false;
				m_isVisible = true;
			}
		}
	}
	
	public void MakeInvisible(float t = -1.0f)
	{
		duration = (t >= 0.0f ? t : defaultDuration);

		if (m_isTransitioning)
			return;
		
		if (!m_isVisible)
			return;
		
		stage = 0F;
		
		m_isTransitioning = true;
	}
	
	public void MakeVisible(float t = -1.0f)
	{
		duration = (t >= 0.0f ? t : defaultDuration);

		if (m_isTransitioning)
			return;
		
		if (m_isVisible)
			return;
		
		stage = 0F;
		
		m_isTransitioning = true;
	}
}