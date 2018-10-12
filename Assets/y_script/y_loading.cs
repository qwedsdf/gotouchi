using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class y_loading : MonoBehaviour {
    AsyncOperation async;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync("select");

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
