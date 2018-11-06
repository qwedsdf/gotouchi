using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_game : MonoBehaviour {
    static public float PLAYER_SIZE = 1.0f;
    const float PUSH_POWWER = 15f;
    Rigidbody2D[] rb=new Rigidbody2D[2];
    static public bool winflg = false;
    static public int PLAYERNUM = 0;
    static public int ENEMYNUM = 1;
    public GameObject text;
    static public GameObject game_mastar;
    public GameObject[] player;
    static public AudioSource audiosouce;
    public AudioSource bgm;
    int count;
    bool[] readylflg = new bool[2];

	// Use this for initialization
	void Start () {
        init();
        game_mastar = this.gameObject;
        text.SetActive(false);
        audiosouce = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Battle();
	}

    void init()
    {
        count = 0;
        for (int i = 0; i < player.Length; i++)
        {
            if (i == ENEMYNUM)
            {
                player[i].GetComponent<SpriteRenderer>().sprite = y_button.now_battle_chardate.img;
            }
            rb[i] = player[i].GetComponent<Rigidbody2D>();
            readylflg[i] = false;
            
            //画像のサイズを見て、オブジェクトの大きさを調整
            float player_sprite_size;
            player_sprite_size = player[i].GetComponent<SpriteRenderer>().bounds.size.x;
            Debug.Log("プレイヤー"+i+" " + player_sprite_size);
            float magnification = PLAYER_SIZE / player_sprite_size;
            Vector3 localsize = player[i].transform.localScale;
            localsize.x *= magnification;
            localsize.y *= magnification;
            player[i].transform.localScale = localsize;

            //コリジョンの大きさを調整
            Vector2 collision_size = player[i].GetComponent<BoxCollider2D>().size;
            collision_size.x = player_sprite_size;
            collision_size.y = player_sprite_size;
            player[i].GetComponent<BoxCollider2D>().size = collision_size;
        }
    }

    void EnemyAttack()
    {
        if (count % 15 == 0 && !y_player.Settlementflg)
        {
            PushPlayer(1);
        }
        count++;
    }

    void Battle()
    {
        if (y_player.Settlementflg)
        {
            bgm.Stop();
        }
        for(int i=0; i<readylflg.Length; i++){

            if (!readylflg[i])
            {
                readylflg[i] = player[i].GetComponent<y_player>().GetReadyFlg();
            }
        }
        
        EnemyAttack();
    }
    //テキストを表示
    public void TextActive(bool flg)
    {
        text.SetActive(flg);
    }

    //ボタンを押すとキャラを押す
    public void PushPlayer(int num)
    {
        if (!readylflg[num]) return;
        float push = PUSH_POWWER;
        if (num == 1) push = push * -1;
        Vector3 force = new Vector3(push, 0.0f, 0.0f);
        rb[num].AddForce(force);  
    }
}
