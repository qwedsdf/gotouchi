using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_ChackSaveImg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SarchUserChar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// ユーザーが使用してるキャラの画像を探す
	/// </summary>
	/// この関数自体はどこかのマネージャーに入れてください
	void SarchUserChar()//変更（ヨハ）
	{
		string char_name = SaveData.GetString(SaveKey.UserCharacter);
		for (int i = 0; i < y_Database.PrefectureDataSize; i++)
		{
			List<data> list = y_Database.Prefecturedata[i];
			if (list == null) continue;
			for (int f = 0; f < list.Count; f++)
			{
				if (list[f].name == char_name)
				{
					//画像を入れたい変数（UserDataのimg ?）に入れてください
					GetComponent<SpriteRenderer>().sprite = list[f].img;
				}
			}
		}
	}
}
