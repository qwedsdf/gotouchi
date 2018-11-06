using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : SingletonMonoBehaviour<GameData>
{
    /// <summary>
    /// フレームレート
    /// </summary>
    public const int FrameRate = 60;

    /// <summary>
    /// フェードイン・フェードアウト速度
    /// </summary>
    public const float FadeSpeed = 0.3f;

    #region シーン名

    /// <summary>
    /// シーン名:00_Managerシーン
    /// </summary>
    public const string Scene_Manager = "00_Manager";

    /// <summary>
    /// シーン名:01_Splashシーン
    /// </summary>
    public const string Scene_Splash = "01_Splash";

    /// <summary>
    /// シーン名:02_Titleシーン
    /// </summary>
    public const string Scene_Title = "02_Title";

    /// <summary>
    /// シーン名:03_Homeシーン
    /// </summary>
    public const string Scene_Home = "03_Home";

    /// <summary>
    /// シーン名:04_Avatarシーン
    /// </summary>
    public const string Scene_Avatar = "04_Avatar";

    /// <summary>
    /// シーン名:05_Battleシーン
    /// </summary>
    public const string Scene_Battle = "05_Battle";

    /// <summary>
    /// シーン名:101_Loadingシーン
    /// </summary>
    public const string Scene_Loading = "101_Loading";

    /// <summary>
    /// シーン名:102_Profileシーン
    /// </summary>
    public const string Scene_Profile = "102_Profile";

    /// <summary>
    /// シーン名:103_SelectAreaシーン
    /// </summary>
    public const string Scene_SelectArea = "103_SelectArea";

    /// <summary>
    /// シーン名:104_Selectシーン
    /// </summary>
    public const string Scene_Select = "104_Select";

    /// <summary>
    /// シーン名:105_PictureBookシーン
    /// </summary>
    public const string Scene_PictureBook = "105_PictureBook";

    #endregion

    #region 一時保存データ

    /// <summary>
    /// ユーザー情報
    /// </summary>
    public static UserData UserData;

    /// <summary>
    /// 所持アイテム情報
    /// </summary>
    //public static List<PossessionData> PossessionDataList;

    /// <summary>
    /// 装備中の衣装
    /// </summary>
    public static EquipData equipData;

    /// <summary>
    /// 着せ替え用
    /// </summary>
    public static EquipData backupEquipData;

    #endregion
}
