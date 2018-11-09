using System;
using UnityEngine;

[Serializable]
public class CharacterManager
{
    /// <summary>
    /// Player or CPUのインスタンス
    /// </summary>
    [HideInInspector]
    public GameObject instance;

    /// <summary>
    /// 現在の勝利数
    /// </summary>
    [HideInInspector]
    public int wins;

    /// <summary>
    /// コントローラースクリプト
    /// </summary>
    private Controller controller;


    /// <summary>
    /// セットアップ
    /// </summary>
    public void Setup()
    {
        // コントローラースクリプトを取得
        controller = instance.GetComponent<Controller>();
    }

    /// <summary>
    /// 制御できるようにする
    /// </summary>
    public void EnableControl()
    {
        controller.enabled = true;
    }

    /// <summary>
    /// 制御できないようにする
    /// </summary>
    public void DisableControl()
    {
        controller.enabled = false;
    }

    /// <summary>
    /// インスタンスをデフォルト状態に戻す
    /// </summary>
    public void Reset()
    {
        instance.SetActive(false);
        instance.SetActive(true);
    }
}
