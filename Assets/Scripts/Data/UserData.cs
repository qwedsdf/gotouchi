﻿using System;
using UnityEngine;

/// <summary>
/// ユーザー情報
/// </summary>
[Serializable]
public class UserData
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    public int UserId = 0;

    /// <summary>
    /// ユーザー名
    /// </summary>
    public string UserName = "";

    /// <summary>
    /// 地域ID
    /// </summary>
    public int RegionId = 0;

    /// <summary>
    /// 都道府県ID
    /// </summary>
    public int PrefecturesId = 0;

    /// <summary>
    /// 応援キャラクターID
    /// </summary>
    public int CharacterId = 0;

    /// <summary>
    /// ユーザーの使用キャラクター画像名
    /// </summary>
    public string CharacterTexName = "";

    /// <summary>
    /// 所持ジュエル
    /// </summary>
    //public int Juwel = 0;

    /// <summary>
    /// 所持スター
    /// </summary>
    //public int StarCount = 0;

    /// <summary>
    /// ログインフラグ
    /// </summary>
    //public bool LoginFlg = false;

    /// <summary>
    /// 登録日
    /// </summary>
    //public string CreateDate;

    /// <summary>
    /// 更新日
    /// </summary>
    //public string UpdateDate;

    /// <summary>
    /// 動画広告状態
    /// </summary>
    //public int Stage_AdMovieState = 0;
    //public bool showAdButtonOK = false;


    /// <summary>
    /// 動画広告状態 UnityAds
    /// </summary>
    //public int Stage_UnityAdMovieState = 0;
    //public bool showUnityAdButtonOK = false;


    /// <summary>
    /// 動画広告状態 UnityAdFully
    /// </summary>
    //public int Stage_UnityAdFullyMovieState = 0;
    //public bool showUnityAdFullyButtonOK = false;
}