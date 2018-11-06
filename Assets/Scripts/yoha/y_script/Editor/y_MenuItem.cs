using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class y_MenuItem : MonoBehaviour {
    private const string sourceDirectory = "Assets/Resources/DataChar/area0/";
    private const string DataFolderPath = "Assets/StreamingAssets/DataChar/area0/";

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        // Android用
        BuildPipeline.BuildAssetBundles(DataFolderPath, BuildAssetBundleOptions.None, BuildTarget.Android);
    }


    //[MenuItem("Assets/Build AssetBundles")]
    //static void BuildAssetBundles()
    //{
    //    //sourceディレクトリの全てのファイル・フォルダを取得(サブフォルダの内部までは見ない)
    //    //sourceフォルダに入れたものは全てAssetBundle化対象
    //    string[] srcFilesPath = Directory.GetFileSystemEntries(sourceDirectory);
    //    foreach (string srcFilePath in srcFilesPath)
    //    {
    //        //ファイルの名前を取得し、AssetBundleの名前に設定する
    //        string fileName = Path.GetFileNameWithoutExtension(srcFilePath);
    //        AssetImporter importer = AssetImporter.GetAtPath(srcFilePath);
    //        if (importer != null)
    //        {
    //            importer.SetAssetBundleNameAndVariant(fileName, "");
    //        }
    //    }
    //    //sourceディレクトリのAssetBundle対象化を解除
    //    foreach (string srcPath in srcFilesPath)
    //    {
    //        AssetImporter importer = AssetImporter.GetAtPath(srcPath);
    //        if (importer != null)
    //        {
    //            importer.SetAssetBundleNameAndVariant("", "");
    //        }
    //    }

    //}
}
