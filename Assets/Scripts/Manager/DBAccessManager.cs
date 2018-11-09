using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DBアクセス管理クラス
/// </summary>
public class DBAccessManager : SingletonMonoBehaviour<DBAccessManager>
{

    #region 定数

    /// <summary>
    /// サーバーのルートパス
    /// </summary>
    private const string rootPath = "http://sub0000545829.hmk-temp.com/HandPush/";

    /// <summary>
    /// ユーザーマスタ用PHP
    /// </summary>
    public static string php_m_user = "m_user.php";

    /// <summary>
    /// アイテムマスタ用PHP
    /// </summary>
    //public static string php_m_item = "m_item.php";

    /// <summary>
    /// 所持アイテムテーブル用PHP
    /// </summary>
    //public static string php_t_possesionitem = "t_possessionitem.php";

    /// <summary>
    /// 禁止文字テーブル用PHP
    /// </summary>
    public static string php_t_dirty = "t_dirty.php";

    /// <summary>
    /// 課金者テーブル用PHP
    /// </summary>
    //public static string php_m_IAPPlayer = "m_IAPPlayer.php";

    #endregion

    /// <summary>
    /// 起動するPHPのパス
    /// </summary>
    private static string phpPath;

    /// <summary>
    /// WWWForm
    /// </summary>
    private static WWWForm wwwForm;

    /// <summary>
    /// DBアクセス結果
    /// </summary>
    private static string wwwResult;

    /// <summary>
    /// エラー発生フラグ
    /// </summary>
    public bool ErrFlg;

    // Use this for initialization
    public void Start()
    {
        ErrFlg = false;
    }


    ///// <summary>
    ///// ユーザー情報を取得する
    ///// </summary>
    ///// <param name="userId">ユーザーID</param>
    ///// <returns></returns>
    //public IEnumerator GetUserData(int userId)
    //{
    //    Debug.Log("ユーザーマスタ:SELECT");
    //    phpPath = rootPath + php_m_user;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");
    //    wwwForm.AddField("userId", userId);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        GameData.UserData = Utility.ListFromJson<UserData>(wwwResult)[0];
    //    }
    //}

    ///// <summary>
    ///// ユーザーマスタへのINSERT
    ///// </summary>
    ///// <param name="userName">入力したユーザー名</param>
    ///// <returns></returns>
    //public IEnumerator InsertNewUser(string userName)
    //{
    //    Debug.Log("ユーザーマスタ:INSERT");
    //    phpPath = rootPath + php_m_user;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "INSERT");
    //    wwwForm.AddField("userName", userName);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        // 登録したユーザーIDはローカルセーブデータに保存
    //        SaveData.SetInt(SaveKey.UserId, int.Parse(wwwResult));
    //        SaveData.Save();
    //    }
    //}


    /// <summary>
    /// ユーザー情報を取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    public IEnumerator GetUserData(int userId)
    {
        Debug.Log("ユーザーマスタ:SELECT");
        phpPath = rootPath + php_m_user;
        wwwForm = new WWWForm();
        wwwForm.AddField("process", "SELECT");
        wwwForm.AddField("userId", userId);

        yield return StartCoroutine("DBAccess");

        if (!ErrFlg)
        {
            GameData.UserData = Utility.ListFromJson<UserData>(wwwResult)[0];
        }
    }

    /// <summary>
    /// ユーザーマスタへのINSERT
    /// </summary>
    /// <param name="userName">入力したユーザー名</param>
    /// <returns></returns>
    public IEnumerator InsertNewUser(string userName)
    {
        Debug.Log("ユーザーマスタ:INSERT");
        phpPath = rootPath + php_m_user;
        wwwForm = new WWWForm();
        wwwForm.AddField("process", "INSERT");
        wwwForm.AddField("userName", userName);

        yield return StartCoroutine("DBAccess");

        if (!ErrFlg)
        {
            // 登録したユーザーIDはローカルセーブデータに保存
            SaveData.SetInt(SaveKey.UserId, int.Parse(wwwResult));
            SaveData.Save();
        }
    }

