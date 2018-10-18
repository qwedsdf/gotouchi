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

    void Awake()
    {
        first_pos = first_node.transform.position.y;
        last_pos = last_node.transform.position.y;
        interval_size = Mathf.Abs(first_pos - second_node.transform.position.y);
    }

    void Start()
    {
        first_pos = first_node.transform.position.y;
        last_pos = last_node.transform.position.y;
        interval_size = Mathf.Abs(first_pos - second_node.transform.position.y);

        Debug.Log("上限値" + GetMaxPosY());
        Debug.Log("下限値" + last_pos);
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
