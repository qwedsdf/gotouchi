//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//// ステートマシーンクラス
//public class StateMachine<T> where T : MonoBehaviour
//{
//    T owner;
//    State<T> state = null;

//    // 連想配列(キー(key)と値(value)のペアを保持しているコレクション、インデックス番号の代わりにキーを用いて、その各値にアクセスすることが出来る)
//    Hashtable stateList = new Hashtable();

//    // コンストラクタ
//    public StateMachine(T owner, GameObject obj)
//    {
//        this.owner = owner;

//        // Hashtableにステートを追加
//        stateList.Add(CharacterState.Idle, new Idle<T>(this, obj));
//        stateList.Add(CharacterState.Attack, new Attack<T>(this, obj));
//        stateList.Add(CharacterState.Avoidance, new Avoidance<T>(this));
//    }

//    // ステートを切り替えるメソッド(切り替えるステートをenumで受け取る)
//    public void ChangeState(CharacterState e)
//    {
//        if (state != null)
//        {
//            state.Exit();
//        }

//        state = (State<T>)stateList[e];
//        state.Enter(owner);
//    }

//    // 毎フレーム呼ばれるメソッド
//    public void Execute()
//    {
//        if (state != null)
//        {
//            state.Execute();
//        }
//    }

//    public void FixdExecute()
//    {
//        if (state != null)
//        {
//            state.FixedExecute();
//        }
//    }
//}


//// ステートの基底クラス
//public abstract class State<T> where T : MonoBehaviour
//{
//    protected T owner;
//    protected StateMachine<T> parent;

//    // コンストラクタ
//    public State(StateMachine<T> parent)
//    {
//        this.parent = parent;
//    }

//    // ステートに遷移する時に一度だけ呼ばれる
//    public virtual void Enter(T owner)
//    {
//        this.owner = owner;
//    }

//    // 現在のステートである間、毎フレーム呼ばれる
//    public abstract void Execute();
//    public abstract void FixedExecute();

//    // 現在のステートから他のステートに遷移する時の一度だけ呼ばれる
//    public abstract void Exit();
//}

//// 以下、状態クラス------------------------------------------------------------------------

//// 待機状態クラス
//public class Idle<T> : State<T> where T : MonoBehaviour
//{
//    // 上半身オブジェクト
//    private GameObject upperBodyObj;

//    // 上半身の初期角度
//    private Quaternion initRot;

//    // 上半身の回転速度
//    private float rotSpeed;


//    // コンストラクタ
//    public Idle(StateMachine<T> parent, GameObject obj) : base(parent)
//    {
//        upperBodyObj = obj;
//        initRot = upperBodyObj.transform.localRotation;
//    }

//    public override void Enter(T owner)
//    {
//        base.Enter(owner);
//    }

//    public override void Execute()
//    {
//        // 上半身の角度を徐々に初期角度に戻す
//        upperBodyObj.transform.localRotation = Quaternion.Lerp(upperBodyObj.transform.localRotation, initRot, rotSpeed * Time.deltaTime);

//        if (Input.GetKeyDown("return"))
//        {
//            // 攻撃状態に遷移
//            parent.ChangeState(CharacterState.Attack);
//        }
//        else if (Input.GetKeyDown("space"))
//        {
//            // 回避状態に遷移
//            parent.ChangeState(CharacterState.Avoidance);
//        }

//    }

//    public override void FixedExecute()
//    {

//    }

//    public override void Exit()
//    {

//    }
//}

//// 攻撃状態クラス
//public class Attack<T> : State<T> where T : MonoBehaviour
//{
//    // 上半身オブジェクト
//    private GameObject upperBodyObj;

//    // コンストラクタ
//    public Attack(StateMachine<T> parent, GameObject obj) : base(parent)
//    {
//        upperBodyObj = obj;
//    }

//    public override void Enter(T owner)
//    {
//        base.Enter(owner);
//        Quaternion rot = upperBodyObj.transform.localRotation;
//        float xRot = rot.eulerAngles.x;
//        float yRot = rot.eulerAngles.y;
//        float zRot = rot.eulerAngles.z;

//        // 制限角度でない場合、上半身の角度を変更
//        if (limitRot >= Quaternion.Angle(initRot, rot))
//        {
//            xRot -= rotAmount;
//            rot = Quaternion.Euler(xRot, yRot, zRot);
//            myUpperBodyObj.transform.localRotation = rot;
//        }
//        else
//        {
//            // 負け判定
//            Destroy(this.gameObject);
//        }
//    }

//    public override void Execute()
//    {

//    }

//    public override void FixedExecute()
//    {

//    }

//    public override void Exit()
//    {

//    }
//}

//// 回避状態クラス
//public class Avoidance<T> : State<T> where T : MonoBehaviour
//{
//    // コンストラクタ
//    public Avoidance(StateMachine<T> parent) : base(parent)
//    {

//    }

//    public override void Enter(T owner)
//    {
//        base.Enter(owner);
//    }

//    public override void Execute()
//    {

//    }

//    public override void FixedExecute()
//    {

//    }

//    public override void Exit()
//    {

//    }
//}

