using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_player : MonoBehaviour {
    bool readyflg;
    public GameObject death_effect;
    public Text nametext;
    public AudioSource hit;
    public GameObject hit_effect;
    static public bool Settlementflg;
    static public y_Database DataBaseScript;

    void Awake()
    {
        DataBaseScript = GameObject.Find("Master").GetComponent<y_Database>();
        SetPlayerCharImage();
    }

	// Use this for initialization
	void Start () {
        readyflg = false;
        Settlementflg = false;
	}
	
	// Update is called once per frame
	void Update () {
        DeathChack();
	}

    void DeathChack()
    {
        if (transform.position.y < -3 && !Settlementflg)
        {
            if (name == "Player2")
            {
                y_game.winflg = true;
            }
            Settlementflg = true;
            y_game.game_mastar.GetComponent<y_game>().TextActive(true);
            y_game.audiosouce.Play();
            Instantiate(death_effect, transform.position,transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void SetPlayerCharImage()
    {
        Sprite sp = DataBaseScript.save_data_all.playerdata.use_char;
        if (name == "Player1")
        {
            nametext.text = DataBaseScript.save_data_all.playerdata.player_name;
            if (sp != null)
            {

                GetComponent<SpriteRenderer>().sprite = sp;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "feeld" && !readyflg)
        {
            readyflg = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        if (col.gameObject.tag == "Player")
        {
            hit.Play();
            Vector3 pos = transform.position;
            pos.x += y_game.PLAYER_SIZE / 2;
            Instantiate(hit_effect, pos, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "out")
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
