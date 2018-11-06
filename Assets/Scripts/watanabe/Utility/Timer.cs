using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timeText;
    private float time;
    private float initTime;
    public GameObject logDisp;
    private bool start;


    void Start()
    {
        time = 30.0f;
        initTime = time;
    }

    void Update()
    {
        if (start)
        {
            time -= Time.deltaTime;
            timeText.text = time.ToString("F2");
        }
    }

    public void GameStart()
    {
        start = true;
    }

    public void Stop()
    {
        start = false;
    }

    public void Reset()
    {
        start = false;
        time = initTime;
        timeText.text = time.ToString("F2");
    }

    public bool TimeOut()
    {
        if (time <= 0.0f)
        {
            start = false;
            timeText.text = 0.0f.ToString();
            logDisp.GetComponent<LogDisplay>().DebagLog("引き分け", 3);
            return true;
        }
        else
        {
            return false;
        }
    }
}
