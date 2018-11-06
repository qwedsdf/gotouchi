using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_bottom : MonoBehaviour {

    public static float BOTTOM = -5;
    public GameObject first_node;
    public GameObject second_node;
    public GameObject last_node;

    public float first_pos;
    public float last_pos;
    public float interval_size;

    public static bool scroll_flg;

    void Awake()
    {
        first_pos = first_node.transform.position.y;
        last_pos = last_node.transform.position.y;
        interval_size = Mathf.Abs(first_pos - second_node.transform.position.y);
    }

    void Start()
    {
        scroll_flg = true;
        first_pos = first_node.transform.position.y;
        last_pos = last_node.transform.position.y;
        interval_size = Mathf.Abs(first_pos - second_node.transform.position.y);
    }
   
	void Update () {
        chack_buttom();
	}

    void chack_buttom()
    {
        if (scroll_flg)
        {
            gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, BOTTOM);
        }
    }


    public float GetMaxPosY()
    {
        return first_pos + interval_size;
    }

    public float GetMinPosY()
    {
        return last_pos;
    }

    public float GetIntervalSize()
    {
        return interval_size;
    }

    public float GetAllSize()
    {
        return Mathf.Abs(first_pos-last_pos);
    }
}
