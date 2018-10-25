using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class data
{
    public Sprite img;//画像
    public string place_name;//場所
    public string area;//所属地方
    public string description;//説明文章
    public string name;//名前
    public bool getflg;//手に入れたかどうか
}

[Serializable]
public class Savedata
{
    public string area;//所属地方
    public string prefecture_name;//場所
    public int prefecture_num;//都道府県番号
    public string char_name;//名前
    public int char_num;//キャラの番号
}

[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> data;
    public List<T> ToList() { return data; }

    public Serialization(List<T> target)
    {
        data = target;
    }
}

enum Prefecture
{
    Nagasaki,
    Saga,
    Hukuoka,
    Ooita,
    Kumamoto,
    Miyazaki,
    Kagoshima
}

public class y_Database : MonoBehaviour {
    //キャラのデータ
    public static string[] Prefecture_names = { "長崎", "佐賀", "福岡", "大分", "熊本", "宮崎", "鹿児島", "沖縄",
                                "鳥取","島根","岡山","広島","山口","香川","愛媛","高知" };
    public static int PrefectureDataSize = 47;

    //出身県ごとにデータを分ける
    List<data>[] Prefecturedata = new List<data>[PrefectureDataSize];

    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<data> dataBase = new List<data>();

    int now_prefecture_num;

    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

    const string SaveKey = "UserID";

    static private GameObject obj = null;

    const string TEXT_EXTENSION = ".txt";
    const string IMG_EXTENSION = ".png";


