using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_scroll : MonoBehaviour {

    [SerializeField]
    RectTransform prefab = null;

    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            var item = GameObject.Instantiate(prefab) as RectTransform;
            item.SetParent(transform, false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
