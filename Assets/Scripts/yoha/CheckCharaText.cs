using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CheckCharaText : MonoBehaviour {

	TextAsset stageTextAssets;
	string[] name=new string[47];
	int count = 0;

	// Use this for initialization
	void Start () {
		//Sprite sp = Resources.Load("ja-bou") as Sprite;
		//Debug.Log(sp.name);
		//stageTextAssets = Resources.Load("ja-bou") as TextAsset;
		//PrefectureName();
		this.GetComponent<Image>().sprite = Resources.Load("Textures/" + GameData.UserData.CharacterTexName, typeof(Sprite)) as Sprite;

		//テスト
		//string path = "Assets/Resources/Textures";
		//string[] str = Directory.GetFiles(@path, "01_01_*");

		//foreach (string lstr in str)
		//{
		//	Debug.Log(lstr);
		//}
	}

	void PrefectureName()
	{
		StringReader reader = new StringReader(stageTextAssets.text);

		while (reader.Peek()>-1)
		{
			string line = reader.ReadLine();
			name[count] = line;
			count++;
		}
		foreach(string str in name)
		{
			Debug.Log(str);
		}
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
