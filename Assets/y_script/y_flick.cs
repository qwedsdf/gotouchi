using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_flick : MonoBehaviour {
    //フリックで使うベクトル
    float touchStartPosX, touchEndPosX;
    const float NEXT_DISTANCE = 5f;
    public y_pictures script_button;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter()
    {
        Flick();
    }

    //フリック
    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPosX = Input.mousePosition.x;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPosX = Input.mousePosition.x;
            FlickDirectionCheck();
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
    }
}
