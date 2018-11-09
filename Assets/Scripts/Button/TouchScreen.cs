using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// タイトル画面 ボタン押下制御
/// </summary>
public class TouchScreen : MonoBehaviour
{
    /// <summary>
    /// メッセージボックス
    /// </summary>
    private GameObject messageBoxPrefab;

    /// <summary>
    /// インプットフィールド
    /// </summary>
    private GameObject inputField;

    /// <summary>
    /// ユーザーID(新規登録用)
    /// </summary>
    private int userId = 0;

    /// <summary>
    /// ユーザー名(新規登録用)
    /// </summary>
    private string userName;

    //禁止文字List
    private List<DirtyWordData> DirtyWordList;

    // 連打防止用
    private bool DontTouchFlg = false;

    // テスト用変数
    public int regionId;
    public int prefecturesId;
    public int characterId;

    // Use this for initialization
    void Start()
    {
        userId = 0;
        DontTouchFlg = false;
        messageBoxPrefab = (GameObject)Resources.Load("Prefabs/MessageBox");
        inputField = GameObject.Find("Canvas").transform.Find("NameInput/InputField").gameObject;
        InputReset();
    }

    /// <summary>
    /// タイトル画面タッチ時
    /// </summary>
    public void Screen_OnTouch()
    {
        AudioManager.Instance.PlaySE("touch");
        // セーブデータからプレイヤーIDを取得
        this.userId = SaveData.GetInt(SaveKey.UserId, 0);

        if (this.userId != 0)
        {
            if (!DontTouchFlg)
            {
                // ユーザーが登録済みならDBからユーザー情報を取得してメニューへ遷移            
                Login();
                DontTouchFlg = true;
            }
        }
        else
        {
            SaveData.Clear();
            GameObject.Find("Canvas").transform.Find("NameInput").gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// OKボタンを押した時
    /// </summary>
    public void OKButton()
    {
        if (!DontTouchFlg)
        {
            // 入力
            string tmpUserName = inputField.GetComponent<InputField>().text;
            DontTouchFlg = true;

            // 公序良俗チェック
            StartCoroutine(DirtyWordCheck(tmpUserName));
        }
    }

    /// <summary>
    /// 新規ユーザー登録時　公序良俗チェック
    /// </summary>
    private IEnumerator DirtyWordCheck(string TmpName)
    {
        this.userName = "★★";

        // 不正ワードリスト取得
        IEnumerator coroutine = DBAccessManager.Instance.GetDirtyWord();
        yield return StartCoroutine(coroutine);

        this.DirtyWordList = (List<DirtyWordData>)coroutine.Current;

        // 空文字
        TmpName = TmpName.Replace("　", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
        //Debug.Log("入力チェック: " + TmpName.ToString() );

        // 絵文字を取り除く
        string ReName = "";
        foreach (char c in TmpName)
        {
            if (!char.IsSurrogate(c))
            {
                ReName += c;
            }
        }

        TmpName = ReName;

        GameObject msgBox;

        // 入力チェック
        if (string.IsNullOrEmpty(TmpName))
        {
            msgBox = Instantiate(messageBoxPrefab);
            msgBox.GetComponent<MessageBoxManager>().Initialize_OK("名前が入力されていません。", null);
            Debug.Log("名前が入力されていません。");
            InputReset();
            this.DontTouchFlg = false;

            yield break;
        }

        // 不正ワードチェック
        foreach (DirtyWordData Dirty in this.DirtyWordList)
        {
            int dirtyCheck = 0;
            dirtyCheck = TmpName.IndexOf(Dirty.Keyword);
            if (-1 != dirtyCheck)
            {
                msgBox = Instantiate(messageBoxPrefab);
                msgBox.GetComponent<MessageBoxManager>().Initialize_OK("その名前は使えません。", null);
                Debug.Log("その名前は使えません。");
                InputReset();
                this.DontTouchFlg = false;

                yield break;
            }
        }

        this.userName = TmpName;

        msgBox = Instantiate(messageBoxPrefab);
        msgBox.GetComponent<MessageBoxManager>().Initialize_YesNo(userName + "\n\nこの名前でいいですか?", InsertUser, null);

        this.DontTouchFlg = false;
    }

    /// <summary>
    /// 入力をリセットする
    /// </summary>
    public void InputReset()
    {
        inputField.GetComponent<InputField>().text = "";
        inputField.GetComponent<InputField>().ActivateInputField();
    }

    /// <summary>
    /// サーバー通信を行い、ユーザー情報を登録する
    /// </summary>
    private void InsertUser()
    {
        StartCoroutine("InsertUserData");
    }

    /// <summary>
    /// サーバー通信を行い、ユーザー情報を更新する
    /// </summary>
    private void Login()
    {
        StartCoroutine("UpdateUserData");
    }

    /// <summary>
    /// 新規ユーザー登録処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator InsertUserData()
    {
        if (this.userId != 0)
        {
            yield break;
        }

        // 新規ユーザーをINSERT
        yield return StartCoroutine(DBAccessManager.Instance.InsertNewUser(this.userName));
        if (DBAccessManager.Instance.ErrFlg)
        {
            yield break;
        }

        this.userId = SaveData.GetInt(SaveKey.UserId, 0);

        // INSERTしたユーザー情報を取得
        yield return StartCoroutine(DBAccessManager.Instance.GetUserData(userId));
        if (DBAccessManager.Instance.ErrFlg)
        {
            yield break;
        }

        // -------------------------------------------

        // テスト
        GameData.UserData.RegionId = regionId;
        GameData.UserData.PrefecturesId = prefecturesId;
        GameData.UserData.CharacterId = characterId;
        GameData.UserData.CharacterTexName = string.Format("{0:00}", GameData.UserData.RegionId) + "_" + string.Format("{0:00}", GameData.UserData.PrefecturesId) + "_" + string.Format("{0:00}", GameData.UserData.CharacterId);

        //GoToMenu();

        // -------------------------------------------

        // デバッグ表示
        DebugLogManager.Instance.AddDebugLogText("USERID:" + String.Format("{0:00000000000000000000}", GameData.UserData.UserId));
        DebugLogManager.Instance.AddDebugLogText("USERNAME:" + GameData.UserData.UserName);
        DebugLogManager.Instance.AddDebugLogText("REGIONID:" + String.Format("{0:00}", GameData.UserData.RegionId));
        DebugLogManager.Instance.AddDebugLogText("PREFECTUREID:" + String.Format("{0:00}", GameData.UserData.PrefecturesId));
        DebugLogManager.Instance.AddDebugLogText("CHARACTERID:" + String.Format("{0:00000000}", GameData.UserData.CharacterId));

        // キャラ選択シーンへ遷移
        GoToSelect();
    }

    /// <summary>
    /// ユーザー情報更新(その日のログインフラグ、ログイン日付)
    /// </summary>
    /// <param name="path">PHPのパス</param>
    /// <returns></returns>
    private IEnumerator UpdateUserData()
    {
        // ユーザー情報取得
        yield return StartCoroutine(DBAccessManager.Instance.GetUserData(this.userId));
        if (DBAccessManager.Instance.ErrFlg)
        {
            yield break;
        }

        // デバッグ表示
        //DebugLogManager.Instance.AddDebugLogText("USERID:" + (GameData.UserData.UserId).ToString());
        //DebugLogManager.Instance.AddDebugLogText("USERNAME:" + GameData.UserData.UserName);

        // メニュー画面へ遷移
        GoToMenu();
    }

    /// <summary>
    /// キャラ選択シーンへ遷移
    /// </summary>
    public void GoToSelect()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_SelectArea, GameData.FadeSpeed);
    }

    /// <summary>
    /// ホーム画面へ遷移
    /// </summary>
    public void GoToMenu()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
    }
}
