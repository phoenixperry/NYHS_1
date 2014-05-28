using UnityEngine;
using System.Collections;

public class SinglePath 
{
  
    public float radius = 20;
    public Camera cammy;
    public Vector3 start = new Vector3(0.0f, 0.5f, -2.0f);
    public Vector3 end = new Vector3(1.0f, 0.5f, -2.0f);

    public SinglePath(){
        start = cammy.ViewportToWorldPoint(start);
        start.z = -2.0f;
        end = cammy.ViewportToWorldPoint(end);
        start.z = -2.0f;

    }

} 
