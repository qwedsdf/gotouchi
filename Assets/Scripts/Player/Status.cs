using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    /// <summary>
    /// 体力
    /// </summary>
    public float hp;

    /// <summary>
    /// 攻撃力
    /// </summary>
    public float attack;

    /// <summary>
    /// 防御力
    /// </summary>
    public float defense;

    /// <summary>
    /// ログディスプレイ
    /// </summary>
    public LogDisplay logDisp;
    public int num;


    private void Update()
    {
        if (hp <= 0)
        {
            Debug.Log(gameObject.name + "のhpは0以下になりました。");
        }
    }


    /// <summary>
    /// 初期ステータス
    /// </summary>
    public void InitStatus()
    {
        hp = 10;
        attack = 5;
        defense = 3;

        logDisp.DebagLog("ノーマルタイプ", num);
    }

    /// <summary>
    /// 攻撃タイプ
    /// </summary>
    public void AttackTypeButton()
    {
        attack = 8;
        defense = 3;
        logDisp.DebagLog("攻撃タイプ", num);
    }

    /// <summary>
    /// 防御タイプ
    /// </summary>
    public void DefenseTypeButton()
    {
        attack = 5;
        defense = 4;
        logDisp.DebagLog("防御タイプ", num);
    }
}
