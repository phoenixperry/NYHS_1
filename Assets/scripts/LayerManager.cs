using UnityEngine;
using System.Collections;

public class LayerManager : MonoBehaviour {
	public GameObject node; 
	private ArrayList nodes; 
	public int numNodes=20; 
	public Vector3 min; 
	public Vector3 max;
	// Use this for initialization
	void Start () {
		nodes = new ArrayList();
		for (int i = 0 ; i < numNodes; i++) {
			Vector3 pos = new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f), Random.Range(-1f,50f)); 
			Quaternion rot = node.GetComponent<Transform>().rotation; 
			GameObject n = Instantiate(node, pos, rot ) as GameObject;
			n.GetComponent<Node>().pos = pos; 
			n.GetComponent<Node>().setBoundary(min,max);
			nodes.Add(n); 
		} 

	}
	
	// Update is called once per frame
	void Update () {

			for (int i = 0 ; i < nodes.Count; i++) {
				GameObject n = nodes[i] as GameObject; 
				n.GetComponent<Node>().attract(nodes);
               // if(n.transform.position.x < n.GetComponent<Node>().maxX && n.transform.position.x > n.GetComponent<Node>().minX && n.transform.position.y < n.GetComponent<Node>().maxY && n.transform.position.y > n.GetComponent<Node>().minY && n.transform.position.z < n.GetComponent<Node>().maxZ && n.transform.position.z > n.GetComponent<Node>().minZ) 
                    n.transform.position = n.GetComponent<Node>().pos + n.GetComponent<Node>().pVelocity; 
               
		}
	}
}
