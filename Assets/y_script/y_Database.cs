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
    //デバッグ用の変数
    public GameObject test_img;

    //エリア判別用変数
    public static int area_num = 1;

    //キャラのデータ
    public static string[] Prefecture_names = { "長崎", "佐賀", "福岡", "大分", "熊本", "宮崎", "鹿児島", "沖縄",
                                "鳥取","島根","岡山","広島","山口","香川","愛媛","高知","高知","高知","高知","高知","高知","高知","高知","高知","高知","高知","高知" };

    public static int AREA_VOLUME = 8;
    const int VOLUME_KYUSH = 1;
    const int VOLUME_SHIKOKU = 2;
    const int VOLUME_HOKKAIDOU = 8;
    const int VOLUME_TOUHOKU = 8;
    const int VOLUME_KANTOU = 2;
    const int VOLUME_TYUBU = 3;
    const int VOLUME_KINKI = 1;
    const int VOLUME_TYUGOKU = 2;

    public static int PrefectureDataSize = 47;
    public static int MaxAreaLenth = 8;
    public static int[] AreaLenth = { VOLUME_HOKKAIDOU, VOLUME_TOUHOKU, VOLUME_KANTOU, VOLUME_TYUBU, VOLUME_KINKI, VOLUME_TYUGOKU, VOLUME_SHIKOKU, VOLUME_KYUSH };

    //出身県ごとにデータを分ける
    List<data>[] Prefecturedata = new List<data>[PrefectureDataSize];

    //このなかに1都道府県ごとにキャラのデータを入れる（１都道府県のデータを全て入れる）
    List<data> dataBase = new List<data>();

    int now_prefecture_num;

    //一キャラにつき必要な素材数
    const int MAX_ITEM = 2;

    const string SaveKey = "UserID";

    static private GameObject obj = null;

    const string DATE_FOLDER_NAME = "DataChar";
    const string AREA_FOLDER_NAME = "area";
    const string ANDROID_FOLDER_NAME = "Android";

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
        //checkpath();
        //LoadDataAndroid();
        //LoadData();
        BundleLoadData();
	}
	
	// Updata is called once per frame
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

    void checkpath()
    {
        string str = Application.streamingAssetsPath;
        //this.GetComponent<y_button>().check("パスは" + str);
        //this.GetComponent<y_button>().check("aa");
        GameObject.Find("EventSystem").GetComponent<y_button>().check("うわああ");
        string[] files = System.IO.Directory.GetFiles(@str, "*.png", System.IO.SearchOption.AllDirectories);
        GameObject.Find("EventSystem").GetComponent<y_button>().check("大きさは" + files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            if (System.IO.Path.GetExtension(files[i]) == IMG_EXTENSION)
            {
                this.GetComponent<y_button>().check("パスは" + files[i]);
            }
        }
    }

    //データベースを作成する
    void BundleLoadData()
    {
        //ロードできているか確認
        test_img.GetComponent<Image>().sprite = Resources.Load<Sprite>("ja-bou");

        int loadcount = 0;
        //エリアごとに分ける
        for (int i = 0; i < MaxAreaLenth; i++)
        {
            //都道府県ごと
            for (int f = 0; f < AreaLenth[i]; f++)
            {
                string number = loadcount.ToString();
                int tmp = i + 1;
                string path = ANDROID_FOLDER_NAME + "/" + DATE_FOLDER_NAME + "/" + AREA_FOLDER_NAME + tmp.ToString();
                if (loadcount < 10) number = "0" + number;
                path += "/" + number;

                //アセットバンドルからアセットをとってくる
                Sprite[] tmp_sprite;
                TextAsset[] tmp_text;
                string bundleUrl = Application.streamingAssetsPath + "/" + path;
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundleUrl);
                AssetBundle assetBundle = request.assetBundle;
                Debug.Log(f + assetBundle.name);

                tmp_sprite = assetBundle.LoadAllAssets<Sprite>();
                tmp_text = assetBundle.LoadAllAssets<TextAsset>();

                List<data> tmp_data = new List<data>();
                //キャラごと
                for (int k = 0; k < tmp_sprite.Length; k++)
                {
                    data dt = new data();
                    dt.area = AREA_FOLDER_NAME + i.ToString();
                    dt.getflg = false;
                    dt.place_name = Prefecture_names[loadcount];
                    dt.img = tmp_sprite[k];
                    dt.description = tmp_text[k].text;
                    dt.name = dt.img.name;

                    tmp_data.Add(dt);
                }
                Prefecturedata[loadcount] = tmp_data;
                //次の都道府県に移動
                loadcount++;
            }
        }
        //セーブデータをロードする
        Load();
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
    public void Load()
    {
        for (int i = 0; i < Prefecturedata.Length; i++)
        {
            List<data> list = Prefecturedata[i];
            if (list == null) continue;
            for (int f = 0; f < list.Count; f++)
            {
                list[f].getflg = false;
            }
            Prefecturedata[i] = list;
        }

        string lstr = PlayerPrefs.GetString(SaveKey);
        if (lstr == "")
        {
            get_data = new List<Savedata>();
            Debug.Log("セーブデータはないよ");
            return;
        }
        get_data = JsonUtility.FromJson<Serialization<Savedata>>(lstr).ToList();

        for (int i = 0; i < get_data.Count; i++)
        {
            List<data> list = Prefecturedata[get_data[i].prefecture_num];
            list[get_data[i].char_num].getflg = true;
            Prefecturedata[get_data[i].prefecture_num] = list;
        }
    }
}