using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


/// <summary>
/// AssetBundle管理クラス
/// </summary>
public class AssetBundleManager : SingletonMonoBehaviour<AssetBundleManager> {

    /// <summary>
    /// Android用AssetBundleの格納URL
    /// </summary>
    /// 
    private const string Url_Android = "http://www.sub0000545829.hmk-temp.com/FlipFlick/AssetBundle/Android/";



    /// <summary>
    /// iOS用AssetBundleの格納URL
    /// </summary>
    //  
    private const string Url_iOS = "http://www.sub0000545829.hmk-temp.com/FlipFlick/AssetBundle/iOS/";

    


    /// <summary>
    /// URL(プラットフォームで異なる)
    /// </summary>
    private string baseUrl;

    /// <summary>
    /// AssetBundle名込みのパス
    /// </summary>
    private string workPath;

    /// <summary>
    /// エラー発生フラグ
    /// </summary>
    private bool errFlg = false;

    /// <summary>
    /// ダウンロードするAssetBundleの総件数
    /// </summary>
    private int totalDownloadCount = 0;

    /// <summary>
    /// ダウンロードした件数
    /// </summary>
    private int downloadCount = 1;

    /// <summary>
    /// ダウンロード完了確認フラグ
    /// </summary>
    public bool DownloadCompleteFlg = false;

    /// <summary>
    /// AssetBundleのキャッシュ(一度ロードしたものを保持し続ける)
    /// </summary>
    private List<AssetBundle> assetBundleCacheList;

    // アセットバンドルは現状使用していないので、そのままメイン画面に遷移する
    void Start()
    {
        GoToSplash();
    }

    // Use this for initialization
    /*
    IEnumerator Start()
    {
        assetBundleCacheList = new List<AssetBundle>();

        // Asset BundleのURL
#if UNITY_ANDROID
        this.baseUrl = Url_Android;
#else
        this.baseUrl = Url_iOS;
#endif
        DontDestroyOnLoad(this.gameObject);
        yield return StartCoroutine(LoadAssetBundleCoroutine());
    }
    */
    #region ダウンロード

    /// <summary>
    /// AssetBundleをダウンロードする
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAssetBundleCoroutine()
    {
        AssetBundleManifest singleManifest;
        string platform = "";

#if UNITY_ANDROID
        platform = "Android";
#else
        platform = "iOS";
#endif
        // TODO: デバッグ用。毎回キャッシュをクリアしてデータをダウンロードする
        //Caching.CleanCache();

        // シングルマニフェストのロード
        using (WWW download = new WWW(this.baseUrl + platform))
        {
            yield return download;

            if (string.IsNullOrEmpty(download.error))
            {
                AssetBundleRequest request = download.assetBundle.LoadAssetAsync("AssetBundleManifest", typeof(AssetBundleManifest));
                yield return request;

                singleManifest = (AssetBundleManifest)request.asset;
            }
            else
            {
                //GameObject errorMsg = Instantiate((GameObject)(Resources.Load("Prefabs/MessageBox")));
                //errorMsg.GetComponent<MessageBoxManager>().Initialize_OK("通信エラーが発生しました。\n時間をおいて\n再度お試しください。", GoToSplash);
                SceneManager.LoadScene("FlickPuzzleMain");
                this.errFlg = true;
                yield break;
            }
        }

        // AssetBundleの総数分ダウンロードを試みる
        this.totalDownloadCount = singleManifest.GetAllAssetBundles().Length;
        foreach (string bundleName in singleManifest.GetAllAssetBundles())
        {
            this.workPath = this.baseUrl + bundleName;
            yield return StartCoroutine("LoadAssetBundle", singleManifest.GetAssetBundleHash(bundleName));
            this.downloadCount++;
        }

        if (!this.errFlg)
        {
            // 全てのダウンロードが完了した→次のシーンへ遷移する為のフラグ
            this.DownloadCompleteFlg = true;
            SceneManager.LoadScene("FlickPuzzleMain");
        }
    }