    /// <summary>
    /// ログイン日時を更新する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    //public IEnumerator UpdateLoginDate(int userId)
    //{
    //    Debug.Log("ユーザーマスタ:UPDATE - LOGINDATE");
    //    phpPath = rootPath + php_m_user;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "LOGINDATE");
    //    wwwForm.AddField("userId", userId);

    //    yield return StartCoroutine("DBAccess");
    //}

    /// <summary>
    /// ログインフラグを更新する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    //public IEnumerator UpdateLoginFlg(int userId)
    //{
    //    Debug.Log("ユーザーマスタ:UPDATE - LOGINFLG");
    //    phpPath = rootPath + php_m_user;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "LOGINFLG");
    //    wwwForm.AddField("userId", userId);

    //    yield return StartCoroutine("DBAccess");
    //}

    ///// <summary>
    ///// ユーザー情報を更新する(キャラ名、勉強レベル、勉強ポイント、ジュエル、投票カウント)
    ///// </summary>
    ///// <returns></returns>
    //public IEnumerator UpdateUserData()
    //{
    //    Debug.Log("ユーザーマスタ:UPDATE");
    //    phpPath = rootPath + php_m_user;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "UPDATE");
    //    wwwForm.AddField("userName", GameData.UserData.UserName);
    //    //wwwForm.AddField("juwel", GameData.UserData.Juwel);
    //    wwwForm.AddField("userId", GameData.UserData.UserId);

    //    yield return StartCoroutine("DBAccess");
    //}


    /// <summary>
    /// ユーザー情報を更新する
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateUserData()
    {
        Debug.Log("ユーザーマスタ:UPDATE");
        phpPath = rootPath + php_m_user;
        wwwForm = new WWWForm();
        wwwForm.AddField("process", "UPDATE");
        wwwForm.AddField("userName", GameData.UserData.UserName);
        wwwForm.AddField("regionId", GameData.UserData.RegionId);
        wwwForm.AddField("prefecturesId", GameData.UserData.PrefecturesId);
        wwwForm.AddField("characterId", GameData.UserData.CharacterId);
        wwwForm.AddField("userId", GameData.UserData.UserId);

        yield return StartCoroutine("DBAccess");
    }


    #region アイテムマスタ

    /// <summary>
    /// 店頭に並ぶアイテムの一覧を取得する
    /// </summary>
    /// <param name="itemtype">アイテムタイプ</param>
    /// <returns></returns>
    //public IEnumerator GetShopLinenapList(string itemType)
    //{
    //    Debug.Log("アイテムマスタ:SELECT");
    //    phpPath = rootPath + php_m_item;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");
    //    wwwForm.AddField("itemType", itemType);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        yield return string.IsNullOrEmpty(wwwResult) ? new List<ItemData>() : Utility.ListFromJson<ItemData>(wwwResult);
    //    }
    //}

    /// <summary>
    /// ミニゲーム景品用アイテムの一覧を取得する
    /// </summary>
    /// <param name="itemtype">アイテムタイプ</param>
    /// <returns></returns>
    //public IEnumerator GetPrizeItemList()
    //{
    //    Debug.Log("ミニゲームアイテムマスタ:SELECT");
    //    phpPath = rootPath + php_m_item_prize;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        List<ItemData> ItemList = Utility.ListFromJson<ItemData>(wwwResult);

    //        yield return ItemList;
    //    }
    //}

    #endregion

    #region 所持アイテムテーブル

    /// <summary>
    /// 所持アイテムを取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    //public IEnumerator GetPossesionItemList(int userId)
    //{
    //    Debug.Log("所持アイテムテーブル:SELECT");
    //    phpPath = rootPath + php_t_possesionitem;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");
    //    wwwForm.AddField("userId", userId);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        GameData.PossessionDataList = Utility.ListFromJson<PossessionData>(wwwResult);
    //        Debug.Log("所持アイテム件数：" + GameInfo.PossessionDataList.Count + "個");
    //    }
    //}

