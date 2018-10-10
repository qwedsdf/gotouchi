using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_buttom : MonoBehaviour {
    GameObject[] buttom = new GameObject[3];

	// Use this for initialization
	void Start () {
        LoadButtom();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadButtom() {
        for (int i = 0; i < 3; i++) {
            string name="bt_char_" + i;
            buttom[i] = GameObject.Find(name);
        }
    }

    public void bt_Prefecture(int PrefectureNumber)
    {
        Debug.Log("押したよー");
        List<date> PrefectureDate = this.gameObject.GetComponent<y_Datebase>().GetPrefectureDate(PrefectureNumber);
        for (int i = 0; i < PrefectureDate.Count; i++) {
            buttom[i].GetComponent<Image>().sprite = PrefectureDate[i].Getimg();
        }
    }
}
