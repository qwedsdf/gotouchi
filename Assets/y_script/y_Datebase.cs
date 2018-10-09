using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//キャラのデータ
public struct date
{
    Sprite img;//画像
    string place_name;//場所
    string description;//説明文章

    public void Setimg(Sprite limg){    img=limg;   }
    public void Setplace_name(string lplace_name) { place_name = lplace_name; }
    public void Setdescription(string ldescription) { description = ldescription; }

    public Sprite Getimg() { return img; }
    public string Getplace_name() { return place_name; }
    public string Getdescription() { return description; }
}


public class y_Datebase : MonoBehaviour {
    List<date> DateBase = new List<date>();

	// Use this for initialization
	void Start () {
        LoadDate();
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    //データベース作成
    void LoadDate() {
        string str = Application.dataPath + "/Resources/DateChar";
        string remove_str = Application.dataPath + "/Resources/";
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        date dt = new date();
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;

            files[i] = files[i].Replace(remove_str, "");

            //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == ".jpg"){
                string flie_parent = System.IO.Path.GetDirectoryName(files[i]);
                files[i] = files[i].Replace(remove_str, "");
                files[i] = files[i].Replace(".jpg", "");
                dt.Setimg(Resources.Load<Sprite>(files[i]));

               //出身地を入れる
                flie_parent = System.IO.Path.GetDirectoryName(flie_parent);
                dt.Setplace_name(System.IO.Path.GetFileNameWithoutExtension(flie_parent));

            }
            //一個上のファイル名を取得

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == ".txt")
            {
                dt.Setdescription("説明書くよー");
                DateBase.Add(dt);

            }
        }
        for(int i=0;i<DateBase.Count;i++){
            Debug.Log(DateBase[i].Getimg().name);
            Debug.Log(DateBase[i].Getplace_name());
            Debug.Log(DateBase[i].Getdescription());
        }
    }

    public date Getdate(int num) {
        return DateBase[num];
    }
}
