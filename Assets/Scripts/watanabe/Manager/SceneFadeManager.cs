using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class SceneFadeManager : SingletonMonoBehaviour<SceneFadeManager>
{
    /// <summary>
    /// ローディング用
    /// </summary>
    private GameObject loadingObject;


    public void Start()
    {
        this.loadingObject = this.transform.Find("LoadingCanvas").gameObject;
    }

    // <summary>
    /// 遷移元のシーン名保持
    /// </summary>
    private string beforeSceneName;


    /// <summary>
    /// 1つ前の画面のシーン名を返す
    /// </summary>
    /// <returns>シーン名</returns>
    public string GetBeforeSceneName()
    {
        return this.beforeSceneName;
    }

    /// <summary>
    /// 画面遷移
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="interval">暗転にかかる時間(秒)</param>
    public void Load(string scene, float interval)
    {
        try
        {
            // 遷移元のシーン名を保持
            this.beforeSceneName = SceneManager.GetActiveScene().name;
            StartCoroutine(TransScene(scene, interval));
        }
        catch (Exception ex)
        {
            GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
            msgBox.GetComponent<MessageBoxManager>().Initialize_OK("シーンの遷移に失敗しました。\n" + ex.Message + "\n" + ex.StackTrace, null);
        }
    }

    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <param name="interval">暗転にかかる時間(秒)</param>
    private IEnumerator TransScene(string scene, float interval)
    {
        float time = 0;

        this.loadingObject.SetActive(true);
        RawImage raw = this.loadingObject.transform.Find("Screen").GetComponent<RawImage>();

        // ホワイトアウト
        if (raw != null)
        {
            while (time <= interval)
            {
                raw.color = new Color(1, 1, 1, Mathf.Lerp(0f, 0.9f, time / interval));
                time += Time.deltaTime;
                yield return null;
            }
            // 端数で真っ白にならないことがある為、補完する
            //raw.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, time / interval));
        }

        // 「ちょっとまってね！」を表示
        //this.loadingObject.transform.FindChild("Text").gameObject.SetActive(true);

        // シーン遷移
        SceneManager.LoadScene(scene);

        // 名前が変わる＝シーンが変わるのを待つ
        while (SceneManager.GetActiveScene().name != scene)
        {
            yield return null;
        }

        time = 0;

        // 「ちょっとまってね！」を非表示
        //this.loadingObject.transform.Find("Text").gameObject.SetActive(false);

        if (raw != null)
        {
            while (time <= interval)
            {
                raw.color = new Color(1, 1, 1, Mathf.Lerp(0.9f, 0f, time / interval));
                time += Time.deltaTime;
                yield return null;
            }

            // 端数で真っ白にならないことがある為、補完する
            //raw.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, time / interval));
        }

        this.loadingObject.SetActive(false);

        // 遷移先のシーンで「BeforeButton」があれば押下時の遷移先をセット
        //GameObject beforeButton = GameObject.Find("Canvas/BeforeButton");
        //if (beforeButton != null)
        //{
        //    beforeButton.GetComponent<BeforeButtonManager>().SetGotoSceneName(this.beforeSceneName);
        //}
    }
}
