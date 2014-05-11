using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DynamicText))]
public class ClickableURL : MonoBehaviour
{
    public string url;
    public Color hiliteColor = Color.white;
    public Camera cam;

    DynamicText dynText;
    Color orgColor;

    void Start()
    {
        dynText = GetComponent<DynamicText>();
        orgColor = dynText.color;
        if (url.Equals(""))
            url = GetComponent<DynamicText>().GetText();
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        dynText.color = orgColor;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool pressed = Input.GetMouseButtonDown(0);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ray = cam.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                pressed = true;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ClickableURL curl = hit.collider.GetComponent<ClickableURL>();
            if (curl && curl == this)
            {
                dynText.color = hiliteColor;
                if (pressed)
                    Application.OpenURL(curl.url);
            }
        }
    }
}
