using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelectArea : MonoBehaviour
{
    /// <summary>
    /// 地方ボタンを押した時に実行される
    /// </summary>
    /// <param name="num">Inspectorで値を設定する(各ボタンごとに設定してください)</param>
    public void SelectedArea(int num)
    {
        GameData.UserData.RegionId = num;
        SceneFadeManager.Instance.Load(GameData.Scene_TestSelectPrefecture, GameData.FadeSpeed);
    }
}