    /// <summary>
    /// 所持アイテムを登録する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="itemId">アイテムID</param>
    /// <param name="itemType">アイテム種別</param>
    /// <returns></returns>
    //public IEnumerator InsertPossesionItem(int userId, int itemId, string itemType)
    //{
    //    Debug.Log("所持アイテムテーブル:INSERT");
    //    phpPath = rootPath + php_t_possesionitem;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "INSERT");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("itemId", itemId);
    //    wwwForm.AddField("itemType", itemType);

    //    yield return StartCoroutine("DBAccess");
    //}

    #endregion

    #region 開催期間テーブル

    /// <summary>
    /// 開催期間情報を取得する
    /// </summary>
    /// <returns></returns>
    //public IEnumerator GetPeriodData()
    //{
    //    Debug.Log("開催期間テーブル:SELECT");
    //    phpPath = rootPath + php_t_period;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        List<PeriodData> period = Utility.ListFromJson<PeriodData>(wwwResult);
    //        GameInfo.PeriodData = period[0];
    //    }
    //}

    #endregion

    #region エントリーテーブル

    /// <summary>
    /// エントリー情報を取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns></returns>
    //public IEnumerator GetEntryData(int userId)
    //{
    //    Debug.Log("エントリーテーブル:SELECT");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "SELECT");
    //    wwwForm.AddField("userId", userId);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        GameInfo.EntryData = Utility.ListFromJson<EntryData>(wwwResult);
    //    }
    //}



    /// <summary>
    /// エントリー情報のダミーユーザーを今期のPeriodに変更する
    /// </summary>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator UpdateDummyUser(int period)
    //{
    //    Debug.Log("エントリーテーブル:DUMMY USER UPDATE");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "DUMMYUSER");
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    //if (!ErrFlg)
    //    //{
    //    //    Debug.Log("エントリーテーブル:DUMMY USER UPDATEじゃない");
    //    //}
    //}


    /// <summary>
    /// エントリー情報を登録する
    /// </summary>
    /// <param name="entryData">エントリー情報</param>
    /// <returns></returns>
    //public IEnumerator InsertEntryData(EntryData entryData)
    //{
    //    Debug.Log("エントリーテーブル:INSERT");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "INSERT");
    //    wwwForm.AddField("period", entryData.Period);
    //    wwwForm.AddField("userId", entryData.UserId);
    //    wwwForm.AddField("userName", entryData.UserName);
    //    wwwForm.AddField("hair", entryData.Hair);
    //    wwwForm.AddField("eye", entryData.Eye);
    //    wwwForm.AddField("skin", entryData.Skin);
    //    wwwForm.AddField("tops", entryData.Tops);
    //    wwwForm.AddField("bottoms", entryData.Bottoms);
    //    wwwForm.AddField("socks", entryData.Socks);
    //    wwwForm.AddField("shoes", entryData.Shoes);
    //    wwwForm.AddField("accesary1", entryData.Accesary1);
    //    wwwForm.AddField("accesary2", entryData.Accesary2);
    //    wwwForm.AddField("accesary3", entryData.Accesary3);

    //    yield return StartCoroutine("DBAccess");
    //}


    /// <summary>
    /// エントリー情報を更新する(報酬獲得フラグ)
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator UpdateEntryData(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:UPDATE");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "UPDATE");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");
    //}

    /// <summary>
    /// エントリー情報を削除する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator DeleteEntryData(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:DELETE");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "DELETE");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");
    //}

