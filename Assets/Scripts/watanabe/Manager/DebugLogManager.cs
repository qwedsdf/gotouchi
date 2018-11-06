using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// デバッグログ管理クラス
/// </summary>
public class DebugLogManager : SingletonMonoBehaviour<DebugLogManager>
{
    /// <summary>
    /// デバッグログゲームオブジェクト
    /// </summary>
    public GameObject debugLogObj;
    /// <summary>
    /// デバッグボタンゲームオブジェクト
    /// </summary>
    public GameObject debugButtonObj;
    /// <summary>
    /// デバッグログテキスト
    /// </summary>
    public Text debugLogText;


    private void Start()
    {
        DebugLogOff();
    }

    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    public void ReturnToTitle()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Destroy(player);
        }

        ResetText();
        DebugLogOff();

        SceneFadeManager.Instance.Load(GameData.Scene_Title, GameData.FadeSpeed);
    }

    /// <summary>
    /// ホームに戻るボタン
    /// </summary>
    public void ReturnToHome()
    {
        ResetText();
        DebugLogOff();

        SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
    }

    /// <summary>
    /// デバッグログオン
    /// </summary>
    public void DebugLogOn()
    {
        debugLogObj.SetActive(true);
        debugButtonObj.SetActive(false);
    }

    /// <summary>
    /// デバッグログオフ
    /// </summary>
    public void DebugLogOff()
    {
        debugLogObj.SetActive(false);
        debugButtonObj.SetActive(true);
    }

    /// <summary>
    /// 表示したいデバッグログを追加
    /// </summary>
    /// <param name="str"></param>
    public void AddDebugLogText(string str)
    {
        str += "\n";
        debugLogText.text += str;
        Debug.Log(str);
    }

    /// <summary>
    /// デバッグログをリセット
    /// </summary>
    public void ResetText()
    {
        debugLogText.text = "";
    }
}
