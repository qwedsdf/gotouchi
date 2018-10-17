using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_bottom : MonoBehaviour {

    const float BOTTOM = -10;
    public GameObject first_node;
    public GameObject second_node;
    public GameObject last_node;

    public float first_pos;
    public float last_pos;
    public float interval_size;

    void Start()
    {
        first_pos = first_node.transform.position.y;
        last_pos = last_node.transform.position.y;
        interval_size = Mathf.Abs(first_pos - second_node.transform.position.y);

        Debug.Log("開始場所" + first_pos);
        Debug.Log("終了地点" + last_pos);
        Debug.Log("間隔" + interval_size);
        Debug.Log("全体の" + GetAllSize());
    }
   
	void Update () {
        chack_buttom();
	}

    void chack_buttom()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, BOTTOM);
    }


    public float GetFirstPos()
    {
        return first_pos;
    }

    public float GetLastPos()
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
