using System;
using UnityEngine;

[Serializable]
public class CharacterManager
{
    [HideInInspector]
    public GameObject instance;
    // ラウンド勝利数
    [HideInInspector]
    public int roundWins;
}
