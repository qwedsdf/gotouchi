using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_buttom : MonoBehaviour {
    const int BUTTON_MAX = 9;
    GameObject[] buttom = new GameObject[BUTTON_MAX];
    int PrefectureNumber;
    public int PrefectureVolum;
    GameObject[] bt_prefecture;
    public Text txt;
    y_Datebase DataBaseScript;

    const string BUTTON_CHAR_COMMON_NAME = "bt_char_";
    const string BUTTON_PREFECTURE_COMMON_NAME = "bt_prefecture_";

    //アスペクト比固定のまま画像を載せるために使う変数
    Vector2 button_size;
    Vector2 img_size;

	// Use this for initialization
	void Start () {
        //今押されているボタンを探すためにボタンを配列に格納してる
        //まずはボタンを配列に格納する関数作って
        bt_prefecture = new GameObject[PrefectureVolum];
        DataBaseScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        LoadButton();
        RefreshButtonChar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadButton() {
        ////////キャラが乗る用のボタン//////////
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            string name=BUTTON_CHAR_COMMON_NAME + i;
            buttom[i] = GameObject.Find(name);
            buttom[i].GetComponent<Image>().preserveAspect = true;
        }
        //ボタンの大きさを検知
        button_size.x = buttom[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.x;
        button_size.y = buttom[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.y;

        //////////都道府県用のボタン//////////
        for (int i = 0; i < PrefectureVolum; i++)
        {
            string name = BUTTON_PREFECTURE_COMMON_NAME + i;
            bt_prefecture[i] = GameObject.Find(name);
            bt_prefecture[i].transform.Find("Text").GetComponent<Text>().text = y_Datebase.Prefecture_names[i];
        }
    }

    //キャラクターのボタンを初期化
    void RefreshButtonChar()
    {
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            buttom[i].SetActive(false);
        }
    }

    //都道府県用ボタンを初期化と押したボタンの色を変える
    void RefreshButtonPrefecture(int num) {
        Button button;
        ColorBlock colors;

        for (int i = 0; i < PrefectureVolum; i++)
        {
            button = bt_prefecture[i].GetComponent<Button>();
            colors = button.colors;
            colors.normalColor = new Color(255f, 255f, 255f, 255f);
            colors.highlightedColor = new Color(255f, 255f, 255f, 255f);
            button.colors = colors;
        }
        button = bt_prefecture[num].GetComponent<Button>();
        colors = button.colors;
        colors.normalColor = new Color(255f, 0f, 0f, 255f);
        colors.highlightedColor = new Color(255f, 0f, 0f, 255f);
        button.colors = colors;
    }


    //////////////////////ボタン処理///////////////////////
    public void bt_Prefecture(int lPrefectureNumber)
    {
        Debug.Log("押したよ");
        RefreshButtonChar();
        RefreshButtonPrefecture(lPrefectureNumber);
        PrefectureNumber = lPrefectureNumber;
        List<date> PrefectureCharDate = new List<date>();
        PrefectureCharDate = DataBaseScript.GetPrefectureDate(lPrefectureNumber);
        if (PrefectureCharDate == null)
        {
            txt.text += "エラーでてますね";
        }
        //都道府県別のデータを参照し、ボタンにイメージを配置
        for (int i = 0; i < PrefectureCharDate.Count; i++)
        {
            buttom[i].SetActive(true);
            buttom[i].GetComponent<Image>().sprite = PrefectureCharDate[i].img;
        }
    }

    public void bt_GetChar(int num) {
        DataBaseScript.GetChar(PrefectureNumber, num);
    }

    public void LoadsScean_Picture_Book()
    {
        SceneManager.LoadScene("picture_book");
    }

    public void chack(string str)
    {
        txt.text += str + "\n";
    }
}
