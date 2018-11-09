using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadPrefectureNmaeByText()
	{
		List<string> list = new List<string>();
		TextAsset text = Resources.Load("PrefectureName") as TextAsset;
		string AllPrefectureName = text.text;

		string[] PrefectureName = AllPrefectureName.Split('\n');
		foreach (string str in PrefectureName)
		{
			Debug.Log(str);
		}
	}
}
