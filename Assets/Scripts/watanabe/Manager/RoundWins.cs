using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundWins : MonoBehaviour
{
    // ゲーム勝利に必要なラウンド数
    private int numRoundsToWin = 2;
    public static int playerWin;
    public static int cpuWin;
    // デバッグオブジェクト
    public GameObject logDisp;

    public static int getPlayerWin()
    {
        return playerWin;
    }

    public static int getCpuWin()
    {
        return cpuWin;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (getPlayerWin() == numRoundsToWin)
        {
            Time.timeScale = 0;
            logDisp.GetComponent<LogDisplay>().DebagLog("ゲーム勝利", 3);
        }

        if (getCpuWin() == numRoundsToWin)
        {
            Time.timeScale = 0;
            logDisp.GetComponent<LogDisplay>().DebagLog("ゲーム敗北", 3);
        }
    }
}
