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
    date[] bt_date = new date[MAX_BUTTOM_NUM];
    

	// Use this for initialization
	void Start () {
        DateScript = GameObject.Find("Master").GetComponent<y_Datebase>();
        picture = GameObject.Find("Description/picture");
        text = GameObject.Find("Description/Text").GetComponent<Text>();
        LoadPicture();
        Description.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    //図鑑の画面に配置する画像をロード
    void LoadPicture()
    {
        int count=0;
        string bt_name="bt_char_";
        for (int i = 0; i < 47; i++)
        {
            List<date> list = DateScript.GetPrefectureDate(i);
            if (list == null) continue;
            for (int f = 0; f < list.Count; f++)
            {
                if (!list[f].Getgetflg()) continue;
                bt_name += count;
                transform.Find(bt_name).GetComponent<Image>().sprite = list[f].Getimg();
                bt_date[count] = list[f];
                count++;
                bt_name = "bt_char_";
                if (count > 19) return;
            }
        }
    }

    //キャラのボタンを押したら説明を出す
    public void bt_char(int num) {
        if (bt_date[num].Getimg() == null)
        {
            Debug.Log("何も入ってないお");
            return;
        }
        Description.SetActive(true);
        picture.GetComponent<SpriteRenderer>().sprite = bt_date[num].Getimg();
        text.text = bt_date[num].Getdescription();

    }

    public void bt_close() {
        Description.SetActive(false);
    }
}