    /// <summary>
    /// 対象のAssetBundleを1つダウンロードする
    /// </summary>
    /// <param name="hash">対象のAssetBundleのハッシュ値</param>
    /// <returns></returns>
    private IEnumerator LoadAssetBundle(Hash128 hash)
    {
        while (!Caching.ready)
        {
            yield return null;
        }

        // mainシーンでのみ動く前提
        //GameObject dataCheck = GameObject.Find("Canvas").transform.Find("basePanel/DataCheckBox/Message").gameObject;
        GameObject dataCheck = GameObject.Find("Canvas").transform.Find("DataCheckBox/Message").gameObject;
        Text percent = dataCheck.transform.Find("ProgressBar/PercentText").GetComponent<Text>();
        Image bar = dataCheck.transform.Find("ProgressBar/BarImage").GetComponent<Image>();
        Text count = dataCheck.transform.Find("DownloadCount").GetComponent<Text>();
        Text check = dataCheck.transform.Find("DataCheckText").GetComponent<Text>();

        using (WWW www = WWW.LoadFromCacheOrDownload(this.workPath, hash))
        {
            while (!www.isDone)
            {
                if (www.progress == 0)
                {
                    // ダウンロード総件数のうち、いくつ目を見ているかを表示する
                    check.text = "データを確認中…";                    
                    count.text = "(" + this.downloadCount.ToString() + "/" + this.totalDownloadCount.ToString() + ")";
                }
                else
                {
                    check.text = "データをダウンロード中…";
                }

                percent.text = (Math.Truncate(www.progress * 100.0)) + "%";
                bar.fillAmount = www.progress;
                yield return null;
            }

            // ダウンロード完了まで待つ
            yield return www;

            if (www.error == null)
            {
                // Asset Bundleをキャッシュ
                this.assetBundleCacheList.Add(www.assetBundle);
            }
            else
            {
                SceneManager.LoadScene("FlickPuzzleMain");
                //GameObject errorMsg = Instantiate((GameObject)(Resources.Load("Prefabs/MessageBox")));
                //errorMsg.GetComponent<MessageBoxManager>().Initialize_OK("通信エラーが発生しました。\n時間をおいて\n再度お試しください。", GoToSplash);
                this.errFlg = true;
                yield break;
            }
        }

#if !UNITY_EDITOR && UNITY_IOS
        // iOSの場合、ダウンロードしたAssetBundleがiCloudのバックアップ対象になるのを抑止する。
		string dirPath = Application.temporaryCachePath + "/../UnityCache";
		if (System.IO.Directory.Exists(dirPath)) {
			UnityEngine.iOS.Device.SetNoBackupFlag(dirPath);
		}
#endif
    }

    #endregion

    /// <summary>
    /// タイトル画面へ遷移
    /// </summary>
    public void GoToSplash()
    {
        Destroy(this.gameObject);
        //FadeManager.Instance.LoadLevel(GameInfo.Scene_Splash, GameInfo.FadeSpeed);
        SceneManager.LoadScene("FlickPuzzleMain");
    }

    /// <summary>
    /// Asset Bundleから差し替え用の画像を取得(Texture2D)
    /// </summary>
    /// <param name="assetName">取得対象名</param>
    /// <returns>取得した画像(Texture2D)</returns>
    public Texture2D GetTextureFromAssetBundle(string assetName)
    {
        try
        {
            Texture2D tex = null;
            AssetBundle assetBundle = SearchAssetBundle(assetName);
            if (assetBundle != null)
            {
                tex = assetBundle.LoadAsset<Texture2D>(assetName);
            }
            return tex;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// Asset Bundleから画像を取得
    /// </summary>
    /// <param name="assetName">取得対象名</param>
    /// <returns>取得した画像(Sprite)</returns>
    public Sprite GetSpriteFromAssetBundle(string assetName)
    {
        try
        {
            Sprite tex = null;
            AssetBundle assetBundle = SearchAssetBundle(assetName);
            if (assetBundle != null)
            {
                tex = assetBundle.LoadAsset<Sprite>(assetName);
            }            
            return tex;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return null;
        }
    }



    /// <summary>
    /// Asset BundleからTextAsset TEXTを取得
    /// </summary>
    /// <param name="assetName">取得対象名</param>
    /// <returns>取得した画像(Sprite)</returns>
    public TextAsset GetTextAssetFromAssetBundle(string assetName)
    {
        try
        {
            //string bundleUrl = Application.streamingAssetsPath + "/Android/Level1";
            //AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundleUrl);

            //AssetBundle assetBundle = request.assetBundle;
            
            TextAsset text = null;
            AssetBundle assetBundle = SearchAssetBundle(assetName);
            if (assetBundle != null)
            {
                text = assetBundle.LoadAsset<TextAsset>(assetName);
            }

            return text;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return null;
        }
    }


    /// <summary>
    /// 引数で渡されたAsset名が存在するAssetBundleを検索する
    /// </summary>
    /// <param name="assetName">Asset名</param>
    /// <returns></returns>
    private AssetBundle SearchAssetBundle(string assetName)
    {
        AssetBundle result = null;

        foreach (AssetBundle assetBundle in this.assetBundleCacheList)
        {
            foreach (string name in assetBundle.GetAllAssetNames())
            {
                if (name.IndexOf(assetName) >= 0)
                {
                    result = assetBundle;
                    break;
                }
            }            
        }
        return result;
    }
}

