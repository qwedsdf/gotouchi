using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour
{
    const int BUTTON_MAX = 9;
    GameObject[] buttonObj = new GameObject[BUTTON_MAX];
    static public int PrefectureNumber;
    int start_num;
    public int PrefectureVolum;
    GameObject[] bt_prefecture;
    public Text txt;
    Database DataBaseScript;

    const string BUTTON_CHAR_COMMON_NAME = "bt_char_";
    const string BUTTON_PREFECTURE_COMMON_NAME = "bt_prefecture_";

    //エリア判別用変数
    static public int area_num = 1;
    static public int char_num;
    int push_button_num;

    //アスペクト比固定のまま画像を載せるために使う変数
    Vector2 button_size;
    Vector2 img_size;

    //バトルシーンで使うやつ
    static public data now_battle_chardate;


    // Use this for initialization
    void Start()
    {
        //今押されているボタンを探すためにボタンを配列に格納してる
        //まずはボタンを配列に格納する関数作って
        bt_prefecture = new GameObject[PrefectureVolum];
        DataBaseScript = GameObject.Find("Managers/Master").GetComponent<Database>();
        //if (y_game.winflg)
        //{
        //    bt_GetChar(char_num);
        //    y_game.winflg = false;
        //}
        LoadButton();
        RefreshButtonChar();
    }

    // Update is called once per frame
    void Update()
    {

    }

	/// <summary>
	/// ボタンオブジェクトを読み込む
	/// </summary>
    void LoadButton()
    {
        ////////キャラが乗る用のボタン//////////
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            string name = BUTTON_CHAR_COMMON_NAME + i;
            buttonObj[i] = GameObject.Find(name);
            buttonObj[i].GetComponent<Image>().preserveAspect = true;
        }
        //ボタンの大きさを検知
        button_size.x = buttonObj[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.x;
        button_size.y = buttonObj[BUTTON_MAX - 1].GetComponent<RectTransform>().sizeDelta.y;

        //////////都道府県用のボタン//////////
        start_num = 0;

        for (int i = 0; i < area_num; i++)
        {
            start_num += Database.AreaLenth[i];
        }

        for (int i = 0; i < Database.AreaLenth[area_num]; i++)
        {
            string name = BUTTON_PREFECTURE_COMMON_NAME + i;
            bt_prefecture[i] = GameObject.Find(name);
            bt_prefecture[i].transform.Find("Text").GetComponent<Text>().text = Database.Prefecture_names[i + start_num];
		}

        for (int i = Database.AreaLenth[area_num]; i < PrefectureVolum; i++)
        {
            string name = BUTTON_PREFECTURE_COMMON_NAME + i;
            bt_prefecture[i] = GameObject.Find(name);
            bt_prefecture[i].SetActive(false);
        }

    }

	/// <summary>
	/// キャラクターのボタンを初期化
	/// </summary>
	void RefreshButtonChar()
    {
        for (int i = 0; i < BUTTON_MAX; i++)
        {
            buttonObj[i].SetActive(false);
        }
    }

	/// <summary>
	/// 都道府県用ボタンを初期化と押したボタンの色を変える
	/// </summary>
	/// <param name="num">都道府県ボタンの番号</param>
	void RefreshButtonPrefecture(int num)
    {
        Button button;
        ColorBlock colors;

        for (int i = 0; i < Database.AreaLenth[area_num]; i++)
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

	/// <summary>
	/// 都道府県ボタンを押したときにキャラボタンに画像を貼る
	/// </summary>
	/// <param name="lPrefectureNumber">都道府県ボタンの番号</param>
    public void bt_Prefecture(int num)
    {
		Debug.Log("押したよ");
		GameData.UserData.PrefecturesId = start_num + num + 1;
		//RefreshButtonChar();
		//RefreshButtonPrefecture(lPrefectureNumber);
		//PrefectureNumber = tmp_num;
		//List<data> PrefectureCharDate = new List<data>();
		//PrefectureCharDate = DataBaseScript.GetPrefectureData(tmp_num);


		//if (PrefectureCharDate == null)
		//{
		//    txt.text += "エラーでてますね";
		//}

		//都道府県別のデータを参照し、ボタンにイメージを配置
		//for (int i = 0; i < PrefectureCharDate.Count; i++)
		//{
		//    buttonObj[i].SetActive(true);
		//    buttonObj[i].GetComponent<Image>().sprite = PrefectureCharDate[i].img;
		//}

		//テスト
		//都道府県別のデータを参照し、ボタンにイメージを配置
		for (int i = 0; i < BUTTON_MAX; i++)
		{
			UserData userData = GameData.UserData;
			string str = "Textures/" + string.Format("{0:00}", GameData.UserData.RegionId) + "_" + string.Format("{0:00}", GameData.UserData.PrefecturesId) + "_" + string.Format("{0:00}", (i+1));
			Debug.Log(str);
			Sprite sp = Resources.Load(str, typeof(Sprite)) as Sprite;
			if (sp == null) break;
			buttonObj[i].SetActive(true);
			buttonObj[i].GetComponent<Image>().sprite = sp;
		}
	}
	//public void bt_PushCharButton(int num)
	//{
	//    now_battle_chardate = DataBaseScript.GetCharData(PrefectureNumber, num);
	//    char_num = num;
	//    //SceneManager.LoadScene("game"); // 渡邊変更
	//}


	////////////////////////////変更（ヨハ）/////////////////////////

	/// <summary>
	/// 使うキャラのイメージを入れる
	/// </summary>
	/// <param name="num">キャラボタンの番号</param>
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
        GameData.UserData.CharacterTexName = buttonObj[push_button_num].GetComponent<Image>().sprite.name;
        SaveData.SetString(SaveKey.UserCharacter, GameData.UserData.CharacterTexName);
        //SaveData.SetClass(SaveKey.UserCharacter, GameData.UserData);
        SaveData.Save();
		GameData.UserData.CharacterId = push_button_num+1;
		GameData.UserData.CharacterTexName = string.Format("{0:00}", GameData.UserData.RegionId) + "_" + string.Format("{0:00}", GameData.UserData.PrefecturesId) + "_" + string.Format("{0:00}", GameData.UserData.CharacterId);


		DataBaseScript.save_data_all.playerdata.use_char = buttonObj[push_button_num].GetComponent<Image>().sprite;
        DataBaseScript.Save();

        SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
    }


	/// <summary>
	/// キャラをゲットした時の処理
	/// </summary>
	/// <param name="num"></param>
    public void bt_GetChar(int num)
    {
        DataBaseScript.GetChar(PrefectureNumber, num);
    }

	/// <summary>
	/// 確認したい文字列を画面に表示
	/// </summary>
	/// <param name="str">表示したい文字</param>
    public void check(string str)
    {
        txt.text += str + "\n";
    }


    public void LoadAreaSelectScene()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_SelectArea, GameData.FadeSpeed);
        //SceneManager.LoadScene("select_area");
    }

    public void LoadScean_Picture_Book()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_PictureBook, GameData.FadeSpeed);
        //SceneManager.LoadScene("picture_book");
    }
}
