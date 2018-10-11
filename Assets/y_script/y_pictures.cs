using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_pictures : MonoBehaviour {

    y_Datebase DateScript;

    //説明で使うオブジェクトの親
    public GameObject Description;

    //説明に乗るキャラの写真
    GameObject picture;

    Text text;

    //ボタンによってのデータを格納する配列
    const int MAX_BUTTOM_NUM = 20;
    GameObject[] button = new GameObject[MAX_BUTTOM_NUM];

    List<date> chardate = new List<date>();

    int now_number;
    int page_num;
    

	// Use this for initialization
	void Start () {
        init();
        LoadPicture();
        Description.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void init() {
        page_num = 0;
        DateScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        picture = GameObject.Find("Description/picture");
        text = GameObject.Find("Description/Text").GetComponent<Text>();

        string bt_name = "bt_char_";
        for (int i = 0; i < MAX_BUTTOM_NUM; i++)
        {
            bt_name += i;
            button[i] = transform.Find(bt_name).gameObject;
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
                if (!list[f].Getgetflg()) continue;
                AddCharDate(list[f]);
                if (count > 19) continue;
                button[count].SetActive(true);
                button[count].GetComponent<Image>().sprite = list[f].Getimg();
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
            if (count > 19) continue;
            button[count].SetActive(true);
            button[count].GetComponent<Image>().sprite = chardate[f].Getimg();
            count++;
        }
    }

    void AddCharDate(date dt)
    {
        chardate.Add(dt);
    }

    //説明画面を出す
    void ShowDescription(int num)
    {
        Description.SetActive(true);
        picture.GetComponent<SpriteRenderer>().sprite = chardate[num].Getimg();
        text.text = "名前　" + chardate[num].Getname();
        text.text += "\n出身地　" + chardate[num].Getplace_name();
        text.text += "\n" + chardate[num].Getdescription();
    }

    //キャラのボタンを押したら説明を出す
    public void bt_char(int num) {
        num += MAX_BUTTOM_NUM * page_num;
        if (chardate[num] == null)
        {
            Debug.Log("何も入ってない");
            return;
        }
        now_number = num;
        ShowDescription(num);
    }

    public void bt_close() {
        Debug.Log("押されたよ");
        Description.SetActive(false);
    }

    void RefreshButton()
    {
        for (int i = 0; i < MAX_BUTTOM_NUM; i++)
        {
            button[i].SetActive(false);
        }
    }

    // 説明画面でページをめくった処理
    public void bt_page(int AddNum)
    {
        now_number += AddNum;
        //今の見ている説明の番号が0を下回った場合の処理
        if (now_number < 0) now_number += chardate.Count;

        //番号が最大値を超えないようにするためにやってる
        else now_number = now_number % chardate.Count;

        Debug.Log(now_number);
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
}
