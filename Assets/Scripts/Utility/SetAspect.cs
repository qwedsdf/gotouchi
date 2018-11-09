using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAspect : MonoBehaviour {
    Camera camera;

    float width = 1080f;
    float height = 1920f;

    void Awake()
    {
        SetScreenAspect();
    }

    //画面に合わせてカメラの範囲を決める
    void SetScreenAspect()
    {
        camera = GetComponent<Camera>();

        float aspect = (float)Screen.width / (float)Screen.height;

        float ideal_aspect = width / height;

        float scale = ideal_aspect / aspect;

        //カメラの描画開始位置をX座標にどのくらいずらすか
        float rectX = (1.0f - scale) / 2f;
        //カメラの描画開始位置と表示領域の設定
        camera.rect = new Rect(rectX, 0f, scale, 1f);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
