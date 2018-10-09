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

enum Prefecture { 
    Nagasaki,
    Saga,
    Hukuoka,
    Ooita,
    Kumamoto,
    Miyazaki,
    Kagoshima
}


public class y_Datebase : MonoBehaviour {
    string[] Prefecture_names = { "nagasaki", "saga", "hukuoka", "ooita", "kumamoto", "miyazaki","kagoshima" };

    //出身県ごとにデータを分ける
    List<date>[] PrefectureDate = new List<date>[47];
    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<date> DateBase = new List<date>();
    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

	// Use this for initialization
	void Start () {
        LoadDate();
	}
	
	// Update is called once per frame
	void Update () {

	}

    //入れる配列の番号を検索
    int SearchNumer(string flie_parent){
        for (int i = 0; i < Prefecture_names.Length; i++)
        {
            if (flie_parent == Prefecture_names[i]) {
                return i;
            }
        }
        return -1;
    }

    //データベース作成
    void LoadDate() {
        string PrefectureName = "" ;

        string str = Application.dataPath + "/Resources/DateChar";
        string remove_str = Application.dataPath + "/Resources/";
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        date dt = new date();
        int count = 0 ;
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;

            //Resourseからの相対パスにするためいらない文字列は消す
            files[i] = files[i].Replace(remove_str, "");

            //出身地を入れる(出身地が変わった場合は次に都道府県の配列に入れる)
            string flie_parent = System.IO.Path.GetDirectoryName(files[i]);
            flie_parent = System.IO.Path.GetDirectoryName(flie_parent);
            flie_parent = System.IO.Path.GetFileNameWithoutExtension(flie_parent);
            if (PrefectureName == "") PrefectureName = flie_parent;
                //違う都道府県のフォルダに移ったら
            else if (PrefectureName != flie_parent) {
                PrefectureDate[SearchNumer(PrefectureName)] = DateBase;
                dt.Setplace_name(flie_parent);
                DateBase.Clear();
            }

            //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == ".jpg"){
                files[i] = files[i].Replace(remove_str, "");
                files[i] = files[i].Replace(".jpg", "");
                dt.Setimg(Resources.Load<Sprite>(files[i]));
                count++;
            }

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == ".txt")
            {
                dt.Setdescription("説明書くよー");
                count++;
             
            }
            //ここ直す
            if (count % MAX_ITEM == 0 && count != 0)
            {
                DateBase.Add(dt);
            }
        }
    }

    public date Getdate(int num) {
        return DateBase[num];
    }
}
