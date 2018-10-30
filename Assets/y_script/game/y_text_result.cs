using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_text_result : MonoBehaviour {
    float time;
    public float loadtime;

    // Use this for initialization
    void Start()
    {
        
    }

    void OnEnable()
    {
        time = 0;
        string str="LOSE";
        if (y_game.winflg)
        {
            str = "WIN";
        }
        GetComponent<Text>().text = str;
    }

    // Update is called once per frame
    void Update()
    {
        ChackLoadScean();
    }

    void ChackLoadScean()
    {
        time += Time.deltaTime;
        if (time > loadtime)
        {
            SceneManager.LoadScene("select");
        }
    }
}