    [SerializeField]
    List<Savedata> get_data;

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
        LoadDataAndroid();
        //LoadData();
	}
	
	// Updata is called once per frame
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
        //this.GetComponent<y_button>().chack("パスは" + str);
        //this.GetComponent<y_button>().chack("aa");
        GameObject.Find("EventSystem").GetComponent<y_button>().chack("うわああ");
        string[] files = System.IO.Directory.GetFiles(@str, "*.png", System.IO.SearchOption.AllDirectories);
        GameObject.Find("EventSystem").GetComponent<y_button>().chack("大きさは" + files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                this.GetComponent<y_button>().chack("パスは" + files[i]);
            }
        }
    }

    //アンドロイド用データに改変中
    void LoadDataAndroid()
    {
        string PrefectureName = "";
        string str = Application.dataPath;

        Sprite[] Spritedata = Resources.LoadAll<Sprite>("DataChar");
        TextAsset[] Textdata = Resources.LoadAll<TextAsset>("DataChar");

        string remove_str = Application.dataPath;
        int count = 0 ;
        Debug.Log(remove_str);
        //string[] resourcesDirectoryPaths = Directory.GetDirectories("Assets/Resources/DataChar", "*", SearchOption.AllDirectories);

        string[] files = System.IO.Directory.GetDirectories(Application.dataPath, "Resources", System.IO.SearchOption.AllDirectories);

        foreach (string path in files)
        {
            GameObject.Find("EventSystem").GetComponent<y_button>().chack(path);
        }

        string[] lfiles = System.IO.Directory.GetFiles(files[0], "ja-bou.png", System.IO.SearchOption.AllDirectories);
        foreach (string path in lfiles)
        {
            GameObject.Find("EventSystem").GetComponent<y_button>().chack(path);
        }
        

        data dt = new data();
        //GameObject.Find("EventSystem").GetComponent<y_button>().chack("データの量は" + Textdata.Length);
        return;
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;

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
                Prefecturedata[now_prefecture_num] = dataBase;
                PrefectureName = flie_parent;
                dataBase = new List<data>();
            }

           //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                
                files[i] = ConvertPath(files[i], remove_str, IMG_EXTENSION);
                //画像の名前をキャラの名前にする
                dt.name=System.IO.Path.GetFileNameWithoutExtension(files[i]);
                dt.img=Resources.Load<Sprite>(files[i]);
                count++;
            }

            //説明書読み込み
            else if (System.IO.Path.GetExtension(files[i]) == TEXT_EXTENSION)
            {
                files[i] = ConvertPath(files[i], remove_str, TEXT_EXTENSION);
                TextAsset textdata;
                textdata = Resources.Load<TextAsset>(files[i]);
                dt.description=textdata.text + count;
                if (textdata == null)
                {
                    Debug.Log(dt.name);
                }
                count++;
             
            }
            //情報がすべて揃ったらデータベースに入れる
            if (count % MAX_ITEM == 0 && count != 0)
            {
                dt.getflg = false;
                dataBase.Add(dt);
                dt.place_name = flie_parent;
                dt = new data();
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        Prefecturedata[SearchNumer(PrefectureName)] = dataBase;

        //セーブデータをロードする
        Load();
    }

    //データベース作成
    void LoadData() {
        string PrefectureName = "" ;

        string str = Application.dataPath + "/Resources/DataChar/";
        string remove_str = Application.dataPath + "/Resources/";
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        data dt = new data();
        int count = 0 ;
        
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;
            files[i] = files[i].Replace("\\", "/");

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

            if (PrefectureName == "")
            {
                PrefectureName = flie_parent;
                now_prefecture_num = SearchNumer(PrefectureName);
            }

            //違う都道府県のフォルダに移ったら
            else if (PrefectureName != flie_parent)
            {
                now_prefecture_num = SearchNumer(PrefectureName);
                Prefecturedata[now_prefecture_num] = dataBase;
                PrefectureName = flie_parent;
                dataBase = new List<data>();
            }

            //画像読み込み
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                files[i] = ConvertPath(files[i], remove_str, IMG_EXTENSION);
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
                TextAsset textdata;
                textdata = Resources.Load<TextAsset>(files[i]);
                dt.description=textdata.text + count;
                if (textdata == null)
                {
                    Debug.Log(dt.name);
                }
                count++;
             
            }
            //情報がすべて揃ったらデータベースに入れる
            if (count % MAX_ITEM == 0 && count != 0)
            {
                dt.getflg=false;
                dataBase.Add(dt);
                dt.place_name=flie_parent;
                dt = new data();
            }
        }
        //最後の分（今は佐賀）のデータをデータベースに入れる
        Prefecturedata[SearchNumer(PrefectureName)] = dataBase;

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

    public List<data> GetPrefectureData(int num)
    {
        return Prefecturedata[num];
    }

    //キャラのゲット処理
    public void GetChar(int PreNumber,int number)
    {
        List<data> list = new List<data>();
        list = Prefecturedata[PreNumber];
        if (number > list.Count - 1) return;
        data dt = new data();
        dt = list[number];
        if (!dt.getflg)
        {
            Savedata savedata = new Savedata();
            savedata.area = dt.area;
            savedata.prefecture_name = dt.place_name;
            savedata.prefecture_num = PreNumber;
            savedata.char_name = dt.name;
            savedata.char_num = number;
            get_data.Add(savedata);
            Save();
            dt.getflg=true;
        }
        list[number] = dt;
        list[number].getflg=true;
        Prefecturedata[PreNumber] = list;
    }

    //セーブする
    void Save() {
        string json = JsonUtility.ToJson(new Serialization<Savedata>(get_data),true);
        Debug.Log("json使ったやつ\n" + json);

        PlayerPrefs.SetString(SaveKey, json);
    }

    //ロードする
    void Load()
    {
        string lstr = PlayerPrefs.GetString(SaveKey);
        if (lstr == "")
        {
            get_data = new List<Savedata>();
            Debug.Log("セーブデータはないよ");
            return;
        }
        get_data = JsonUtility.FromJson<Serialization<Savedata>>(lstr).ToList();

        Debug.Log("セーブデータの数は" + get_data.Count);
        for (int i = 0; i < get_data.Count; i++)
        {
            List<data> list = Prefecturedata[get_data[i].prefecture_num];
            list[get_data[i].char_num].getflg = true;
            Prefecturedata[get_data[i].prefecture_num] = list;
        }
    }

    public void DataDeleteAll()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}