using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class date
{
    public Sprite img;//画像
    public string place_name;//場所
    public string description;//説明文章
    public string name;//名前
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

    int now_prefecture_num;

    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

    const string SaveKey = "UserID";

    [SerializeField]
    public List<int> get_date = new List<int>();

    date a = new date
    {
        getflg = false,
        img = null,//画像
        place_name = "tt",//場所
        description = "ss",//説
        name = "aa",
    };

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
        DeleteAll();
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
            if (PrefectureName == "")
            {
                PrefectureName = flie_parent;
                now_prefecture_num = SearchNumer(PrefectureName);
            }

            //違う都道府県のフォルダに移ったら
            else if (PrefectureName != flie_parent)
            {
                now_prefecture_num = SearchNumer(PrefectureName);
                PrefectureDate[now_prefecture_num] = DateBase;
                PrefectureName = flie_parent;
                DateBase = new List<date>();
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
                dt.Setgetflg(false);
                DateBase.Add(dt);
                dt.Setplace_name(flie_parent);
                dt = new date();
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        PrefectureDate[SearchNumer(PrefectureName)] = DateBase;

        //セーブデータをロードする
        Load();
    }

    public List<date> GetPrefectureDate(int num)
    {
        return PrefectureDate[num];
    }

    //キャラのゲット処理
    public void GetChar(int PreNumber,int number)
    {
        List<date> list = new List<date>();
        list = PrefectureDate[PreNumber];
        if (number > list.Count - 1) return;
        date dt = new date();
        dt = list[number];
        if (!dt.Getgetflg())
        {
            get_date.Add(PreNumber);
            get_date.Add(number);
            Save();
            dt.Setgetflg(true);
        }
        list[number] = dt;
        list[number].Setgetflg(true);
        PrefectureDate[PreNumber] = list;
    }

    //セーブする
    void Save() {
        string str = Serialize<List<int>>(get_date);
        Debug.Log(str);
        PlayerPrefs.SetString(SaveKey,str);
    }

    //ロードする
    void Load()
    {
        get_date = GetSaveDate();
        List<int> savedate = get_date;
        if (savedate == null)
        {
            Debug.Log("セーブデータはないよ");
            return;
        }

        Debug.Log("セーブデータの数は"+savedate.Count);
        for (int i = 0; i < savedate.Count; i+=2)
        {
            List<date> list = PrefectureDate[savedate[i]];
            list[savedate[i + 1]].Setgetflg(true);
            PrefectureDate[savedate[i]] = list;
        }
    }

    public static List<int> GetSaveDate()
    {
        var str = PlayerPrefs.GetString(SaveKey);
        if (str == null) return null;
        List<int> savedate = Deserialize<List<int>>(str);
        return savedate;
    }

    public void DeleteAll()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
        }
    }


    private static string Serialize<T>(T obj)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    private static T Deserialize<T>(string str)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}