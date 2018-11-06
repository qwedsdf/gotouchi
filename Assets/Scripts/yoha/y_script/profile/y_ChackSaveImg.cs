using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_ChackSaveImg : MonoBehaviour {
	y_Database DataBaseScript;

	// Use this for initialization
	void Start () {
		DataBaseScript = GameObject.Find("Master").GetComponent<y_Database>();
		string char_name= SaveData.GetString(SaveKey.UserCharacter);
		for (int i=0;i < y_Database.PrefectureDataSize; i++)
		{
			List<data> list = DataBaseScript.GetPrefectureData(i);
			if (list == null) continue;
			for (int f = 0; f < list.Count; f++)
			{
				if(list[f].name == char_name)
				{
					GetComponent<SpriteRenderer>().sprite = list[f].img;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
