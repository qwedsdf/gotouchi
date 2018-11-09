using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckUserChara : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Image>().sprite = Resources.Load("Textures/" + GameData.UserData.CharacterTexName, typeof(Sprite)) as Sprite;
		Debug.Log(GameData.UserData.CharacterTexName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
