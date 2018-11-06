using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブデータキー
/// </summary>
public class SaveKey : SingletonMonoBehaviour<SaveKey>
{
    /// <summary>
    /// ユーザー情報を取得・保存するキー
    /// </summary>
    public const string UserId = "UserId";

    /// <summary>
    /// 装備データを取得、保存するキー
    /// </summary>
    public const string equipData = "EquipData";

    /// <summary>
    /// ユーザーが使用しているキャラクターデータを取得、保存するキー
    /// </summary>
    public const string UserCharacter = "UserCharacter";
}
