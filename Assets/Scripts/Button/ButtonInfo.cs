using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetButtonNumber()
    {
        GameObject parent = transform.parent.gameObject;

        string parent_name = parent.name;
        parent_name = parent_name.Replace(scroll_node.PARENT_COMMON_NAME, "");

        string name = transform.gameObject.name;
        name = name.Replace(scroll_node.CHAR_COMMON_NAME, "");

        int number = int.Parse(parent_name);
        number = number * 4;
        number += int.Parse(name);

        number += parent.GetComponent<scroll_node>().GetCount() * pictures.MAX_BUTTON_NUM;

        return number;
    }
}
