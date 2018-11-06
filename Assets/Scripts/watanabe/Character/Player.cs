using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // 腕当たり判定の初期サイズ
    private Vector3 initArmColScale;
    // 状態番号(1:攻撃状態 2:回避状態 3:待機状態)
    public int state;
    // ログディスプレイ
    private LogDisplay logDisp;
    // キャラクターステータス
    private CharacterStatus status;
    // BoxCollider
    private BoxCollider boxCol;
    // 回避時間
    private float avoidanceTime = 1.0f;
    // アニメーター
    private Animator anim;
    // 腕当たり判定
    public BoxCollider armCol;
    // HPバー
    private Slider slider;


    private void Awake()
    {
        armCol = GameObject.Find("Player/ArmCol").GetComponent<BoxCollider>();
        initArmColScale = armCol.size;
        status = GetComponent<CharacterStatus>();
        boxCol = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        state = 3;

        // プレイヤーのステータスを設定
        status.SetAttack(15.0f);
        status.SetDefense(10.0f);
        status.SetAgility(10.0f);
        status.SetAttackedAngle(7.0f);
        status.SetLimitAngle(30.0f);
        status.SetHP(20.0f);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "05_Battle")
        {
            logDisp = GameObject.Find("LogDisplay").GetComponent<LogDisplay>();
            logDisp.DebagLog("PlayerHP" + status.GetHP().ToString(), 0);
            slider = GameObject.Find("Canvas/PlayerHP").GetComponent<Slider>();
            slider.value = status.GetHP();
        }

        // 状態分岐
        switch (state)
        {
            // 攻撃
            case 1:
                armCol.size = new Vector3(0.5f, 0.1f, 2.0f);
                anim.SetTrigger("Attack");
                logDisp.DebagLog("攻撃状態", 2);
                break;
            // 回避
            case 2:
                boxCol.enabled = false;
                anim.SetTrigger("Avoidance");
                StartCoroutine(ResetCol());
                logDisp.DebagLog("回避状態", 2);
                break;
            // 待機
            case 3:
                armCol.size = initArmColScale;
                logDisp.DebagLog("待機状態", 2);
                break;
        }

        // 各状態の処理終了後、待機状態で無いなら待機状態に戻す
        if (state != 3)
        {
            state = 3;
        }
    }

    // 2秒後に回避状態を解除
    IEnumerator ResetCol()
    {
        yield return new WaitForSeconds(avoidanceTime);
        boxCol.enabled = true;
        state = 3;
    }

    // ダメージ
    public void Damage(float damage)
    {
        float hp = status.GetHP();
        hp -= damage;
        status.SetHP(hp);
        slider.value = status.GetHP();
        anim.SetTrigger("Damage");
        // 負け判定
        if (status.GetHP() <= 0)
        {
            logDisp.DebagLog("PlayerHP" + status.GetHP().ToString(), 0);
            logDisp.DebagLog("負け", 3);
            gameObject.SetActive(false);
        }
    }

    // ダメージ計算
    private float CalcDamage(float attack, float defense)
    {
        float damage = attack - defense;
        return damage;
    }

    // 当たり判定
    private void OnTriggerEnter(Collider col)
    {
        // 相手に触れていたらダメージを与える
        if (col.gameObject.CompareTag("CPU"))
        {
            float attack = status.GetAttack();
            float defense = col.gameObject.GetComponent<CharacterStatus>().GetDefense();
            float damage = CalcDamage(attack, defense);
            col.gameObject.GetComponent<CPU>().Damage(damage);
        }
    }

    public void Reset()
    {
        Transform point = GameObject.Find("PlayerSpawnPoint").transform;
        transform.position = point.position;
        transform.rotation = point.rotation;
        status.SetHP(20.0f);
    }
}



//public class Player : MonoBehaviour
//{
//    // 腕の初期サイズ
//    private Vector3 initArmScale;
//    // 上半身オブジェクト
//    public GameObject upperBodyObj;
//    // 初期上半身角度
//    private Quaternion initAngle;
//    // 状態番号(1:攻撃状態 2:回避状態 3:待機状態)
//    private int state;
//    // ログディスプレイ
//    private LogDisplay logDisp;
//    // 腕関節
//    public GameObject armJoint;
//    // 初期関節角度
//    private Vector3 initArmAngle;
//    // ラウンド勝利数オブジェクト
//    private RoundWins roundWins;
//    // キャラクターステータス
//    private CharacterStatus status;
//    // BoxCollider
//    private BoxCollider boxCol;
//    // 回避時間
//    private float avoidanceTime = 1.0f;
//    // 生成位置
//    private Transform spawnPoint;


//    private void Awake()
//    {
//        logDisp = GameObject.Find("LogDisplay").GetComponent<LogDisplay>();
//        roundWins = GameObject.Find("RoundWins").GetComponent<RoundWins>();
//        status = GetComponent<CharacterStatus>();
//        boxCol = GetComponent<BoxCollider>();
//        initAngle = upperBodyObj.transform.localRotation;
//        initArmScale = armJoint.transform.localScale;
//        initArmAngle = armJoint.transform.localEulerAngles;
//        state = 3;

//        // プレイヤーのステータスを設定
//        status.SetAttack(15.0f);
//        status.SetDefense(10.0f);
//        status.SetAgility(10.0f);
//        status.SetAttackedAngle(7.0f);
//        status.SetLimitAngle(30.0f);
//    }

