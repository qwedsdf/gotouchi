using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_pictures : MonoBehaviour {

    y_Datebase DateScript;

    //説明で使うオブジェクトの親
    public GameObject Description;

    //説明に乗るキャラの写真
    GameObject picture;

    Text text_name;
    public Text text_profile;

    //ボタンによってのデータを格納する配列
    const int MAX_BUTTON_PARENT = 7;
    const int MAX_BUTTON_ROW = 4;
    const int MAX_BUTTON_NUM = MAX_BUTTON_PARENT * MAX_BUTTON_ROW;
    GameObject[] button = new GameObject[MAX_BUTTON_NUM];

    List<date> chardate = new List<date>();

    //スプライトレンダーの固定値
    const float render_sizeX = 2f;
    const float render_sizeY = 2.5f;

    //一列に入っているボタンの数
    const int ROW_BUTTON_VOLUM = 4;

    Color color = new Color(0, 0, 0);
    Color clear_color = new Color(255, 255, 255);

    int now_number;
    int page_num;

    int now_load_num;
    public GameObject content;

    Vector2 first_content_pos;
    bool once_push;
    
	// Use this for initialization
	void Start () {
        once_push = false;
        init();
        LoadPicture("");
        Description.SetActive(false);
        Debug.Log(chardate.Count);
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    void init() {
        first_content_pos = content.GetComponent<RectTransform>().offsetMax;
        Debug.Log("初期値"+first_content_pos.y);

        page_num = 0;
        DateScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        picture = GameObject.Find("Description/picture");
        text_name = GameObject.Find("Description/Canvas/Text_name").GetComponent<Text>();

        string bt_name;
        int count = 0;
        for (int i = 0; i < MAX_BUTTON_PARENT; i++)
        {
            bt_name = "bt_parent_";
            bt_name += i;
            GameObject bt_parent = GameObject.Find(bt_name);
            foreach(Transform child in bt_parent.transform){
                button[count] = child.gameObject;
                button[count].GetComponent<Image>().preserveAspect = true;
                count++;
            }
        }

        //for (int i = 0; i < MAX_BUTTON_NUM; i++)
        //{
        //    bt_name = "bt_char_";
        //    bt_name += i;
        //    button[i] = transform.Find(bt_name).gameObject;
        //    button[i].GetComponent<Image>().preserveAspect = true;
        //}
    }

    ///////////////画像読み込み関係///////////////

    //エリア選択した後に図鑑の画面に配置する画像をロード
    public void LoadPicture(string area)
    {
        //ノードとコンテントの場所を初期化

        content.GetComponent<RectTransform>().offsetMax = first_content_pos;

        y_bottom.scroll_flg = true;
        chardate.Clear();
        int count = 0 ;
        RefreshButtonAll();

        once_push = true;

        ParentPosInit();
        
        for (int i = 0; i < y_Datebase.PrefectureDateSize; i++)
        {
            List<date> list = DateScript.GetPrefectureDate(i);
            if (list == null) continue;
            for (int f = 0; f < list.Count; f++)
            {
                if (list[f].area != area) continue;
                AddCharDate(list[f]);
                if (count == MAX_BUTTON_NUM) continue;
                button[count].SetActive(true);
                button[count].GetComponent<Image>().sprite = list[f].img;
                if (!list[f].getflg) {
                    button[count].GetComponent<Image>().color = color;
                }
                else
                {
                    button[count].GetComponent<Image>().color = clear_color;
                }
                count++;
            }
        }
        if (chardate.Count < MAX_BUTTON_NUM)
        {
            Debug.Log("スクロールさせない");
            y_bottom.scroll_flg = false;
            content.GetComponent<RectTransform>().offsetMin = new Vector2(0, y_bottom.BOTTOM);
        }
        now_load_num = MAX_BUTTON_NUM;
    }

    //スクロール中の画像読み込み
    public void LoadPoctureScroll(int parent_num, int direction)
    {
        int load_num;
        
        //上方向にスワイプしたら
        if (direction == y_scroll_node.UP) load_num = now_load_num;
        //下方向にスワイプしたら
        else
        {
            load_num = now_load_num - (MAX_BUTTON_PARENT + 1) * ROW_BUTTON_VOLUM;
        }

        Debug.Log("ロード前の番号　" + now_load_num);
        
        //親のオブジェクトの番号
        RefreshButton(parent_num);
        parent_num = parent_num * ROW_BUTTON_VOLUM;
        for (int i = 0; i < ROW_BUTTON_VOLUM; i++)
        {
            now_load_num += direction;
            if (load_num >= chardate.Count || load_num < 0)
            {  
                y_bottom.scroll_flg = false;
                continue;
            }
            button[parent_num + i].SetActive(true);
            if (!chardate[load_num].getflg)
            {
                button[parent_num + i].GetComponent<Image>().color = color;
            }
            else
            {
                button[parent_num + i].GetComponent<Image>().color = clear_color;
            }
            y_bottom.scroll_flg = true;
            button[parent_num + i].GetComponent<Image>().sprite = chardate[load_num].img;
            load_num++;
        }

        Debug.Log("ロード後の番号　" + now_load_num);
    }

    //キャラを図鑑に表示させる（ページめくったときのみの関数）
    void ShowChar()
    {
        int count = 0;
        RefreshButtonAll();
        for (int f = page_num * MAX_BUTTON_NUM; f < chardate.Count; f++)
        {
            if (count == MAX_BUTTON_NUM) break;
            button[count].SetActive(true);
            button[count].GetComponent<Image>().sprite = chardate[f].img;
            count++;
        }
    }

    void AddCharDate(date dt)
    {
        chardate.Add(dt);
    }

    /////////////////初期化関係/////////////////

    void ParentPosInit()
    {
        for (int i = 0; i < MAX_BUTTON_PARENT; i++)
        {
            //ここ直す（初期化バグ）
            string name = y_scroll_node.PARENT_COMMON_NAME + i;
            y_scroll_node sc = GameObject.Find(name).GetComponent<y_scroll_node>();
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
            button[i].SetActive(false);
        }
    }

    //////////////////ボタンの処理//////////////////

    //エリア選択ボタンを押したとき
    public void bt_area(y_bt_area script)
    {
        ParentPosInit();
        LoadPicture(script.GetAreaName());
    }

    //キャラのボタンを押したら説明を出す
    public void bt_char(y_ButtonInfo Info)
    {
        int num = Info.GetButtonNumber();
        if (!chardate[num].getflg || chardate[num] == null)
        {
            return;
        }
        now_number = num;
        ShowDescription(num);
    }

    public void bt_close()
    {
        Description.SetActive(false);
    }

    // 説明画面でページをめくった処理
    public void bt_page(int AddNum)
    {
        Debug.Log(chardate.Count);
        
        int savecount = 0;
        do{
            now_number += AddNum;

            //今の見ている説明の番号が0を下回った場合の処理
            if (now_number < 0) now_number += chardate.Count;

            //番号が最大値を超えないようにするためにやってる
            else
            {
                now_number = now_number % chardate.Count;
            }

        savecount++;


        } while (!chardate[now_number].getflg && savecount < chardate.Count);
        

        ShowDescription(now_number);
    }

    //キャラ選択画面でページをめくった場合
    public void bt_page_char(int AddNum)
    {
        page_num += AddNum;
        if (page_num < 0) page_num = 0;
        else if (MAX_BUTTON_NUM * page_num > chardate.Count)
        {
            page_num -= AddNum;
        }
        ShowChar();
    }

    //セレクトシーンをロード
    public void loadselect()
    {
        SceneManager.LoadScene("select");
    }

    //////////////////////ここから詳細情報(キャラ説明)出力用処理 ///////////////////////

    //アスペクト比からscaleを変える
    void SetAspect(int num)
    {
        SpriteRenderer sr = picture.GetComponent<SpriteRenderer>();
        Vector2 img_size;
        img_size = chardate[num].img.bounds.size;

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

        sr.sprite = chardate[num].img;
    }

    //説明画面を出す
    void ShowDescription(int num)
    {
        Description.SetActive(true);
        SetAspect(num);

        text_name.text = "名前　" + chardate[num].name;
        text_name.text += "\n出身地　" + chardate[num].place_name;
        text_profile.text = chardate[num].description;
    }
}