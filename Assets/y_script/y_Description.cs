using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_Description : MonoBehaviour {
    const float MAX_SIZE=1;
    const float MIN_SIZE = 0;
    RectTransform rec;
    public y_pictures script_pictures;
    public float growing_spd;

    static public bool active_flg = false;

    //他のスクリプトのstartで参照しているためここにいれた
    void Awake()
    {
        rec = GetComponent<RectTransform>();
    }

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        chack_scale();
	}

    void OnEnable()
    {
        SetPosition();

        Vector3 size = rec.localScale;
        size.x = 0f;
        size.y = 0f;
        rec.localScale = size;
        active_flg = true;
    }

    //表示位置を調整
    void SetPosition()
    {
        Vector3 point = new Vector3(Screen.width / 2,Screen.height / 2,0f);
        point = Camera.main.ScreenToWorldPoint(point);
        point.z = 0f;

        rec.transform.position = point;
    }

    //今の大きさを確認して調整する
    void chack_scale(){
        if (active_flg && rec.localScale.x < MAX_SIZE)
        {
            Vector3 size = rec.localScale;
            size.x += Time.deltaTime * growing_spd;
            size.y += Time.deltaTime * growing_spd;
            if (size.x > MAX_SIZE)
            {
                size.x = MAX_SIZE;
                size.y = MAX_SIZE;
            }
            rec.localScale = size;
        }
        else if (!active_flg && rec.localScale.x > MIN_SIZE)
        {
            Vector3 size = rec.localScale;
            size.x -= Time.deltaTime * growing_spd;
            size.y -= Time.deltaTime * growing_spd;
            if (size.x <= MIN_SIZE)
            {
                size.x = MIN_SIZE;
                size.y = MIN_SIZE;
                script_pictures.RefreshScollPos();
                this.gameObject.SetActive(false);
            }
            rec.localScale = size;
        }
    }
}
