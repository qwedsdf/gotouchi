using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_scroll_node : MonoBehaviour {
    public y_bottom script;
    float size;
    float MaxPosY;
    float MinPosY;

	// Use this for initialization
	void Start () {
        size = script.GetAllSize() + script.GetIntervalSize();
        MaxPosY = script.GetMaxPosY();
        MinPosY = script.GetMinPosY();

        ChackScrollPos();
	}
	
	// Update is called once per frame
	void Update () {
        ChackScrollPos();
    }

    //あまりで出来そうな気がするからあとでやる
    void ChackScrollPos()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 vec = GetComponent<RectTransform>().position;
        float tmp = vec.y - MaxPosY;
        Debug.Log(tmp);
        if (tmp > 0)
        {
            Debug.Log(tmp);
            vec.y = MinPosY + tmp % size;
            rect.position = vec;
        }
        else if (vec.y < MinPosY)
        {
            vec.y = MaxPosY + tmp % size;
            rect.position = vec;
        }
    }
}
