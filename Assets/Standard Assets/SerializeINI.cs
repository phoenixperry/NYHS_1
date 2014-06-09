using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class SerializeINI : MonoBehaviour
{
	public Camera mainCamera;
	public Transform logo;

//	public static void Serialize(string name, string GamePath)
//	{
//		System.Text.StringBuilder sb = new System.Text.StringBuilder();
//		sb.AppendLine("name=" + name);
//		System.IO.StreamWriter writer = new System.IO.StreamWriter(GamePath);
//		writer.Write(sb.ToString());
//		writer.Close();
//	}

	public void Start() {
//		StartCoroutine( wait () );
		string cfgPath = "./config.txt";
//		if (System.IO.Directory.Exists(cfgPath)) {
			DeSerialize(cfgPath);

//		} else {
//			Debug.Log("No config file found: " + cfgPath);
//		}
	}

	public IEnumerator wait() {
		float t = 0;
		while (t < 1.0f) {
			t+= Time.fixedDeltaTime;
			yield return 10;
		}
		DeSerialize("./config.txt");
	}
	
	public void DeSerialize(string GamePath)
	{
		foreach( string line in System.IO.File.ReadAllLines(GamePath) )
		{
			// STRIP WHITESPACE, FORCE LOWERCASE
			string formattedLine = Regex.Replace(line, @"[^\w\.,@#=-]", "").ToLower();

			// SKIP COMMENTS

			if (formattedLine == "" || formattedLine.ToCharArray()[0] == '#' )
				continue;
			Debug.Log(formattedLine);
			int i;
			string[] id_value = formattedLine.Split('=');

			switch (id_value[0])
			{
			case "camera_pos":
				float[] cPos = new float[3];
				i = 0;
				foreach (string s in id_value[1].Split(',')) {
					cPos[i] = float.Parse(s);
					i++;
				}
				mainCamera.transform.position = new Vector3(cPos[0], cPos[1], cPos[2]);
				Debug.Log("Config camera_pos: " + cPos[0] + ", " + cPos[1] + ", " + cPos[2]);
				break;
			case "logo_pos":
				float[] lPos = new float[3];
				i = 0;
				foreach (string s in id_value[1].Split(',')) {
					lPos[i] = float.Parse(s);
					i++;
				}
				logo.transform.position = new Vector3(lPos[0], lPos[1], lPos[2]);
				Debug.Log("Config logo_pos: " + lPos[0] + ", " + lPos[1] + ", " + lPos[2]);
				break;
			case "resolution":
				string[] rez = id_value[1].Split('x');
				Screen.SetResolution(int.Parse(rez[0]), int.Parse(rez[1]), true);
				Debug.Log("Config resolution: " + rez[0] + ", " + rez[1]);
				break;
			case "aspect_ratio":
				string aspect = id_value[1];
				mainCamera.GetComponent<AspectUtility>()._wantedAspectRatio = float.Parse(aspect);
				AspectUtility.SetAspectRatio(float.Parse(aspect));
				AspectUtility.SetCamera();
				Debug.Log("Config aspect ratio = " + aspect);
				break;
			default:
				Debug.Log("Unrecognized variable: " + id_value[0]);
				break;
			}
		}
	}

	// Update is called once per frame
	public void Update ()
	{
//		if (load)
//		{
//			if (!done)
//			{
//				name = SerializeINI.DeSerialize(name, gamePath);
//				done = true;
//			}
//			else
//			{
//				//Destroy(gameObject);
//			}
//		}
	}
}