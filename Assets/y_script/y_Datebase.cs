using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class date
{
    public Sprite img;//画像
    public string place_name;//場所
    public string area;//所属地方
    public string description;//説明文章
    public string name;//名前
    public bool getflg;//手に入れたかどうか
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
    public static string[] Prefecture_names = { "長崎", "佐賀", "福岡", "大分", "熊本", "宮崎", "鹿児島", "沖縄",
                                "鳥取","島根","岡山","広島","山口","香川","愛媛","高知" };
    public static int PrefectureDateSize = 47;

    //出身県ごとにデータを分ける
    List<date>[] PrefectureDate = new List<date>[PrefectureDateSize];

    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<date> DateBase = new List<date>();

    int now_prefecture_num;

    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

    const string SaveKey = "UserID";

    static private GameObject obj = null;

    const string TEXT_EXTENSION = ".txt";
    const string IMG_EXTENSION = ".png";

    [SerializeField]
    private List<int> get_date = new List<int>();

    void Awake()
    {
        if (obj != null)
        {
            Destroy(this.gameObject);
            return;
        }
        obj = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start () {
        //chackpath();
        //LoadDateAndroid();
        LoadDate();
	}
	
	// Update is called once per frame
	void Update () {
        DataDeleteAll();
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

    void chackpath()
    {
        string str = Application.streamingAssetsPath;
        this.GetComponent<y_buttom>().chack("パスは" + str);
        this.GetComponent<y_buttom>().chack("aa");
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        this.GetComponent<y_buttom>().chack("大きさは" + files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                this.GetComponent<y_buttom>().chack("パスは" + files[i]);
            }
        }
    }

    //アンドロイド用データに改変中
    void LoadDateAndroid()
    {
        string PrefectureName = "";
        string str = Application.streamingAssetsPath;
        this.GetComponent<y_buttom>().chack("パスは" + str);
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        //for (int i = 0; i < files.Length; i++)
        //{
        //    if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;

        //    //出身地を入れる(出身地が変わった場合は次に都道府県の配列に入れる)
        //    string flie_parent = System.IO.Path.GetDirectoryName(files[i]);
        //    flie_parent = System.IO.Path.GetDirectoryName(flie_parent);
        //    flie_parent = System.IO.Path.GetFileNameWithoutExtension(flie_parent);
        //    if (PrefectureName == "")
        //    {
        //        PrefectureName = flie_parent;
        //        now_prefecture_num = SearchNumer(PrefectureName);
        //    }

        //    //違う都道府県のフォルダに移ったら
        //    else if (PrefectureName != flie_parent)
        //    {
        //        now_prefecture_num = SearchNumer(PrefectureName);
        //        PrefectureDate[now_prefecture_num] = DateBase;
        //        PrefectureName = flie_parent;
        //        DateBase = new List<date>();
        //    }

        //    //画像読み込み
        //    if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
        //    {
        //        files[i] = ConvertPath(files[i], remove_str, IMG_EXTENSION);
        //        //画像の名前をキャラの名前にする
        //        dt.Setname(System.IO.Path.GetFileNameWithoutExtension(files[i]));
        //        dt.Setimg(Resources.Load<Sprite>(files[i]));
        //        count++;
        //    }

        //    //説明書読み込み
        //    else if (System.IO.Path.GetExtension(files[i]) == TEXT_EXTENSION)
        //    {
        //        files[i] = ConvertPath(files[i], remove_str, TEXT_EXTENSION);
        //        TextAsset textdate;
        //        textdate = Resources.Load<TextAsset>(files[i]);
        //        dt.Setdescription(textdate.text + count);
        //        count++;
             
        //    }
        //    //情報がすべて揃ったらデータベースに入れる
        //    if (count % MAX_ITEM == 0 && count != 0)
        //    {
        //        dt.Setgetflg(false);
        //        DateBase.Add(dt);
        //        dt.Setplace_name(flie_parent);
        //        dt = new date();
        //    }
        //}
        //最後の分（今は佐賀）のデータをデータベースに入れる
        //PrefectureDate[SearchNumer(PrefectureName)] = DateBase;

        ////セーブデータをロードする
        //Load();

        //}
        WWW www = new WWW(str);
        while(!www.isDone){
            Debug.Log("ロードなう");
        }
        Debug.Log("ロード終わり");
        this.GetComponent<y_buttom>().chack(www.text);

    }

    //データベース作成
    void LoadDate() {
        string PrefectureName = "" ;

        string str = Application.dataPath + "/Resources/DateChar/";
        string remove_str = Application.dataPath + "/Resources/";
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        date dt = new date();

        int count = 0 ;
        
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;

            //Resourseからの相対パスにするためいらない文字列は消す
            files[i] = files[i].Replace(remove_str, "");

            //出身地、エリアを入れる(出身地が変わった場合は都道府県の配列に入れる)
            string flie_parent = System.IO.Path.GetDirectoryName(files[i]);
            flie_parent = System.IO.Path.GetDirectoryName(flie_parent);

            //所属エリアを入れる
            string area = System.IO.Path.GetDirectoryName(flie_parent);
            dt.area = System.IO.Path.GetFileNameWithoutExtension(area);

            //出身地を入れる
            flie_parent = System.IO.Path.GetFileNameWithoutExtension(flie_parent);
            Debug.Log(flie_parent);

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
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                files[i] = ConvertPath(files[i], remove_str, IMG_EXTENSION);
                Debug.Log(files[i]);
                //画像の名前をキャラの名前にする
                dt.name=System.IO.Path.GetFileNameWithoutExtension(files[i]);
                dt.img=Resources.Load<Sprite>(files[i]);
                //Debug.Log(dt.name+"　"+dt.img.bounds.size.x);
                count++;
            }

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == TEXT_EXTENSION)
            {
                files[i] = ConvertPath(files[i], remove_str, TEXT_EXTENSION);
                TextAsset textdate;
                textdate = Resources.Load<TextAsset>(files[i]);
                dt.description=textdate.text + count;
                if (textdate == null)
                {
                    Debug.Log(dt.name);
                }
                count++;
             
            }
            //情報がすべて揃ったらデータベースに入れる
            if (count % MAX_ITEM == 0 && count != 0)
            {
                dt.getflg=false;
                DateBase.Add(dt);
                dt.place_name=flie_parent;
                dt = new date();
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        PrefectureDate[SearchNumer(PrefectureName)] = DateBase;

        //セーブデータをロードする
        Load();
    }

    //Resources.Loadで使えるようにパスをコンバートする
    string ConvertPath(string path,string removecharacter,string extension)
    {
        path = path.Replace(removecharacter, "");
        path = path.Replace(extension, "");
        return path;
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
        if (!dt.getflg)
        {
            get_date.Add(PreNumber);
            get_date.Add(number);
            Save();
            dt.getflg=true;
        }
        list[number] = dt;
        list[number].getflg=true;
        PrefectureDate[PreNumber] = list;
    }

    //セーブする
    void Save() {
        string lstr = Serialize<List<int>>(get_date);
        PlayerPrefs.SetString(SaveKey,lstr);
    }

    //ロードする
    void Load()
    {
        get_date = GetSaveDate();
        if (get_date == null)
        {
            get_date = new List<int>();
            Debug.Log("セーブデータはないよ");
            return;
        }

        Debug.Log("セーブデータの数は" + get_date.Count);
        for (int i = 0; i < get_date.Count; i += 2)
        {
            List<date> list = PrefectureDate[get_date[i]];
            list[get_date[i + 1]].getflg=true;
            PrefectureDate[get_date[i]] = list;
        }
    }

    public static List<int> GetSaveDate()
    {
        var stri = PlayerPrefs.GetString(SaveKey);
        if (stri == "") return null;
        List<int> savedate = Deserialize<List<int>>(stri);
        return savedate;
    }

    public void DataDeleteAll()
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

    private static T Deserialize<T>(string lstr)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(lstr));
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}