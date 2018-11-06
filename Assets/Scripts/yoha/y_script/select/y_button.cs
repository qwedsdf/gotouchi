using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_button : MonoBehaviour {
    const int BUTTON_MAX = 9;
    GameObject[] button = new GameObject[BUTTON_MAX];
    static public int PrefectureNumber;
    int start_num;
    public int PrefectureVolum;
    GameObject[] bt_prefecture;
    public Text txt;
    y_Database DataBaseScript;

    const string BUTTON_CHAR_COMMON_NAME = "bt_char_";
    const string BUTTON_PREFECTURE_COMMON_NAME = "bt_prefecture_";

    //エリア判別用変数
    static public int area_num = 1;
    static public int char_num;
    int larea_num;
	int push_button_num;

    //アスペクト比固定のまま画像を載せるために使う変数
    Vector2 button_size;
    Vector2 img_size;

    //バトルシーンで使うやつ
    static public data now_battle_chardate;


	// Use this for initialization
	void Start () {
        //今押されているボタンを探すためにボタンを配列に格納してる
        //まずはボタンを配列に格納する関数作って
        bt_prefecture = new GameObject[PrefectureVolum];
        DataBaseScript = GameObject.Find("Master").GetComponent<y_Database>();
        larea_num = area_num - 1;
        if (y_game.winflg) {
            bt_GetChar(char_num);
            y_game.winflg = false;
        }
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
            button[i] = GameObject.Find(name);
            button[i].GetComponent<Image>().preserveAspect = true;
        }
        //ボタンの大きさを検知
        button_size.x = button[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.x;
        button_size.y = button[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.y;

        //////////都道府県用のボタン//////////
        start_num = 0;

        for (int i = 0; i < larea_num; i++)
        {
            start_num += y_Database.AreaLenth[i];
        }

        for (int i = 0; i < y_Database.AreaLenth[larea_num]; i++)
        {
            string name = BUTTON_PREFECTURE_COMMON_NAME + i;
            bt_prefecture[i] = GameObject.Find(name);
            bt_prefecture[i].transform.Find("Text").GetComponent<Text>().text = y_Database.Prefecture_names[i+start_num];
        }

        for (int i = y_Database.AreaLenth[larea_num]; i < PrefectureVolum; i++)
        {
            string name = BUTTON_PREFECTURE_COMMON_NAME + i;
            bt_prefecture[i] = GameObject.Find(name);
            bt_prefecture[i].SetActive(false);
        }

    }

    //キャラクターのボタンを初期化
    void RefreshButtonChar()
    {
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            button[i].SetActive(false);
        }
    }

    //都道府県用ボタンを初期化と押したボタンの色を変える
    void RefreshButtonPrefecture(int num) {
        Button button;
        ColorBlock colors;

        for (int i = 0; i < y_Database.AreaLenth[larea_num]; i++)
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
        int tmp_num = start_num + lPrefectureNumber;
        RefreshButtonChar();
        RefreshButtonPrefecture(lPrefectureNumber);
        PrefectureNumber = tmp_num;
        List<data> PrefectureCharDate = new List<data>();
        PrefectureCharDate = DataBaseScript.GetPrefectureData(tmp_num);
        if (PrefectureCharDate == null)
        {
            txt.text += "エラーでてますね";
        }
        //都道府県別のデータを参照し、ボタンにイメージを配置
        for (int i = 0; i < PrefectureCharDate.Count; i++)
        {
            button[i].SetActive(true);
            button[i].GetComponent<Image>().sprite = PrefectureCharDate[i].img;
        }
    }
    public void bt_PushCharButton(int num)
    {
        now_battle_chardate = DataBaseScript.GetCharData(PrefectureNumber,num);
        char_num = num;
        //SceneManager.LoadScene("game"); // 渡邊変更
    }


	////////////////////////////変更（ヨハ）/////////////////////////
	//使うキャラのイメージを入れる
	public void SetPlayerChar(int num)
	{
		// 渡邊変更
		GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
		msgBox.GetComponent<MessageBoxManager>().Initialize_YesNo("このキャラクターで\nいいですか？", SaveUserChara, null);

		push_button_num = num;

		//dataScript.save_data_all.playerdata.use_char = picture.GetComponent<SpriteRenderer>().sprite;
		//dataScript.Save();
	}

	/// <summary>
	/// 渡邊変更
	/// 使用キャラクターを確定する
	/// </summary>
	public void SaveUserChara()
	{
		// to do yoha
		GameData.UserData.UserCharacter = button[push_button_num].GetComponent<Image>().sprite.name;
		SaveData.SetString(SaveKey.UserCharacter, GameData.UserData.UserCharacter);
		//SaveData.SetClass(SaveKey.UserCharacter, GameData.UserData);
		SaveData.Save();

		DataBaseScript.save_data_all.playerdata.use_char = button[push_button_num].GetComponent<Image>().sprite;
		DataBaseScript.Save();

		SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
	}



	public void bt_GetChar(int num) {
        DataBaseScript.GetChar(PrefectureNumber, num);
    }

    public void check(string str)
    {
        txt.text += str + "\n";
    }

    public void LoadAreaSelectScene()
    {
        SceneManager.LoadScene(GameData.Scene_SelectArea); // 渡邊変更
        //SceneManager.LoadScene("select_area");
    }

    public void LoadScean_Picture_Book()
    {
        SceneManager.LoadScene(GameData.Scene_PictureBook); // 渡邊変更
        //SceneManager.LoadScene("picture_book");
    }
}
