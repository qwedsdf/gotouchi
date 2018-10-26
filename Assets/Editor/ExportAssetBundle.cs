using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// AssetBundleのExport
/// </summary>
public class ExportAssetbundle
{
    [MenuItem("Export/Assetbundle")]
    static void Export()
    {
#if UNITY_ANDROID
        // フォルダ(なければ)作成
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Android");
        // Android用
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Android", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
#elif UNITY_IOS
        Directory.CreateDirectory(Application.streamingAssetsPath + "/iOS");
        // iOS用(勝手にSwitch PlatFormされるので注意！)
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/iOS", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
#endif
    }
}