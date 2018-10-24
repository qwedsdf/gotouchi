using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_SetAspect : MonoBehaviour {
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

        camera.orthographicSize = height / 2f;

        float aspect = (float)Screen.height / (float)Screen.width;

        float ideal_aspect = height / width;

        float magnification;
        float camera_width;

        if (ideal_aspect > aspect)
        {
            magnification = height / Screen.height;

            camera_width = width / (Screen.width * magnification);

        }
        else 
        {
            magnification = width / Screen.width;

            camera_width = height / (Screen.height * magnification);
        }

        camera.rect = new Rect((1f - camera_width) / 2f, 0f, camera_width, 1f);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        SetScreenAspect();
	}
}
