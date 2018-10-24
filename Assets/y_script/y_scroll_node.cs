using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_scroll_node : MonoBehaviour {
    public y_bottom sc_bottom;
    public y_pictures sc_pictuers;
    float size;
    float MaxPosY;
    float MinPosY;
    float first_posY;
    public static string PARENT_COMMON_NAME = "bt_parent_";
    public static string CHAR_COMMON_NAME = "bt_char_";
    public int count;

    public static int UP = 1;
    public static int DOWN = -1;

	// Use this for initialization
	void Start () {
        count = 0;
        first_posY = transform.position.y;
        size = sc_bottom.GetAllSize() + sc_bottom.GetIntervalSize();
        MaxPosY = sc_bottom.GetMaxPosY();
        MinPosY = sc_bottom.GetMinPosY();
	}
	
	// Update is called once per frame
	void Update () {
        ChackScrollPos();
    }

    public void InitPos()
    {
        Vector3 vec = transform.position;
        vec.y = first_posY;
        transform.position = vec;
    }


    //余り出来そうな気がするからあとでやる
    public void ChackScrollPos()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 vec = GetComponent<RectTransform>().position;
        float tmp = vec.y - MaxPosY;
        string parent_name = gameObject.name;
        parent_name = parent_name.Replace(PARENT_COMMON_NAME, "");
        int num = int.Parse(parent_name);

        //てっぺんまでノードが来たら
        if (tmp > 0)
        {
            count++;
            sc_pictuers.LoadPoctureScroll(num, UP);
            vec.y = MinPosY + tmp % size;
            rect.position = vec;
        }

        //下までノードが来たら
        else if (vec.y < MinPosY)
        {
            count--;
            sc_pictuers.LoadPoctureScroll(num, DOWN);
            vec.y = MaxPosY + tmp % size;
            rect.position = vec;
        }
    }

    public int GetCount()
    {
        return count;
    }

    public void RefreshCount()
    {
        count = 0;
    }
}
