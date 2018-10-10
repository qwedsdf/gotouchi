using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_buttom : MonoBehaviour {
    GameObject[] buttom = new GameObject[5];
    int PrefectureNumber;

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

    public void bt_Prefecture(int lPrefectureNumber)
    {
        PrefectureNumber = lPrefectureNumber;
        List<date> PrefectureDate = this.gameObject.GetComponent<y_Datebase>().GetPrefectureDate(lPrefectureNumber);
        for (int i = 0; i < PrefectureDate.Count; i++) {
            buttom[i].GetComponent<Image>().sprite = PrefectureDate[i].Getimg();
        }
    }

    public void bt_GetChar(int num) {
        gameObject.GetComponent<y_Datebase>().GetChar(PrefectureNumber,num);
    }

    public void loadscean()
    {
        SceneManager.LoadScene("picture_book");
    }
}
