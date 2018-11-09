using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSelect : MonoBehaviour
{
    public static string SCENE_NAME_SELECT = "select";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(int num)
    {
        button.area_num = num;
        GameData.UserData.RegionId = num + 1;

        SceneFadeManager.Instance.Load(GameData.Scene_Select, GameData.FadeSpeed);
        //SceneManager.LoadScene(SCENE_NAME_SELECT);
    }
}
