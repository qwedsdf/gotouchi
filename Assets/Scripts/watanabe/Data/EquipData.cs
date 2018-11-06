using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 装備データクラス
/// (4桁の数値を文字列として保持)
/// </summary>
public class EquipData
{
    /// <summary>
    /// 頭装備
    /// </summary>
    public string equipHead = "human006h4_fashion001_baseball_cap"; // 仮初期データ
    //public string equipHead = "0000";

    /// <summary>
    /// 体装備
    /// </summary>
    public string equipBody = "human006h4_fashion001_baseball_wear"; // 仮初期データ
    //public string equipBody = "0000";

    /// <summary>
    /// 脚装備
    /// </summary>
    public string equipLeg = "human006h4_fashion001_baseball_shose"; // 仮初期データ
    //public string equipLeg = "0000";

    /// <summary>
    /// ドロップダウンの値
    /// </summary>
    public int[] dropDownValue = new int[] { 0, 0, 0 };

    public EquipData Clone()
    {
        EquipData clone = new EquipData();
        clone.equipHead = equipHead;
        clone.equipBody = equipBody;
        clone.equipLeg = equipLeg;
        clone.dropDownValue = dropDownValue;

        return clone;
    }
}
