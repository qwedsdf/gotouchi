using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// サウンド関連
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{

    /// <summary>
    /// BGMキー名
    /// </summary>
    public string[] bgmKey;

    /// <summary>
    /// SEキー名
    /// </summary>
    public string[] seKey;

    /// <summary>
    /// BGM音源
    /// </summary>
    public AudioClip[] bgmFiles;

    /// <summary>
    /// SE音源
    /// </summary>
    public AudioClip[] seFiles;

    /// <summary>
    /// BGM再生オブジェクト
    /// </summary>
    public AudioSource bgmSource;

    /// <summary>
    /// SE再生オブジェクト
    /// </summary>
    public AudioSource seSource;



    public int BgmSetting = 0;

    /// <summary>
    /// BGM一覧
    /// </summary>
    private Dictionary<string, AudioClip> BGMList = new Dictionary<string, AudioClip>();

    /// <summary>
    /// SE一覧
    /// </summary>
    private Dictionary<string, AudioClip> SEList = new Dictionary<string, AudioClip>();

    // Use this for initialization
    public void Start()
    {
        this.seSource = this.gameObject.AddComponent<AudioSource>();

        // BGM一覧
        for (int i = 0; i < this.bgmKey.Length; i++)
        {
            this.BGMList.Add(this.bgmKey[i], this.bgmFiles[i]);
        }

        // SE一覧
        for (int j = 0; j < this.seKey.Length; j++)
        {
            this.SEList.Add(this.seKey[j], this.seFiles[j]);
        }
    }

    /// <summary>
    /// 指定したBGMを再生する
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayBGM(string audioName)
    {
        // 再生中のBGMと指定されたBGMが異なる場合、BGMを変更する
        if (this.bgmSource.clip != this.BGMList[audioName])
        {
            this.bgmSource.Stop();
            this.bgmSource.clip = BGMList[audioName];

            if (this.BgmSetting == 0)
            {
                this.bgmSource.Play();
            }
        }
    }

    /// <summary>
    /// 指定したSEを再生する
    /// </summary>
    /// <param name="audioName"></param>
    public void PlaySE(string audioName)
    {
        if (this.BgmSetting == 0)
        {

            this.seSource.PlayOneShot(SEList[audioName]);
        }
    }

    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public void StopBGM()
    {
        this.bgmSource.Stop();
        this.bgmSource.clip = null;
    }
}
