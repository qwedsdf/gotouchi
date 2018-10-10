using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public struct date
{
    Sprite img;//画像
    string place_name;//場所
    string description;//説明文章
    string name;//名前
    public bool getflg;//手に入れたかどうか

    public void Setimg(Sprite limg) { this.img = limg; }
    public void Setplace_name(string lplace_name) { this.place_name = lplace_name; }
    public void Setdescription(string ldescription) { this.description = ldescription; }
    public void Setname(string lname) { this.name = lname; }
    public void Setgetflg(bool lgetflg) { this.getflg = lgetflg; }

    public Sprite Getimg() { return this.img; }
    public string Getplace_name() { return this.place_name; }
    public string Getdescription() { return this.description; }
    public string Getname() { return this.name; }
    public bool Getgetflg() { return this.getflg; }
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
    //キャラのデータ
    

    string[] Prefecture_names = { "nagasaki", "saga", "hukuoka", "ooita", "kumamoto", "miyazaki","kagoshima" };

    //出身県ごとにデータを分ける
    List<date>[] PrefectureDate = new List<date>[47];
    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<date> DateBase = new List<date>();
    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

	// Use this for initialization
	void Start () {
        int[] a = new int[50];
        LoadDate();
        List<date> list = PrefectureDate[(int)Prefecture.Kagoshima];
        Debug.Log(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].Getimg().name);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    //入れる配列の番号を検索
    int SearchNumer(string flie_parent){
        Debug.Log(flie_parent);
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
        date[] dt = new date[100];
        dt[0].Setgetflg(false);
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
                List<date> list = PrefectureDate[2];
                Debug.Log("キャラ名"+list[0].Getimg().name);
                PrefectureName = flie_parent;
                DateBase.Clear();
            }

            //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == ".jpg"){
                files[i] = files[i].Replace(remove_str, "");
                files[i] = files[i].Replace(".jpg", "");
                //画像の名前をキャラの名前にする
                dt[count / MAX_ITEM].Setname(System.IO.Path.GetFileNameWithoutExtension(files[i]));
                dt[count / MAX_ITEM].Setimg(Resources.Load<Sprite>(files[i]));
                count++;
            }

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == ".txt")
            {
                dt[count / MAX_ITEM].Setdescription("説明書くよー");
                count++;
             
            }
            //情報がすべて揃ったらデータベースに入れる
            if (count % MAX_ITEM == 0 && count != 0)
            {
                DateBase.Add(dt[(count - 1) / MAX_ITEM]);
                dt[count / MAX_ITEM].Setplace_name(flie_parent);
                dt[count / MAX_ITEM].Setgetflg(false);

                
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        PrefectureDate[SearchNumer(PrefectureName)] = DateBase;
    }

    public List<date> GetPrefectureDate(int num)
    {
        return PrefectureDate[num];
    }

    public void GetChar(int PreNumber,int number)
    {
        List<date> list = PrefectureDate[PreNumber];
        date dt = list[number];
        dt.Setgetflg(true);
        list[number] = dt;
        list[number].Setgetflg(true);
        Debug.Log(list[number].Getgetflg());
        PrefectureDate[PreNumber] = list;
    }

    public void bt_DebugFlgCheak() {
        Debug.Log("ookisa "+PrefectureDate.Length);
        for (int i = 0; i < PrefectureDate.Length; i++)
        {
            List<date> list = PrefectureDate[i];
            for (int f = 0; f < list.Count; f++)
            {
                Debug.Log(i+" "+list[f].Getgetflg());
            }
        }
    }
}
