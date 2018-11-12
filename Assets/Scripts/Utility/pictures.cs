using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pictures : MonoBehaviour
{
    Database dataScript;
    //説明で使うオブジェクトの親
    public GameObject description;

    //説明に乗るキャラの写真
    GameObject picture;

    Text text_name;
    public Text text_profile;

    //ボタンによってのデータを格納する配列
    const int MAX_BUTTON_AREA = 4;
    const int MAX_BUTTON_PARENT = 7;
    const int MAX_BUTTON_ROW = 4;
    static public int MAX_BUTTON_NUM = MAX_BUTTON_PARENT * MAX_BUTTON_ROW;
    const string IMG_OBJ_NAME = "Image";
    const float DESCRIPTION_PANEL_FIRSTPOS = -0.19f;

    //ボタンを格納するやつ
    GameObject[] button = new GameObject[MAX_BUTTON_NUM];
    GameObject[] image = new GameObject[MAX_BUTTON_NUM];
    GameObject[] bt_area_obj = new GameObject[MAX_BUTTON_AREA];

    //説明文が貼ってあるパネルとテキスト
    public GameObject scroll_panel_text;
    public GameObject scroll_panel;

    List<data> chardata = new List<data>();

    //スプライトレンダーの固定値
    const float render_sizeX = 2f;
    const float render_sizeY = 2.5f;

    //一列に入っているボタンの数
    const int ROW_BUTTON_VOLUM = 4;

    //キャラにマスクをかける用のパレット
    Color color = new Color(0, 0, 0);
    Color clear_color = new Color(255, 255, 255);

    bool scroll_stop_flg;

    int now_number;
    int page_num;

    int now_load_num;
    public GameObject content;

    Vector2 first_content_pos;

    // Use this for initialization
    void Start()
    {
        //once_push = false;
        init();
        dataScript = GameObject.Find("Managers/Master").GetComponent<Database>();
        scroll_stop_flg = false;
        LoadPicture(0);
        description.SetActive(false);
    }

    // Updata is called once per frame
    void Update()
    {
        CheckScrollStop();
    }

    void init()
    {
        first_content_pos = content.GetComponent<RectTransform>().offsetMax;

        page_num = 0;
        picture = GameObject.Find("Description/picture");
        text_name = GameObject.Find("Description/Canvas/Text_name").GetComponent<Text>();

        string bt_name;
        int count = 0;
        for (int i = 0; i < MAX_BUTTON_PARENT; i++)
        {
            bt_name = scroll_node.PARENT_COMMON_NAME;
            bt_name += i;
            GameObject bt_parent = GameObject.Find(bt_name);
            foreach (Transform child in bt_parent.transform)
            {
                button[count] = child.gameObject;
                image[count] = button[count].transform.Find(IMG_OBJ_NAME).gameObject;
                image[count].GetComponent<Image>().preserveAspect = true;
                count++;
            }
        }

        for (int i = 0; i < MAX_BUTTON_AREA; i++)
        {
            int tmp = i + 1;
            string tmp_str = "Canvas/bt_area" + tmp.ToString();
            bt_area_obj[i] = GameObject.Find(tmp_str);
        }
    }

    ///////////////画像読み込み関係///////////////

    //エリア選択した後に図鑑の画面に配置する画像をロード
    public void LoadPicture(int areanum)
    {
        string area = "area" + areanum.ToString();

        //ノードとコンテントの場所を初期化
        content.GetComponent<RectTransform>().offsetMax = first_content_pos;

        bottom.scroll_flg = true;
        chardata.Clear();
        int count = 0;
        RefreshButtonAll();

        ParentPosInit();

        //エリアを参照し、画像を抽出・表示
        for (int i = 0; i < Database.PrefectureDataSize; i++)
        {
            List<data> list = Database.Prefecturedata[i];
            if (list == null) continue;
            for (int f = 0; f < list.Count; f++)
            {
                if (list[f].area != area) continue;
                AddChardata(list[f]);
                if (count == MAX_BUTTON_NUM) continue;
                button[count].SetActive(true);
                image[count].GetComponent<Image>().sprite = list[f].img;
                if (!list[f].getflg)
                {
                    image[count].GetComponent<Image>().color = color;
                }
                else
                {
                    image[count].GetComponent<Image>().color = clear_color;
                }
                count++;
            }
        }

		////////////テスト用（キャラの名前の表示とかはどーするか聞く）/////////
		int PrefectureNumber = 1;
		for(int i = 0; i < areanum; i++)
		{
			PrefectureNumber += Database.AreaLenth[i];
		}

		int chara_num_count = 0;
		while (chara_num_count != 1)
		{
			Sprite sp = null;
			do
			{
				string str = "Textures/" + string.Format("{0:00}", area) + "_" + string.Format("{0:00}", PrefectureNumber) + "_" + string.Format("{0:00}", chara_num_count);
				sp = Resources.Load(str,typeof(Sprite)) as Sprite;
			} while (sp == null);
			
		}
		

        //キャラの数をみてスクロールをするかを決める
        if (chardata.Count < MAX_BUTTON_NUM)
        {
            bottom.scroll_flg = false;
            content.GetComponent<RectTransform>().offsetMin = new Vector2(0, bottom.BOTTOM);
        }
        now_load_num = MAX_BUTTON_NUM;
    }

    //スクロール中の画像読み込み
    public void LoadPoctureScroll(int parent_num, int direction)
    {
        int load_num;

        //上方向にスワイプしたら
        if (direction == scroll_node.UP) load_num = now_load_num;
        //下方向にスワイプしたら
        else
        {
            load_num = now_load_num - (MAX_BUTTON_PARENT + 1) * ROW_BUTTON_VOLUM;
        }

        //親のオブジェクトの番号
        RefreshButton(parent_num);
        parent_num = parent_num * ROW_BUTTON_VOLUM;
        for (int i = 0; i < ROW_BUTTON_VOLUM; i++)
        {
            now_load_num += direction;
            if (load_num >= chardata.Count || load_num < 0)
            {
                bottom.scroll_flg = false;
                continue;
            }
            button[parent_num + i].SetActive(true);
            if (!chardata[load_num].getflg)
            {
                image[parent_num + i].GetComponent<Image>().color = color;
            }
            else
            {
                image[parent_num + i].GetComponent<Image>().color = clear_color;
            }
            bottom.scroll_flg = true;
            image[parent_num + i].GetComponent<Image>().sprite = chardata[load_num].img;
            load_num++;
        }
    }

    ////キャラを図鑑に表示させる（ページめくったときのみの関数）
    //void ShowChar()
    //{
    //    int count = 0;
    //    RefreshButtonAll();
    //    for (int f = page_num * MAX_BUTTON_NUM; f < chardata.Count; f++)
    //    {
    //        if (count == MAX_BUTTON_NUM) break;
    //        button[count].SetActive(true);
    //        button[count].GetComponent<Image>().sprite = chardata[f].img;
    //        count++;
    //    }
    //}

    void AddChardata(data dt)
    {
        chardata.Add(dt);
    }

    /////////////////初期化関係/////////////////

    void ParentPosInit()
    {
        for (int i = 0; i < MAX_BUTTON_PARENT; i++)
        {
            string name = scroll_node.PARENT_COMMON_NAME + i;
            scroll_node sc = GameObject.Find(name).GetComponent<scroll_node>();
            sc.InitPos();
        }
    }


    void RefreshButtonAll()
    {
        for (int i = 0; i < MAX_BUTTON_PARENT; i++)
        {
            RefreshButton(i);
        }
    }

    void RefreshButton(int parent_num)
    {
        if (parent_num < 0) return;
        int first_num = parent_num * MAX_BUTTON_ROW;
        for (int i = first_num; i < first_num + MAX_BUTTON_ROW; i++)
        {
            GameObject parent = button[i].transform.parent.gameObject;
            parent.GetComponent<scroll_node>().count = 0;
            button[i].SetActive(false);
        }
    }

    //////////////////ボタンの処理//////////////////

    //エリア選択ボタンを押したとき
    public void bt_area(int areanum)
    {
        ParentPosInit();
        LoadPicture(areanum);
    }

    //キャラのボタンを押したら説明を出す
    public void bt_char(ButtonInfo Info)
    {
        int num = Info.GetButtonNumber();
        if (!chardata[num].getflg || chardata[num] == null)
        {
            return;
        }
        now_number = num;
        ShowDescription(num);
    }

    public void bt_close()
    {
        Description.active_flg = false;
        ChangeInteractable(true);
    }

    //スクロールを止めるかどうかを判断
    void CheckScrollStop()
    {
        if (scroll_stop_flg)
        {
            if (Input.GetMouseButton(0))
            {
                scroll_panel.GetComponent<ScrollRect>().inertia = true;
                scroll_stop_flg = false;
            }
        }
    }


    //説明文のスクロールポジションの初期化
    public void RefreshScollPos()
    {
        scroll_stop_flg = true;
        scroll_panel.GetComponent<ScrollRect>().inertia = false;
        Vector3 pos = scroll_panel_text.GetComponent<RectTransform>().position;
        pos.y = DESCRIPTION_PANEL_FIRSTPOS;
        scroll_panel_text.GetComponent<RectTransform>().position = pos;
    }

    // 説明画面でページをめくった処理
    public void bt_page(int AddNum)
    {
        RefreshScollPos();
        int savecount = 0;
        do
        {
            now_number += AddNum;

            //今の見ている説明の番号が0を下回った場合の処理
            if (now_number < 0) now_number += chardata.Count;

            //番号が最大値を超えないようにするためにやってる
            else
            {
                now_number = now_number % chardata.Count;
            }

            savecount++;


        } while (!chardata[now_number].getflg && savecount < chardata.Count);


        ShowDescription(now_number);
    }

    ////キャラ選択画面でページをめくった場合
    //public void bt_page_char(int AddNum)
    //{
    //    page_num += AddNum;
    //    if (page_num < 0) page_num = 0;
    //    else if (MAX_BUTTON_NUM * page_num > chardata.Count)
    //    {
    //        page_num -= AddNum;
    //    }
    //    ShowChar();
    //}

    //セレクトシーンをロード
    public void loadselect()
    {
        SceneManager.LoadScene(GameData.Scene_Select); // 渡邊変更
        //SceneManager.LoadScene("select");
    }


    //セーブデータ全消し
    public void bt_DataDeleteAll()
    {
        dataScript.ResetGetChar();
        LoadPicture(0);
    }

    //////////////////////ここから詳細情報(キャラ説明)出力用処理 ///////////////////////

    //アスペクト比からscaleを変える
    void SetAspect(int num)
    {
        SpriteRenderer sr = picture.GetComponent<SpriteRenderer>();
        Vector2 img_size;
        img_size = chardata[num].img.bounds.size;

        //画像によりloaclscaleを調整
        Vector2 aspect = new Vector2(img_size.x / render_sizeX, img_size.y / render_sizeY);


        //大きい方のアスペクト比をみてloaclscaleを採寸
        float scale;
        if (aspect.x > aspect.y)
        {
            scale = render_sizeX / img_size.x;
            sr.transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            scale = render_sizeY / img_size.y;
            sr.transform.localScale = new Vector2(scale, scale);
        }

        sr.sprite = chardata[num].img;
    }

    //説明画面を出す
    void ShowDescription(int num)
    {
        ChangeInteractable(false);
        description.SetActive(true);
        SetAspect(num);

        text_name.text = "名前　" + chardata[num].name;
        text_name.text += "\n出身地　" + chardata[num].place_name;
        text_profile.text = chardata[num].description;
    }

    ///////////////ボタンを押せなくする//////////////////
    void ChangeInteractable(bool flg)
    {
        for (int i = 0; i < MAX_BUTTON_NUM; i++)
        {
            button[i].GetComponent<Button>().interactable = flg;
        }
        foreach (GameObject button in bt_area_obj)
        {
            button.GetComponent<Button>().interactable = flg;
        }
    }

    //使うキャラのイメージを入れる
    public void SetPlayerChar()
    {
        // 渡邊変更
        GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
        msgBox.GetComponent<MessageBoxManager>().Initialize_YesNo("このキャラクターで\nいいですか？", SaveUserChara, null);

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
        GameData.UserData.CharacterTexName = picture.GetComponent<SpriteRenderer>().sprite.name;
        SaveData.SetString(SaveKey.UserCharacter, GameData.UserData.CharacterTexName);//変更（ヨハ）
        //SaveData.SetClass(SaveKey.UserCharacter, GameData.UserData);
        SaveData.Save();

        dataScript.save_data_all.playerdata.use_char = picture.GetComponent<SpriteRenderer>().sprite;
        dataScript.Save();

        SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
    }
}