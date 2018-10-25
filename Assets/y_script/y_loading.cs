using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class y_loading : MonoBehaviour {
    AsyncOperation async;
    public Text text;

	// Use this for initialization
	void Start () {
        //string str = Application.dataPath;
        //text.text = str;
        
        //string[] files = System.IO.Directory.GetFiles(@str, "*.png", System.IO.SearchOption.AllDirectories);
        //text.text = files.Length.ToString();

        StartCoroutine(LoadScene());
	}
	
	// Update is called once per frame
	void Update () {
        test();
	}

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("select");

        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void test()
    {
        text.text = "通っているよ";
    }
}
