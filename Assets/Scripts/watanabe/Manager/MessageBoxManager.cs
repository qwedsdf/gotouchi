using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageBoxManager : MonoBehaviour
{
    /// <summary>
    /// ダイアログオブジェクト
    /// </summary>
    private GameObject dialog;

    /// <summary>
    /// 「はい」を押下した時に実行するメソッド
    /// </summary>
    public UnityAction yesEvent;

    /// <summary>
    /// 「いいえ」を押下した時に実行するメソッド
    /// </summary>
    public UnityAction noEvent;

    /// <summary>
    /// 「OK」を押下した時に実行するメソッド
    /// </summary>
    public UnityAction okEvent;

    /// <summary>
    /// ボタン押下時に実行する処理
    /// </summary>
    [SerializeField]
    private UnityEvent events;

    /// <summary>
    /// アニメーター
    /// </summary>
    //Animator animator;


    private void Awake()
    {
        this.dialog = this.gameObject.transform.Find("Message").gameObject;
        //this.animator = this.dialog.GetComponent<Animator>();
    }

    void Start()
    {
        this.events = new UnityEvent();

        this.dialog.transform.Find("YesButton").gameObject.GetComponent<Button>().onClick.AddListener(YesButton_OnClick);
        this.dialog.transform.Find("NoButton").gameObject.GetComponent<Button>().onClick.AddListener(NoButton_OnClick);
        this.dialog.transform.Find("OKButton").gameObject.GetComponent<Button>().onClick.AddListener(OKButton_OnClick);
    }

    /// <summary>
    /// 初期化処理(はい／いいえ)
    /// </summary>
    /// <param name="message">ダイアログに表示するメッセージ</param>
    /// <param name="yesEvent">「はい」押下時に起動するメソッド</param>
    /// <param name="noEvent">「いいえ」押下時に起動するメソッド</param>
    public void Initialize_YesNo(string message, UnityAction yesEvent, UnityAction noEvent)
    {
        this.dialog = this.gameObject.transform.Find("Message").gameObject;
        //this.animator = this.dialog.GetComponent<Animator>();

        SetButtonType(1);
        SetMessage(message);

        this.yesEvent = yesEvent;
        this.noEvent = noEvent;
    }

    /// <summary>
    /// 初期化処理(任意の2択)
    /// </summary>
    /// <param name="leftButtonName">左のボタン名</param>
    /// <param name="rightButtonName">右のボタン名ージ</param>
    /// <param name="message">ダイアログに表示するメッセージ</param>
    /// <param name="yesEvent">左のボタン押下時に起動するメソッド</param>
    /// <param name="noEvent">右のボタン押下時に起動するメソッド</param>
    public void Initialize_Select(string leftButtonName, string rightButtonName, string message, UnityAction yesEvent, UnityAction noEvent)
    {
        this.gameObject.transform.Find("Message/YesButton/Text").GetComponent<Text>().text = leftButtonName;
        this.gameObject.transform.Find("Message/NoButton/Text").GetComponent<Text>().text = rightButtonName;
        Initialize_YesNo(message, yesEvent, noEvent);
    }

    /// <summary>
    /// 初期化処理(OK)
    /// </summary>
    /// <param name="message">ダイアログに表示するメッセージ</param>
    /// <param name="okEvent">「OK」押下時に起動するメソッド</param>
    public void Initialize_OK(string message, UnityAction okEvent)
    {
        SetButtonType(2);
        SetMessage(message);

        this.okEvent = okEvent;
    }

    /// <summary>
    /// ボタンのタイプを設定する
    /// </summary>
    /// <param name="type">タイプ</param>
    private void SetButtonType(int type)
    {
        switch (type)
        {
            case 1:
                // はい／いいえ を表示
                this.dialog.transform.Find("YesButton").gameObject.SetActive(true);
                this.dialog.transform.Find("NoButton").gameObject.SetActive(true);
                this.dialog.transform.Find("OKButton").gameObject.SetActive(false);
                break;
            case 2:
                // OK を表示
                this.dialog.transform.Find("YesButton").gameObject.SetActive(false);
                this.dialog.transform.Find("NoButton").gameObject.SetActive(false);
                this.dialog.transform.Find("OKButton").gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// メッセージをセットする
    /// </summary>
    /// <param name="message">内容</param>
    private void SetMessage(string message)
    {
        this.dialog.transform.Find("Text").gameObject.GetComponent<Text>().text = message;
    }

    /// <summary>
    /// 「はい」ボタン押下時
    /// </summary>
    private void YesButton_OnClick()
    {
        if (this.yesEvent != null)
        {
            this.events.AddListener(this.yesEvent);
            this.events.Invoke();
        }

        StartCoroutine("CloseWindow");
    }

    /// <summary>
    /// 「いいえ」ボタン押下時
    /// </summary>
    private void NoButton_OnClick()
    {
        if (this.noEvent != null)
        {
            this.events.AddListener(this.noEvent);
            this.events.Invoke();
        }

        StartCoroutine("CloseWindow");
    }

    /// <summary>
    /// 「OK」ボタン押下時
    /// </summary>
    private void OKButton_OnClick()
    {
        if (this.okEvent != null)
        {
            this.events.AddListener(this.okEvent);
            this.events.Invoke();
        }

        StartCoroutine("CloseWindow");
    }

    /// <summary>
    /// メッセージボックスを閉じる
    /// </summary>
    /// <returns></returns>
    public IEnumerator CloseWindow()
    {
        // アニメーションを逆再生
        //this.animator.SetFloat("Speed", -1);
        //this.animator.Play(Animator.StringToHash("messageBox"), 0, 1.0f);

        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);
    }
}
