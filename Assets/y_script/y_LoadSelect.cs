using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class y_LoadSelect : MonoBehaviour
{
    public static string SCENE_NAME_SELECT = "select";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadScene(int num)
    {
        y_button.area_num = num;
        SceneManager.LoadScene(SCENE_NAME_SELECT);
    }
}
