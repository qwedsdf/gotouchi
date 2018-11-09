using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /// <summary>
    /// アニメーター
    /// </summary>
    private Animator anim;

    /// <summary>
    /// 自身のコライダー
    /// </summary>
    private BoxCollider2D boxCol2D;

    /// <summary>
    /// 腕のコライダーオブジェクト
    /// </summary>
    private GameObject armCol;

    /// <summary>
    /// 腕のコライダーオブジェクトの初期位置
    /// </summary>
    private Vector3 initArmColPos;

    /// <summary>
    /// 状態番号(1:攻撃状態 2:回避状態 3:待機状態)
    /// </summary>
    [HideInInspector]
    public int state;

    /// <summary>
    /// 対戦相手のタグ名
    /// アタッチしているオブジェクトによって設定してください。(Player or CPU)
    /// </summary>
    [HideInInspector]
    public string opponentTagName;

    /// <summary>
    /// 回避時間
    /// </summary>
    private float avoidanceTime = 1.0f;

    /// <summary>
    /// ステータススクリプト
    /// </summary>
    private Status status;


    private void Start()
    {
        // アニメーターを取得
        anim = GetComponent<Animator>();

        // 自身のコライダー取得
        boxCol2D = GetComponent<BoxCollider2D>();

        // 腕のコライダー取得
        armCol = transform.Find("ArmCol").gameObject;

        // 腕のコライダーオブジェクトの初期位置をセット
        initArmColPos = armCol.transform.localPosition;

        // 最初は待機状態からスタート
        state = 3;

        // ステータススクリプトを取得
        status = GetComponent<Status>();

        // ステータスを初期化
        status.InitStatus();
    }

    private void Update()
    {
        // 状態分岐
        switch (state)
        {
            // 攻撃
            case 1:
                anim.SetTrigger("Attack");
                armCol.transform.localPosition = initArmColPos + new Vector3(-1, 0, 0);
                break;
            // 回避
            case 2:
                anim.SetTrigger("Avoidance");
                boxCol2D.enabled = false;
                StartCoroutine(ResetCol());
                break;
            // 待機
            case 3:
                armCol.transform.localPosition = initArmColPos;
                break;
        }

        // 各状態の処理終了後、待機状態で無いなら待機状態に戻す
        if (state != 3)
        {
            state = 3;
        }
    }

    /// <summary>
    /// ダメージを受けた時
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public void Damage(float amount)
    {
        status.hp -= amount;

        // 負け判定
        if (status.hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">衝突したコライダー</param>
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag(opponentTagName))
        {
            Debug.Log(collider2D.gameObject.name + "に衝突しました。");
            float attack = status.attack;
            float defense = collider2D.GetComponent<Status>().defense;
            float amount = attack - defense;
            if (amount > 0)
            {
                collider2D.GetComponent<Controller>().Damage(amount);
            }
        }
    }

    // 2秒後に回避状態を解除
    IEnumerator ResetCol()
    {
        yield return new WaitForSeconds(avoidanceTime);
        boxCol2D.enabled = true;
        state = 3;
    }
}