    /// <summary>
    /// 自分の順位を取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator GetMyRank(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:MYRANK");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "MYRANK");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        if (wwwResult != null)
    //        {
    //            // 自分の順位と被投票数をカンマ区切りで取得する
    //            string[] result = wwwResult.Split(',');
    //            GameInfo.MyRank = result[0];
    //            GameInfo.MyVote = result[1];
    //        }
    //    }
    //}


    /// <summary>
    /// 未来のエントリー順位を取得する
    /// 順位0 なら未エントリー
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator GetFutureEntry(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:MYRANK");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "MYRANK");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        if (wwwResult != null)
    //        {
    //            // 自分の順位と被投票数をカンマ区切りで取得する
    //            string[] result = wwwResult.Split(',');
    //            GameInfo.NextEntry = result[0];
    //        }
    //    }
    //}


    /// <summary>
    /// エントリーUSER数取得
    /// </summary>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator GetEntryNum(int period)
    //{
    //    Debug.Log("エントリーテーブル:GETENTRYNUM");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "GETENTRYNUM");
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");
    //    if (!ErrFlg)
    //    {
    //        // エントリーユーザー数
    //        //Debug.Log("ランキングエントリー数は" + int.Parse(wwwResult));
    //        GameInfo.EntryNum = int.Parse(wwwResult);
    //    }
    //}




    /// <summary>
    /// ランキングデータを取得する
    /// </summary>
    /// <returns></returns>
    //public IEnumerator GetStageRankingData(int period)
    //{
    //    Debug.Log("エントリーテーブル:RANKING");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "RANKING");
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        yield return string.IsNullOrEmpty(wwwResult) ? new List<EntryData>() : Utility.ListFromJson<EntryData>(wwwResult);
    //    }
    //}

    /// <summary>
    /// 自分の順位と前後の順位を取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator GetMyRankOther(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:MYRANK+OTHER");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "OTHER");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        if (wwwResult != null)
    //        {
    //            yield return string.IsNullOrEmpty(wwwResult) ? new List<EntryData>() : Utility.ListFromJson<EntryData>(wwwResult);
    //        }
    //    }
    //}

    /// <summary>
    /// 自分以外のランダムな二人を選出する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator GetRandomPare(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:RANDOM");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "RANDOM");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        List<EntryData> resultList = Utility.ListFromJson<EntryData>(wwwResult);
    //        yield return resultList;
    //    }
    //}

    //public IEnumerator GetRandomACCESARYPare(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:RANDOMACCESARY3");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "RANDOMACCESARY");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");

    //    if (!ErrFlg)
    //    {
    //        List<EntryData> resultList = Utility.ListFromJson<EntryData>(wwwResult);
    //        yield return resultList;
    //    }
    //}


    /// <summary>
    /// エントリー情報を更新する(投票回数)
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator UpdateVoteCount(int userId, int period)
    //{
    //    Debug.Log("エントリーテーブル:UPDATE-VOTE");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "VOTE");
    //    wwwForm.AddField("userId", userId);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");
    //}

    /// <summary>
    /// エントリー情報を更新する(登壇回数)
    /// </summary>
    /// <param name="char1">一人目の登壇者</param>
    /// <param name="char2">二人目の登壇者</param>
    /// <param name="period">開催期</param>
    /// <returns></returns>
    //public IEnumerator UpdateApperCount(int char1, int char2, int period)
    //{
    //    Debug.Log("エントリーテーブル:UPDATE-APPER");
    //    phpPath = rootPath + php_t_entry;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "APPER");
    //    wwwForm.AddField("char1", char1);
    //    wwwForm.AddField("char2", char2);
    //    wwwForm.AddField("period", period);

    //    yield return StartCoroutine("DBAccess");
    //}

    #endregion

    #region 禁止用語チェック
    /// <summary>
    /// 所持アイテムを登録する
    /// </summary>
    /// <param name="userName">入力したユーザー名</param>
    /// <returns></returns>
    public IEnumerator GetDirtyWord()
    {
        Debug.Log("禁止用語チェック:SELECT");
        phpPath = rootPath + php_t_dirty;
        wwwForm = new WWWForm();
        wwwForm.AddField("process", "SELECT");
        //wwwForm.AddField("KEYWORD", word);

        yield return StartCoroutine("DBAccess");

        if (!ErrFlg)
        {
            if (wwwResult != null)
            {
                //yield return string.IsNullOrEmpty(wwwResult) ? new List<DirtyWordData>() : Utility.ListFromJson<DirtyWordData>(wwwResult);
                List<DirtyWordData> resultList = Utility.ListFromJson<DirtyWordData>(wwwResult);
                yield return resultList;
            }
        }

    }
    #endregion

    #region 勉強問題の動向をDBに送る
    /// <summary>
    /// 勉強問題の動向をDBに送る
    /// </summary>
    /// <param name="GenreName">教科名</param>
    /// <param name="LevelName">レベル</param>
    /// <returns></returns>
    //public IEnumerator DataMining()
    //{
    //    Debug.Log("勉強データ送信:SET");
    //    phpPath = rootPath + php_t_trend;
    //    wwwForm = new WWWForm();
    //    wwwForm.AddField("process", "COUNTUP");
    //    wwwForm.AddField("Genre", GameInfo.SelectedQuizGenre);
    //    wwwForm.AddField("Level", GameInfo.SelectedQuizLevel);

    //    yield return StartCoroutine("DBAccess");
    //}

    #endregion

    #region 課金者情報の登録
    /// <summary>
    /// ユーザーマスタへのINSERT
    /// </summary>
    /// <param name="userName">入力したユーザー名</param>
    /// <returns></returns>
    //    public IEnumerator InsertIAPPlayer(string jewel)
    //    {
    //        Debug.Log("課金ユーザー:INSERT");
    //        phpPath = rootPath + php_m_IAPPlayer;
    //        wwwForm = new WWWForm();
    //        wwwForm.AddField("process", "INSERT");
    //        wwwForm.AddField("userId", GameData.UserData.UserId);
    //        wwwForm.AddField("userName", GameData.UserData.UserName);
    //        wwwForm.AddField("Juwel", int.Parse(jewel));
    //#if UNITY_ANDROID
    //        wwwForm.AddField("OS", "Google");
    //#else
    //        wwwForm.AddField("OS", "Apple");
    //#endif

    //        yield return StartCoroutine("DBAccess");

    //    }
    #endregion


    #region DBアクセス実行

    /// <summary>
    /// DBアクセス実行
    /// </summary>
    /// <returns></returns>
    private IEnumerator DBAccess()
    {
        //this.transform.FindChild("DBAccessFilter").gameObject.SetActive(true);
        StartCoroutine("AccessWait");

        using (WWW www = new WWW(phpPath, wwwForm.data))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                // SQL実行結果を返す
                wwwResult = www.text;
                Debug.Log(wwwResult);
            }
            else
            {
                Debug.Log(www.error);
                //GameObject errorMsg = Instantiate((GameObject)(Resources.Load("Prefabs/MessageBox")));
                //errorMsg.GetComponent<MessageBoxManager>().Initialize_OK("通信エラーが発生しました。\nタイトルに戻ります。", GoToTitle);

                ErrFlg = true;
                yield break;
            }
        }
        //this.transform.FindChild("DBAccessFilter").gameObject.SetActive(false);
        Handheld.StopActivityIndicator();
    }

    private IEnumerator AccessWait()
    {
#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.Gray);
#elif UNITY_ANDROID
        Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
#endif
        Handheld.StartActivityIndicator();
        yield return new WaitForSeconds(0);
    }

    /// <summary>
    /// タイトル画面へ遷移
    /// </summary>
    //public void GoToTitle()
    //{
    //    ErrFlg = false;
    //    FadeManager.Instance.LoadLevel(GameInfo.Scene_Title, GameInfo.FadeSpeed);
    //}

    #endregion
}
