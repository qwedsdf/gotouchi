using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AvatarTest : MonoBehaviour
{
    [SerializeField]
    private Dropdown[] dropDowns = new Dropdown[AvatarManager.partsNum];


    private void Start()
    {
        dropDowns[(int)MySkinnedMeshCombiner.MAIN_PARTS.HEAD].value = GameData.equipData.dropDownValue[(int)MySkinnedMeshCombiner.MAIN_PARTS.HEAD];
        dropDowns[(int)MySkinnedMeshCombiner.MAIN_PARTS.BODY].value = GameData.equipData.dropDownValue[(int)MySkinnedMeshCombiner.MAIN_PARTS.BODY];
        dropDowns[(int)MySkinnedMeshCombiner.MAIN_PARTS.LEG].value = GameData.equipData.dropDownValue[(int)MySkinnedMeshCombiner.MAIN_PARTS.LEG];
    }

    // =====================================================
    // UI Interface
    // =====================================================
    public void SelectHeadFile(Dropdown dropdown)
    {
        int idx = (int)MySkinnedMeshCombiner.MAIN_PARTS.HEAD;
        int value = dropDowns[idx].value;
        if (AvatarManager.Instance.headFileNames.Length >= value)
        {
            // 着せ替え実行
            AvatarManager.Instance.Change(idx, value);
        }
    }

    public void SelectBodyFile(Dropdown dropdown)
    {
        int idx = (int)MySkinnedMeshCombiner.MAIN_PARTS.BODY;
        int value = dropDowns[idx].value;
        if (AvatarManager.Instance.bodyFileNames.Length >= dropDowns[idx].value)
        {
            // 着せ替え実行
            AvatarManager.Instance.Change(idx, value); ;
        }
    }

    public void SelectLegFile(Dropdown dropdown)
    {
        int idx = (int)MySkinnedMeshCombiner.MAIN_PARTS.LEG;
        int value = dropDowns[idx].value;
        if (AvatarManager.Instance.legFileNames.Length >= dropDowns[idx].value)
        {
            // 着せ替え実行
            AvatarManager.Instance.Change(idx, value);
        }
    }

    /// <summary>
    /// 確定ボタンを押した時
    /// </summary>
    public void ConfirmButton()
    {
        // 1つでも衣装を変えていれば変更確認
        if (GameData.equipData.equipHead.Equals(GameData.backupEquipData.equipHead) &&
            GameData.equipData.equipBody.Equals(GameData.backupEquipData.equipBody) &&
            GameData.equipData.equipLeg.Equals(GameData.backupEquipData.equipLeg))
        {

        }
        else
        {
            GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
            msgBox.GetComponent<MessageBoxManager>().Initialize_YesNo("この衣装で\nいいですか？", AvatarManager.Instance.ConfirmCostume, null);
        }
    }

    /// <summary>
    /// 戻るボタンを押した時
    /// </summary>
    public void ReturnHome()
    {
        // 1つでも衣装を変えていれば変更確認
        if (GameData.equipData.equipHead.Equals(GameData.backupEquipData.equipHead) &&
            GameData.equipData.equipBody.Equals(GameData.backupEquipData.equipBody) &&
            GameData.equipData.equipLeg.Equals(GameData.backupEquipData.equipLeg))
        {
            GoToHome();
        }
        else
        {
            GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
            msgBox.GetComponent<MessageBoxManager>().Initialize_YesNo("この衣装で\nいいですか？", ConfirmAndGoToHome, GoToHome);
        }
    }

    /// <summary>
    /// 衣装を確定してからホームに戻る
    /// </summary>
    public void ConfirmAndGoToHome()
    {
        AvatarManager.Instance.ConfirmCostume();
        GoToHome();
    }

    /// <summary>
    /// ホームシーンに戻る
    /// </summary>
    public void GoToHome()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_Home, GameData.FadeSpeed);
    }

    /// <summary>
    /// ゲームスタートボタンを押した時
    /// </summary>
    public void GameStartButton()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_Battle, GameData.FadeSpeed);
    }

    // =======================================================

    //void Awake()
    //{
    //    Application.targetFrameRate = 60; // ターゲットフレームレートを60に設定
    //}

    //void Start()
    //{
    //    AudioManager.Instance.PlayBGM("menu");
    //    Caching.ClearCache();
    //    Resources.UnloadUnusedAssets();
    //    StartCoroutine(InitAvatar());
    //}

    //IEnumerator InitAvatar()
    //{
    //    GameData.equipData = SaveData.GetClass(SaveKey.equipData, new EquipData());
    //    GameData.backupEquipData = GameData.equipData.Clone();

    //    selectedFileNames[(int)SkinnedMeshCombiner.MAIN_PARTS.HEAD] = GameData.equipData.equipHead;
    //    selectedFileNames[(int)SkinnedMeshCombiner.MAIN_PARTS.BODY] = GameData.equipData.equipBody;
    //    selectedFileNames[(int)SkinnedMeshCombiner.MAIN_PARTS.LEG] = GameData.equipData.equipLeg;
    //    dropDowns[(int)SkinnedMeshCombiner.MAIN_PARTS.HEAD].value = GameData.equipData.dropDownValue[(int)SkinnedMeshCombiner.MAIN_PARTS.HEAD];
    //    dropDowns[(int)SkinnedMeshCombiner.MAIN_PARTS.BODY].value = GameData.equipData.dropDownValue[(int)SkinnedMeshCombiner.MAIN_PARTS.BODY];
    //    dropDowns[(int)SkinnedMeshCombiner.MAIN_PARTS.LEG].value = GameData.equipData.dropDownValue[(int)SkinnedMeshCombiner.MAIN_PARTS.LEG];
    //    yield return StartCoroutine(LoadAvatar());
    //}

    //IEnumerator LoadAvatar()
    //{
    //    if (isLoading)
    //    {
    //        Debug.Log("Now Loading!");
    //        yield break;
    //    }

    //    // ルートボーン用のファイルを読み込む
    //    Debug.Log("rootBoneFileName " + rootBoneFileName);
    //    ResourceRequest bornReq = Resources.LoadAsync<GameObject>(rootBoneFileName);

    //    // 各パーツのファイルを読み込む
    //    for (int i = 0; i < partsNum; i++)
    //    {
    //        resourceReqs[i] = Resources.LoadAsync<Object>(selectedFileNames[i]);
    //    }

    //    // ロード待ち
    //    while (true)
    //    {
    //        bool isLoadEnd = true;
    //        for (int i = 0; i < partsNum; i++)
    //        {
    //            if (!resourceReqs[i].isDone) isLoadEnd = false;
    //        }

    //        if (isLoadEnd)
    //        {
    //            break;
    //        }
    //        yield return null;
    //    }

    //    while (!bornReq.isDone)
    //    {
    //        yield return null;
    //    }

    //    // Resourcesから必要なファイルを読み込み終わったら、空のGameObjectを生成
    //    GameObject root = new GameObject();
    //    root.transform.position = Vector3.zero;
    //    root.transform.localScale = Vector3.one;
    //    root.name = "Avatar";

    //    // 生成した空のGameObjectにSkinnedMeshCombinerを追加する（以下、Root)
    //    SkinnedMeshCombiner smc = root.AddComponent<SkinnedMeshCombiner>();

    //    if (bornReq.asset == null)
    //    {
    //        Debug.LogError("born asset is null");
    //    }
    //    // ルートボーン用のファイルをInstantiateする
    //    GameObject rootBone = (GameObject)Instantiate(bornReq.asset as GameObject);
    //    if (rootBone != null)
    //    {
    //        rootBone.transform.parent = root.transform;
    //        rootBone.transform.localPosition = Vector3.zero;
    //        rootBone.transform.localScale = Vector3.one;
    //        rootBone.transform.localRotation = Quaternion.identity;
    //        smc.rootBoneObject = rootBone.transform;
    //    }
    //    else {
    //        Debug.LogError("Root Bone Instantiate Error!");
    //        yield break;
    //    }

    //    // Rootの下に読み込んだファイル一式をInstanTiateする
    //    for (int i = 0; i < partsNum; i++)
    //    {
    //        GameObject obj = (GameObject)Instantiate(resourceReqs[i].asset);
    //        if (obj != null)
    //        {
    //            Debug.Log("[" + i + "] " + obj.name);
    //            obj.transform.parent = root.transform;
    //            obj.transform.localPosition = Vector3.zero;
    //            obj.transform.localScale = Vector3.one;
    //            obj.transform.localRotation = Quaternion.identity;
    //            // 生成したモデルをRootのSkinnedMeshCombinerの各種プロパティに設定する
    //            smc.equipPartsObjectList[i] = obj;
    //        }
    //    }

    //    // レッツ・コンバイン
    //    smc.Combine();

    //    // AvatarTest.playerにRootを割り当てる（古いRootは削除する）
    //    if (player != null)
    //    {
    //        GameObject.DestroyImmediate(player);
    //        player = null;
    //    }

    //    player = root;

    //    GameObject armCol = Instantiate(armColPrefab);
    //    armCol.transform.parent = player.transform;
    //    armCol.name = "ArmCol";

    //    DontDestroyOnLoad(player);

    //    // 着替えを確定させる(ローカルセーブデータに保存)
    //    SaveData.SetClass(SaveKey.equipData, GameData.equipData);
    //    SaveData.Save();
    //    GameData.backupEquipData = GameData.equipData.Clone();
    //}
}
