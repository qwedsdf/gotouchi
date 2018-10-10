using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_pictures : MonoBehaviour {
    y_Datebase DateScript;
	// Use this for initialization
	void Start () {
        DateScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        LoadPicture();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void LoadPicture()
    {
        int count=0;
        string bt_name="bt_char_";
        for (int i = 0; i < 47; i++)
        {
            List<date> list = DateScript.GetPrefectureDate(i);
            for (int f = 0; f < list.Count; f++)
            {
                if (!list[f].Getgetflg()) continue;
                bt_name += count;
                Debug.Log(i);
                GameObject.Find(bt_name).GetComponent<Image>().sprite = list[f].Getimg();
                count++;
                bt_name = "bt_char_";
                if (count > 19) return;
            }
        }
    }
}
