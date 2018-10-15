using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_buttom : MonoBehaviour {
    const int BUTTON_MAX = 5;
    GameObject[] buttom = new GameObject[BUTTON_MAX];
    int PrefectureNumber;
    public int PrefectureVolum;
    GameObject[] bt_prefecture;
    public Text txt;

    //アスペクト比固定のまま画像を載せるために使う変数
    Vector2 button_size;
    Vector2 img_size;

	// Use this for initialization
	void Start () {
        //今押されているボタンを探すためにボタンを配列に格納してる
        //まずはボタンを配列に格納する関数作って
        bt_prefecture = new GameObject[PrefectureVolum];
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

        button_size.x = buttom[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.x;
        button_size.y = buttom[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.y;
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
        txt.text += "押されたよ\n";
        RefreshButton();
        PrefectureNumber = lPrefectureNumber;
        txt.text += "番号" + PrefectureNumber+"\n";
        y_Datebase script = this.GetComponent<y_Datebase>();
        txt.text += "ゲッコー終了" + "\n";
        List<date> PrefectureDate = new List<date>();
        PrefectureDate = script.GetPrefectureDate(lPrefectureNumber);
        if (PrefectureDate == null)
        {
            txt.text += "エラーでてますね";
        }
        //都道府県別のデータを参照し、ボタンにイメージを配置
        for (int i = 0; i < PrefectureDate.Count; i++) {
            //アスペクト比を算出
            float aspect_button = button_size.x / button_size.y;
            float aspect_img = PrefectureDate[i].img.bounds.size.x / PrefectureDate[i].img.bounds.size.y;

            buttom[i].SetActive(true);
            buttom[i].GetComponent<Image>().sprite = PrefectureDate[i].img;
            txt.text += PrefectureDate[i].img.name + "\n";
        }
        txt.text += "表示中\n";
    }

    public void bt_GetChar(int num) {
        gameObject.GetComponent<y_Datebase>().GetChar(PrefectureNumber,num);
    }

    public void loadscean()
    {
        SceneManager.LoadScene("picture_book");
    }

    public void chack(string str)
    {
        txt.text += str + "\n";
    }
}
