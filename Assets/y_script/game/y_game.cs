using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class y_game : MonoBehaviour {
    const float PLAYER_SIZE=1.5f;
    Rigidbody2D[] rb=new Rigidbody2D[2];

    public GameObject[] player;
    int count;
    bool[] readylflg = new bool[2];

	// Use this for initialization
	void Start () {
        init();
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
            rb[i] = player[i].GetComponent<Rigidbody2D>();
            readylflg[i] = false;
            
            //画像のサイズを見て、オブジェクトの大きさを調整
            float player_sprite_size;
            player_sprite_size = player[i].GetComponent<SpriteRenderer>().size.x;
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
        if (count % 10 == 0)
        {
            PushPlayer(1);
        }
        count++;
    }

    void Battle()
    {
        for(int i=0; i<readylflg.Length; i++){

            if (!readylflg[i])
            {
                readylflg[i] = player[i].GetComponent<y_player>().GetReadyFlg();
            }
        }
        
        EnemyAttack();
    }

    public void PushPlayer(int num)
    {
        if (!readylflg[num]) return;
        float push = 10f;
        if (num == 1) push = push * -1;
        Vector3 force = new Vector3(push, 0.0f, 0.0f);
        rb[num].AddForce(force);  
    }
}
