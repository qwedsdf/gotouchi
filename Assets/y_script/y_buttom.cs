using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_buttom : MonoBehaviour {
    const int BUTTON_MAX = 5;
    GameObject[] buttom = new GameObject[BUTTON_MAX];
    int PrefectureNumber;

	// Use this for initialization
	void Start () {
        LoadButton();
        RefreshButton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadButton() {
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            string name="bt_char_" + i;
            buttom[i] = GameObject.Find(name);
        }
    }

    void RefreshButton()
    {
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            buttom[i].SetActive(false);
        }
    }


    //////////////////////ボタン処理///////////////////////
    public void bt_Prefecture(int lPrefectureNumber)
    {
        RefreshButton();
        PrefectureNumber = lPrefectureNumber;
        List<date> PrefectureDate = this.gameObject.GetComponent<y_Datebase>().GetPrefectureDate(lPrefectureNumber);
        Debug.Log("データの数"+PrefectureDate.Count);
        for (int i = 0; i < PrefectureDate.Count; i++) {
            buttom[i].SetActive(true);
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
