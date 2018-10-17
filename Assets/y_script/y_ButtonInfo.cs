﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_ButtonInfo : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetButtonNumber()
    {
        string parent_name = transform.parent.gameObject.name;
        parent_name = parent_name.Replace("bt_parent_", "");

        string name = transform.gameObject.name;
        name = name.Replace("bt_char_", "");

        int number = int.Parse(parent_name);
        number = number * 4;
        number += int.Parse(name);

        return number;
    }
}