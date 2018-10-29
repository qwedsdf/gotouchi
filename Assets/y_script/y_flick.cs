using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_flick : MonoBehaviour {
    //フリックで使うベクトル
    float touchStartPosX, touchEndPosX;
    const float NEXT_DISTANCE = 5f;
    public y_pictures script_button;

    bool hitflg;

	// Use this for initialization
	void Start () {
        hitflg = false;
	}
	
	// Update is called once per frame
	void Update () {
        Flick();
	}

    public void Onclikmouse()
    {
        touchStartPosX = Input.mousePosition.x;
        Debug.Log("触っている");
        hitflg = true;
    }

    //フリック
    void Flick()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPosX = Input.mousePosition.x;
            if(hitflg)FlickDirectionCheck();
        }
    }

    //どの方向にフリックしたかを検知
    void FlickDirectionCheck()
    {
        float distance = touchStartPosX - touchEndPosX;
        if (Mathf.Abs(distance) > NEXT_DISTANCE)
        {
            if (distance < 0)
            {
                script_button.bt_page(-1);
            }
            else
            {
                script_button.bt_page(1);
            }
        }
        hitflg = false;
    }
}
