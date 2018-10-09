using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class y_Datebase : MonoBehaviour {

    //キャラのデータ
	struct date {
        Sprite img;//画像
		string place_name;//場所
        string description;//説明文章
	}

    List<date> DateBase = new List<date>();

	// Use this for initialization
	void Start () {
        //string path = "/hoge/hogehoge/hoge.png";
        //string fileName = System.IO.Path.GetDirectoryName(path);
        //Debug.Log(fileName);

       // Sprite i = Resources.Load<Sprite>("DateChar/hukuoka/ふなっしー/img");
        //Debug.Log(i.name);
        string str = Application.dataPath + "/Resources/DateChar";
        string[] files = System.IO.Directory.GetFiles(@str, "*", System.IO.SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++) {
            if (System.IO.Path.GetExtension(files[i]) == ".meta") continue;
            string flie_parent = System.IO.Path.GetDirectoryName(files[i]);
            Debug.Log(System.IO.Path.GetFileNameWithoutExtension(flie_parent));
        }
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
