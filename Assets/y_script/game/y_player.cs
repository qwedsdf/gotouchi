using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_player : MonoBehaviour {
    bool readyflg;

	// Use this for initialization
	void Start () {
        readyflg = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "feeld")
        {
            readyflg = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        
        if (col.gameObject.tag == "out")
        {
            readyflg = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "out")
        {
            readyflg = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    public bool GetReadyFlg()
    {
        return readyflg;
    }
}
