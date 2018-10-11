﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class date
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
    string[] Prefecture_names = { "長崎", "佐賀", "福岡", "大分", "熊本", "宮崎","鹿児島" };

    //出身県ごとにデータを分ける
    List<date>[] PrefectureDate = new List<date>[47];
    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<date> DateBase = new List<date>();
    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
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
        date dt = new date();
        dt.Setgetflg(false);

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
                PrefectureName = flie_parent;
                DateBase=new List<date>();
            }

            //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == ".png")
            {
                files[i] = files[i].Replace(remove_str, "");
                files[i] = files[i].Replace(".png", "");
                //画像の名前をキャラの名前にする
                dt.Setname(System.IO.Path.GetFileNameWithoutExtension(files[i]));
                dt.Setimg(Resources.Load<Sprite>(files[i]));
                count++;
            }

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == ".txt")
            {
                dt.Setdescription("説明書くよー" + count);
                count++;
             
            }
            //情報がすべて揃ったらデータベースに入れる
            if (count % MAX_ITEM == 0 && count != 0)
            {
                DateBase.Add(dt);
                dt.Setplace_name(flie_parent);
                dt.Setgetflg(false);
                dt = new date();
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        PrefectureDate[SearchNumer(PrefectureName)] = DateBase;
    }

    public List<date> GetPrefectureDate(int num)
    {
        return PrefectureDate[num];
    }

    //キャラのゲット処理
    public void GetChar(int PreNumber,int number)
    {
        List<date> list=new List<date>();
        list = PrefectureDate[PreNumber];
        if (number > list.Count - 1) return;
        date dt = new date();
        dt = list[number];
        dt.Setgetflg(true);
        list[number] = dt;
        list[number].Setgetflg(true);
        PrefectureDate[PreNumber] = list;
    }
}




