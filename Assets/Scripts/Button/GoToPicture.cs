using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPicture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadAreaSelectScene()
	{
		SceneFadeManager.Instance.Load(GameData.Scene_PictureBook, GameData.FadeSpeed);
		//SceneManager.LoadScene("select_area");
	}
}
