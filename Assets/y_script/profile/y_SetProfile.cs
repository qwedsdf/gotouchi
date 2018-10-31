using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class y_SetProfile : MonoBehaviour {

    InputField inputField;
    y_Database DataBaseScript;

	// Use this for initialization
	void Start () {
        inputField = GetComponent<InputField>();
        DataBaseScript = GameObject.Find("Master").GetComponent<y_Database>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PushOk()
    {
        if (inputField.text == "") inputField.text = "NO NAME";
        DataBaseScript.save_data_all.playerdata.player_name = inputField.text;
        Debug.Log(DataBaseScript.save_data_all.playerdata.player_name);
    }

    public void LoadScean_SelectArea()
    {
        PushOk();
        DataBaseScript.save_data_all.playerdata.use_char = null;
        SceneManager.LoadScene("select_area");
    }
}
