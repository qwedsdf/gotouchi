using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 汎用ユーティリティクラス
/// </summary>
public class Utility : MonoBehaviour
{
    /// <summary>
    /// 任意の型でListのジェネリックを指定する
    /// </summary>
    /// <typeparam name="T">任意の型のList</typeparam>
    [Serializable]
    public class Wrapper<T>
    {
        public List<T> list = null;
    }

    /// <summary>
    /// JSON形式のデータをList<T>型で返す
    /// </summary>
    /// <typeparam name="T">結果を格納する任意の型</typeparam>
    /// <param name="json">JSON形式のデータ</param>
    /// <returns>データを格納したList</returns>
    public static List<T> ListFromJson<T>(string json)
    {
        Wrapper<T> wrapper = new Wrapper<T>();

        try
        {
            string newJson = "{ \"list\": " + json + "}";
            wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return wrapper.list;
    }
}
