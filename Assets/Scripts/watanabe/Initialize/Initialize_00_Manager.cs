using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize_00_Manager : MonoBehaviour
{
    private void Awake()
    {
        // ゲームのフレームレートを設定
        Application.targetFrameRate = GameData.FrameRate;
    }

    private void Start()
    {

    }
}
