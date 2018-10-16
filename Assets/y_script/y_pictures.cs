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
    const int MAX_BUTTOM_NUM = 24;
    GameObject[] button = new GameObject[MAX_BUTTOM_NUM];

    List<date> chardate = new List<date>();

    //スプライトレンダーの固定値
    const float render_sizeX = 2f;
    const float render_sizeY = 2.5f;

    int now_number;
    int page_num;
    
	// Use this for initialization
	void Start () {
        init();
        LoadPicture();
        Description.SetActive(false);
        Debug.Log(chardate.Count);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void init() {
        page_num = 0;
        DateScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        picture = GameObject.Find("Description/picture");
        text_name = GameObject.Find("Description/Canvas/Text_name").GetComponent<Text>();

        string bt_name = "bt_char_";
        for (int i = 0; i < MAX_BUTTOM_NUM; i++)
        {
            bt_name += i;
            button[i] = transform.Find(bt_name).gameObject;
            button[i].GetComponent<Image>().preserveAspect = true;
            bt_name = "bt_char_";
        }
    }

    //最初に図鑑の画面に配置する画像をロード
    void LoadPicture()
    {
        int count = 0 ;
        RefreshButton();
        for (int i = 0; i < 47; i++)
        {
            List<date> list = DateScript.GetPrefectureDate(i);
            if (list == null) continue;
            for (int f = 0; f < list.Count; f++)
            {
                if (!list[f].getflg) continue;
                AddCharDate(list[f]);
                if (count == MAX_BUTTOM_NUM) continue;
                button[count].SetActive(true);
                button[count].GetComponent<Image>().sprite = list[f].img;
                count++;
            }
        }
    }

    //キャラを図鑑に表示させる（ページめくったときのみの関数）
    void ShowChar()
    {
        int count = 0;
        RefreshButton();
        for (int f = page_num * MAX_BUTTOM_NUM; f < chardate.Count; f++)
        {
            if (count == MAX_BUTTOM_NUM) break;
            button[count].SetActive(true);
            button[count].GetComponent<Image>().sprite = chardate[f].img;
            count++;
        }
    }

    void AddCharDate(date dt)
    {
        chardate.Add(dt);
        Debug.Log(chardate.Count);
    }


    void RefreshButton()
    {
        for (int i = 0; i < MAX_BUTTOM_NUM; i++)
        {
            button[i].SetActive(false);
        }
    }

    //キャラのボタンを押したら説明を出す
    public void bt_char(int num)
    {
        num += MAX_BUTTOM_NUM * page_num;
        if (chardate[num] == null)
        {
            Debug.Log("何も入ってない");
            return;
        }
        now_number = num;
        Debug.Log("押したとき" + chardate.Count);
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
        now_number += AddNum;
        //今の見ている説明の番号が0を下回った場合の処理
        if (now_number < 0) now_number += chardate.Count;

        //番号が最大値を超えないようにするためにやってる
        else
        {
            now_number = now_number % chardate.Count;
        }

        ShowDescription(now_number);
    }

    //キャラ選択画面でページをめくった場合
    public void bt_page_char(int AddNum)
    {
        page_num += AddNum;
        if (page_num < 0) page_num = 0;
        else if (MAX_BUTTOM_NUM * page_num > chardate.Count)
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

    //////////////////////ここから詳細情報出力用処理 ///////////////////////

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
        text_profile.text += "\n" + chardate[num].description;
        Debug.Log("出力中" + chardate.Count);
    }
}