//    private void Update()
//    {
//        // 角度判定
//        AngleJudge();

//        // 状態分岐
//        switch (state)
//        {
//            // 攻撃
//            case 1:
//                ChangeAngle(-status.GetAttackedAngle());
//                AttackAnim();
//                logDisp.DebagLog("攻撃状態", 2);
//                break;
//            // 回避
//            case 2:
//                boxCol.enabled = false;
//                AvoidanceAnim();
//                StartCoroutine(ResetCol());
//                logDisp.DebagLog("回避状態", 2);
//                break;
//            // 待機
//            case 3:
//                armJoint.transform.localScale = initArmScale;
//                logDisp.DebagLog("待機状態", 2);
//                break;
//        }

//        // 各状態の処理終了後、待機状態で無いなら待機状態に戻す
//        if (state != 3)
//        {
//            state = 3;
//        }

//        // 徐々に初期角度に戻す
//        float speed = status.GetAgility() / 100;
//        upperBodyObj.transform.localRotation = Quaternion.Lerp(upperBodyObj.transform.localRotation, initAngle, speed * Time.deltaTime);

//        // 角度ログ
//        AngleLog();
//    }

//    // 2秒後に回避状態を解除
//    IEnumerator ResetCol()
//    {
//        yield return new WaitForSeconds(avoidanceTime);
//        boxCol.enabled = true;
//        armJoint.transform.localEulerAngles = initArmAngle;
//        state = 3;
//    }

//    // 角度変更
//    public void ChangeAngle(float amount)
//    {
//        Quaternion rot = upperBodyObj.transform.localRotation;
//        float xRot = rot.eulerAngles.x;
//        float yRot = rot.eulerAngles.y;
//        float zRot = rot.eulerAngles.z;

//        xRot += amount;
//        rot = Quaternion.Euler(xRot, yRot, zRot);
//        upperBodyObj.transform.localRotation = rot;
//    }

//    // 角度判定
//    private void AngleJudge()
//    {
//        Quaternion rot = upperBodyObj.transform.localRotation;
//        float xRot = rot.eulerAngles.x;
//        float yRot = rot.eulerAngles.y;
//        float zRot = rot.eulerAngles.z;

//        // 制限角度を超えたら負けフラグを立てる
//        if (status.GetLimitAngle() <= Quaternion.Angle(initAngle, rot))
//        {
//            logDisp.DebagLog("負け", 3);
//            gameObject.SetActive(false);
//        }
//    }

//    // 角度と大きさリセット
//    public void Reset()
//    {
//        upperBodyObj.transform.localRotation = initAngle;
//        armJoint.transform.localScale = initArmScale;
//        armJoint.transform.localEulerAngles = initArmAngle;
//    }

//    // 角度ログ
//    private void AngleLog()
//    {
//        if (upperBodyObj.transform.localEulerAngles.x >= 180)
//        {
//            float angle = upperBodyObj.transform.localEulerAngles.x - 360;
//            logDisp.DebagLog("PlayerAngle" + angle.ToString(), 0);
//        }
//        else
//        {
//            logDisp.DebagLog("PlayerAngle" + (upperBodyObj.transform.localEulerAngles.x).ToString(), 0);
//        }
//    }

//    // 攻撃アニメーション
//    private void AttackAnim()
//    {
//        // 変更後のTransform値
//        Vector3 scale = new Vector3(1.0f, 1.0f, 2.0f);

//        // 各Transformを変更
//        armJoint.transform.localScale = scale;
//    }

//    // 回避アニメーション
//    private void AvoidanceAnim()
//    {
//        // 変更後のTransform値
//        Vector3 rot = new Vector3(90.0f, 0.0f, 0.0f);

//        // 腕のTransformを変更
//        armJoint.transform.localEulerAngles = rot;
//    }

//    // ダメージ計算
//    private float CalcDamage(float attack, float defense)
//    {
//        float damage = attack - defense;
//        return damage;
//    }

//    // 当たり判定
//    private void OnTriggerEnter(Collider col)
//    {
//        // 相手に触れていたらダメージを与える
//        if (col.gameObject.CompareTag("CPU"))
//        {
//            float attack = status.GetAttack();
//            float defense = col.gameObject.GetComponent<CharacterStatus>().GetDefense();
//            float damage = CalcDamage(attack, defense);
//            col.gameObject.GetComponent<CPU>().ChangeAngle(damage);
//        }
//    }

//    // 以下ボタン処理-----------------------------------------------------------------------------------------

//    public void PushAttackButton()
//    {
//        state = 1;
//    }

//    public void PushAvoidanceButton()
//    {
//        state = 2;
//    }

//    public void AttackTypeButton()
//    {
//        status.SetAttack(30.0f);
//        status.SetDefense(10.0f);
//        status.SetAgility(10.0f);
//        logDisp.DebagLog("AttackType", 6);
//    }

//    public void DefenseTypeButton()
//    {
//        status.SetAttack(20.0f);
//        status.SetDefense(15.0f);
//        status.SetAgility(10.0f);
//        logDisp.DebagLog("DefenseType", 6);
//    }

//    public void AgilityTypeButton()
//    {
//        status.SetAttack(20.0f);
//        status.SetDefense(10.0f);
//        status.SetAgility(100.0f);
//        logDisp.DebagLog("AgilityType", 6);
//    }
//}
