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
			Vector3 pos = new Vector3(Screen.width/2+ Random.Range(-1.0f, 1.0f), Screen.height/2+Random.Range(-1.0f, 1.0f), Screen.height/2+Random.Range(-1.0f, 1.0f)); 
			GameObject n = Instantiate(node, pos, Quaternion.identity) as GameObject;
			n.GetComponent<Node>().pos = pos; 
			n.GetComponent<Node>().setBoundary(min,max);
			nodes.Add(n); 
		} 

	}
	
	// Update is called once per frame
	void Update () {
		if(nodes.Count > 0){
			for (int i = 0 ; i < nodes.Count; i++) {
		//		GameObject n = nodes[i] as GameObject; 
//				n.GetComponent<Node>().attract(nodes);
			} 
		}
	}
}
