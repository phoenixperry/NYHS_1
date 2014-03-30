using UnityEngine;
using System.Collections;

public class ScreenControl : MonoBehaviour
{
	public GameObject blankNode; 
	public int numNodes; 
	public ArrayList nodes; 
	void Start(){
		nodes = new ArrayList(); 
		for(int i=0; i<numNodes; i++)
		{
			GameObject go = Instantiate(blankNode, transform.position, Quaternion.identity) as GameObject; 
			nodes.Add(go); 
		}
	} 
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			for(int i=0; i<numNodes; i++)
			{
				//Destroy(nodes[i]); 

			}
			for(int i=0; i<numNodes; i++)
			{
				GameObject go = Instantiate(blankNode, transform.position, Quaternion.identity) as GameObject; 
				nodes.Add(go); 
			}
		}
	}
}
