using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// アバター管理クラス
/// </summary>
public class AvatarManager : SingletonMonoBehaviour<AvatarManager>
{
    /// <summary>
    /// 着せ替え可能なパーツ数
    /// </summary>
    public const int partsNum = (int)MySkinnedMeshCombiner.MAIN_PARTS.MAX;

    /// <summary>
    /// 各パーツファイルの名前
    /// </summary>
    public string rootBoneFileName = null;
    public string[] headFileNames = null;
    public string[] bodyFileNames = null;
    public string[] legFileNames = null;

    /// <summary>
    /// ロード中フラグ
    /// </summary>
    //private bool isLoading;

    /// <summary>
    /// 現在装備中の衣装ファイルの名前
    /// </summary>
    private string[] selectedFileNames = new string[partsNum];

    /// <summary>
    /// リソース読み込みリクエスト(Resources.LoadAsyncで読み込んだ返り値)
    /// </summary>
    private ResourceRequest[] resourceReqs = new ResourceRequest[partsNum];

    /// <summary>
    /// プレイヤーゲームオブジェクト
    /// </summary>
    [HideInInspector]
    public GameObject player = null;

    /// <summary>
    /// プレイヤーオブジェクトがアクティブかどうか
    /// </summary>
    private bool playerActive = false;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        Caching.ClearCache();
        Resources.UnloadUnusedAssets();
        StartCoroutine(InitAvatar());
    }

    /// <summary>
    /// 着せ替え実行
    /// </summary>
    /// <param name="partsNum"></param>
    /// <param name="value"></param>
    public void Change(int partsNum, int value)
    {
        StartCoroutine(ChangeAvatar(partsNum, value));
    }

    /// <summary>
    /// 衣装の確定
    /// </summary>
    public void ConfirmCostume()
    {
        // 着替えを確定させる(ローカルセーブデータに保存) todo yoha
        SaveData.SetClass(SaveKey.equipData, GameData.equipData);
		SaveData.Save();
        GameData.backupEquipData = GameData.equipData.Clone();
    }

    /// <summary>
    /// プレイヤーを表示
    /// </summary>
    public void EnablePlayer()
    {
        // プレイヤーが存在している　かつ　プレイヤーがアクティブでない場合、プレイヤーをアクティブにする
        if (player != null && playerActive == false)
        {
            player.SetActive(true);
            playerActive = player.activeSelf;
        }
    }

    /// <summary>
    /// プレイヤーを非表示
    /// </summary>
    public void DisablePlayer()
    {
        // プレイヤーが存在している　かつ　プレイヤーがアクティブな場合、プレイヤーを非アクティブにする
        if (player != null && playerActive == true)
        {
            player.SetActive(false);
            playerActive = player.activeSelf;
        }
    }

    /// <summary>
    /// アバターの初期化
    /// </summary>
    /// <returns></returns>
    IEnumerator InitAvatar()
    {
        GameData.equipData = SaveData.GetClass(SaveKey.equipData, new EquipData());
        GameData.backupEquipData = GameData.equipData.Clone();

        selectedFileNames[(int)MySkinnedMeshCombiner.MAIN_PARTS.HEAD] = GameData.equipData.equipHead;
        selectedFileNames[(int)MySkinnedMeshCombiner.MAIN_PARTS.BODY] = GameData.equipData.equipBody;
        selectedFileNames[(int)MySkinnedMeshCombiner.MAIN_PARTS.LEG] = GameData.equipData.equipLeg;

        yield return StartCoroutine(LoadAvatar());
        playerActive = player.activeSelf;
    }

    /// <summary>
    /// アバター切り替え
    /// </summary>
    /// <param name="partsNum"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator ChangeAvatar(int partsNum, int value)
    {
        // 装備中の衣装情報更新
        switch (partsNum)
        {
            case (int)MySkinnedMeshCombiner.MAIN_PARTS.HEAD:
                selectedFileNames[partsNum] = headFileNames[value];
                GameData.equipData.equipHead = selectedFileNames[partsNum];
                break;
            case (int)MySkinnedMeshCombiner.MAIN_PARTS.BODY:
                selectedFileNames[partsNum] = bodyFileNames[value];
                GameData.equipData.equipBody = selectedFileNames[partsNum];
                break;
            case (int)MySkinnedMeshCombiner.MAIN_PARTS.LEG:
                selectedFileNames[partsNum] = legFileNames[value];
                GameData.equipData.equipLeg = selectedFileNames[partsNum];
                break;
        }

        GameData.equipData.dropDownValue[partsNum] = value;

        yield return StartCoroutine(LoadAvatar());
    }

    /// <summary>
    /// アバターをロード、生成
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadAvatar()
    {
        //if (isLoading)
        //{
        //    Debug.Log("Now Loading!");
        //    yield break;
        //}

        // ルートボーン用のファイルを読み込む
        Debug.Log("rootBoneFileName " + rootBoneFileName);
        ResourceRequest bornReq = Resources.LoadAsync<GameObject>(rootBoneFileName);

        // 各パーツのファイルを読み込む
        for (int i = 0; i < partsNum; i++)
        {
            resourceReqs[i] = Resources.LoadAsync<Object>(selectedFileNames[i]);
        }

        // ロード待ち
        while (true)
        {
            bool isLoadEnd = true;
            for (int i = 0; i < partsNum; i++)
            {
                if (!resourceReqs[i].isDone) isLoadEnd = false;
            }

            if (isLoadEnd)
            {
                break;
            }
            yield return null;
        }

        while (!bornReq.isDone)
        {
            yield return null;
        }

        // Resourcesから必要なファイルを読み込み終わったら、空のGameObjectを生成
        GameObject root = new GameObject();
        root.transform.position = Vector3.zero;
        root.transform.localScale = Vector3.one;
        root.name = "Player";

        // 生成した空のGameObjectにSkinnedMeshCombinerを追加する（以下、Root)
        MySkinnedMeshCombiner smc = root.AddComponent<MySkinnedMeshCombiner>();

        if (bornReq.asset == null)
        {
            Debug.LogError("born asset is null");
        }

        // ルートボーン用のファイルをInstantiateする
        GameObject rootBone = (GameObject)Instantiate(bornReq.asset as GameObject);
        if (rootBone != null)
        {
            rootBone.transform.parent = root.transform;
            rootBone.transform.localPosition = Vector3.zero;
            rootBone.transform.localScale = Vector3.one;
            rootBone.transform.localRotation = Quaternion.identity;
            smc.rootBoneObject = rootBone.transform;
        }
        else
        {
            Debug.LogError("Root Bone Instantiate Error!");
            yield break;
        }

        // Rootの下に読み込んだファイル一式をInstanTiateする
        for (int i = 0; i < partsNum; i++)
        {
            GameObject obj = (GameObject)Instantiate(resourceReqs[i].asset);
            if (obj != null)
            {
                Debug.Log("[" + i + "] " + obj.name);
                obj.transform.parent = root.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;
                // 生成したモデルをRootのSkinnedMeshCombinerの各種プロパティに設定する
                smc.equipPartsObjectList[i] = obj;
            }
        }

        // コンバイン
        smc.Combine();

        // AvatarTest.playerにRootを割り当てる（古いRootは削除する）
        if (player != null)
        {
            GameObject.DestroyImmediate(player);
            player = null;
        }

        player = root;

        // プレイヤーはシーン遷移で破棄しない
        DontDestroyOnLoad(player);
    }
}
