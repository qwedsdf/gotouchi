using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

/// <summary>
/// セーブデータ管理クラス
/// </summary>
public class SaveData : SingletonMonoBehaviour<SaveData>
{
    /// <summary>
    /// AES暗号化のパスワード
    /// </summary>
    private const string PASSWORD = "prolead";

    /// <summary>
    /// セーブデータのファイル名
    /// </summary>
    private const string SaveFileName = "savedata.json";

    /// <summary>
    /// Baseクラス(static)
    /// </summary>
    private static SaveDataBase savedatabase = null;

    /// <summary>
    /// セーブデータ統括
    /// </summary>
    private static SaveDataBase Savedatabase
    {
        get
        {
            if (savedatabase == null)
            {
                string path = "";
#if UNITY_EDITOR                
                path = Application.persistentDataPath;
#elif UNITY_ANDROID
                // Androidの実機でのみ動く
                path = GetActivityFileDir();
#elif UNITY_IOS
                path = Application.persistentDataPath;
#endif
                savedatabase = new SaveDataBase(path + "/", SaveFileName);
#if UNITY_IOS
                // iCloudにバックアップされるのを抑止する
                UnityEngine.iOS.Device.SetNoBackupFlag(path + "/" + SaveFileName);
#endif
            }
            return savedatabase;
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private SaveData()
    {

    }

    /// <summary>
    /// Android用 一般的にアクセス不可のアプリ内部パスを取得する
    /// </summary>
    /// <returns>パス</returns>
    private static string GetActivityFileDir()
    {
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentAtivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject file = currentAtivity.Call<AndroidJavaObject>("getFilesDir"))
        {
            return file.Call<string>("getCanonicalPath");
        }
    }

    #region string型のSet/Get

    /// <summary>
    /// string型のデータを保存する
    /// </summary>
    /// <param name="key">任意のキー</param>
    /// <param name="value">任意の値</param>
    public static void SetString(string key, string value)
    {
        Savedatabase.SetString(key, value);
    }

    /// <summary>
    /// string型で保存されたデータを取得する
    /// </summary>
    /// <param name="key">保存時に指定したキー</param>
    /// <param name="_default">取得できなかった場合のデフォルト値</param>
    /// <returns>取得したデータ</returns>
    public static string GetString(string key, string _default = "")
    {
        return Savedatabase.GetString(key, _default);
    }

    #endregion

    #region int型のSet/Get

    /// <summary>
    /// int型のデータを保存する
    /// </summary>
    /// <param name="key">任意のキー</param>
    /// <param name="value">任意の値</param>
    public static void SetInt(string key, int value)
    {
        Savedatabase.SetInt(key, value);
    }

    /// <summary>
    /// int型で保存されたデータを取得する
    /// </summary>
    /// <param name="key">保存時に指定したキー</param>
    /// <param name="_default">取得できなかった場合のデフォルト値</param>
    /// <returns>取得したデータ</returns>
    public static int GetInt(string key, int _default = 0)
    {
        return Savedatabase.GetInt(key, _default);
    }

    #endregion

    #region float型のSet/Get

    /// <summary>
    /// float型のデータを保存する
    /// </summary>
    /// <param name="key">任意のキー</param>
    /// <param name="value">任意の値</param>
    public static void SetFloat(string key, float value)
    {
        Savedatabase.SetFloat(key, value);
    }

    /// <summary>
    /// float型で保存されたデータを取得する
    /// </summary>
    /// <param name="key">保存時に指定したキー</param>
    /// <param name="_default">取得できなかった場合のデフォルト値</param>
    /// <returns></returns>
    public static float GetFloat(string key, float _default = 0.0f)
    {
        return Savedatabase.GetFloat(key, _default);
    }

    #endregion

    #region List<T>型のSet/Get

    /// <summary>
    /// List<T>型のデータを保存する
    /// </summary>
    /// <typeparam name="T">任意の型</typeparam>
    /// <param name="key">任意のキー</param>
    /// <param name="list">任意の値</param>
    public static void SetList<T>(string key, List<T> list)
    {
        Savedatabase.SetList<T>(key, list);
    }

    /// <summary>
    /// List<T>型で保存されたデータを取得する
    /// </summary>
    /// <typeparam name="T">取得したいデータに合わせた任意の型</typeparam>
    /// <param name="key">保存時に指定したキー</param>
    /// <param name="_default">取得できなかった時のデフォルト値</param>
    /// <returns>取得したデータ</returns>
    public static List<T> GetList<T>(string key, List<T> _default)
    {
        return Savedatabase.GetList<T>(key, _default);
    }

    #endregion

    #region 独自クラスのSet/Get

    /// <summary>
    /// 独自クラスのデータを保存する
    /// </summary>
    /// <typeparam name="T">任意の型</typeparam>
    /// <param name="key">任意のキー</param>
    /// <param name="_default">保存したい独自クラスオブジェクト</param>
    /// <returns></returns>
    public static T GetClass<T>(string key, T _default) where T : class, new()
    {
        return Savedatabase.GetClass(key, _default);
    }

    /// <summary>
    /// 独自クラスのデータを取得する
    /// </summary>
    /// <typeparam name="T">取得したいデータに合わせた任意の型</typeparam>
    /// <param name="key">保存時に指定したキー</param>
    /// <param name="obj">取得できなかった時のデフォルト値</param>
    public static void SetClass<T>(string key, T obj) where T : class, new()
    {
        Savedatabase.SetClass<T>(key, obj);
    }

    #endregion

    #region データの削除

    /// <summary>
    /// 保存されたデータを全て消去する
    /// </summary>
    public static void Clear()
    {
        Savedatabase.Clear();
    }

    /// <summary>
    /// 保存された特定のデータを削除する
    /// </summary>
    /// <param name="key">削除したいデータのキー</param>
    public static void Remove(string key)
    {
        Savedatabase.Remove(key);
    }

    #endregion

    #region キー存在チェック・キー一覧取得

    /// <summary>
    /// 指定したキーが使用されているかチェックする
    /// </summary>
    /// <param name="_key">存在を確認したいキー</param>
    /// <returns>true:存在している, false:存在していない</returns>
    public static bool ContainsKey(string _key)
    {
        return Savedatabase.ContainsKey(_key);
    }

    /// <summary>
    /// 保存されているキーの一覧を取得する
    /// </summary>
    /// <returns>セーブデータとして保存されているデータのキー一覧</returns>
    public static List<string> Keys()
    {
        return Savedatabase.Keys();
    }

    #endregion

    #region 明示的な保存

    /// <summary>
    /// 明示的な保存
    /// </summary>
    public static void Save()
    {
        Savedatabase.Save();
    }

    #endregion

    #region Base Class

    /// <summary>
    /// セーブデータ管理のベースクラス
    /// </summary>
    [Serializable]
    private class SaveDataBase
    {
        #region フィールド

        /// <summary>
        /// セーブデータファイルの保存先
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// セーブデータファイル名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// セーブデータとして格納するデータ(key, value)
        /// </summary>
        private Dictionary<string, string> saveDictionary;

        #endregion

        #region コンストラクタ／デストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_path">保存先</param>
        /// <param name="_fileName">ファイル名</param>
        public SaveDataBase(string _path, string _fileName)
        {
            this.Path = _path;
            this.FileName = _fileName;
            this.saveDictionary = new Dictionary<string, string>();
            Load();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SaveDataBase()
        {
            // ゲーム終了時に保存する
            Save();
        }

        #endregion

        #region セーブデータのロード／セーブ

        /// <summary>
        /// セーブデータのロード
        /// </summary>
        public void Load()
        {
            string filePath = this.Path + this.FileName;

            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8")))
                    {
                        if (this.saveDictionary != null)
                        {
                            Serialization<string, string> sDict = JsonUtility.FromJson<Serialization<string, string>>(Decrypt(sr.ReadToEnd()));
                            sDict.OnAfterDeserialize();
                            this.saveDictionary = sDict.ToDictionary();
                        }
                    }
                }
                else
                {
                    this.saveDictionary = new Dictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                // プロジェクトごとのエラー処理
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
                msgBox.GetComponent<MessageBoxManager>().Initialize_OK("セーブデータのロードに失敗しました。\n" + ex.Message + "\n" + ex.StackTrace, null);
            }
        }

        /// <summary>
        /// セーブデータの保存
        /// </summary>
        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(this.Path + this.FileName, false, Encoding.GetEncoding("utf-8")))
                {
                    Serialization<string, string> serialDict = new Serialization<string, string>(this.saveDictionary);
                    serialDict.OnBeforeSerialize();

                    // 暗号化して保存する
                    string dictJsonString = Encrypt(JsonUtility.ToJson(serialDict));
                    writer.WriteLine(dictJsonString);
                }
            }
            catch (Exception ex)
            {
                // プロジェクトごとのエラー処理
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
                msgBox.GetComponent<MessageBoxManager>().Initialize_OK("セーブデータの保存に失敗しました。\n" + ex.Message + "\n" + ex.StackTrace, null);
            }
        }

        #endregion

        #region string型のSet/Get

        /// <summary>
        /// string型のデータを保存する
        /// </summary>
        /// <param name="key">任意のキー</param>
        /// <param name="value">任意の値</param>
        public void SetString(string key, string value)
        {
            keyCheck(key);

            this.saveDictionary[key] = value;
        }

        /// <summary>
        /// string型で保存されたデータを取得する
        /// </summary>
        /// <param name="key">保存時に指定したキー</param>
        /// <param name="_default">取得できなかった場合のデフォルト値</param>
        /// <returns>取得したデータ</returns>
        public string GetString(string key, string _default)
        {
            keyCheck(key);

            if (!this.saveDictionary.ContainsKey(key))
            {
                return _default;
            }

            return this.saveDictionary[key];
        }

        #endregion

        #region int型のSet/Get

        /// <summary>
        /// int型のデータを保存する
        /// </summary>
        /// <param name="key">任意のキー</param>
        /// <param name="value">任意の値</param>
        public void SetInt(string key, int value)
        {
            keyCheck(key);

            this.saveDictionary[key] = value.ToString();
        }

        /// <summary>
        /// int型で保存されたデータを取得する
        /// </summary>
        /// <param name="key">保存時に指定したキー</param>
        /// <param name="_default">取得できなかった場合のデフォルト値</param>
        /// <returns>取得したデータ</returns>
        public int GetInt(string key, int _default)
        {
            keyCheck(key);

            if (!this.saveDictionary.ContainsKey(key))
            {
                return _default;
            }

            int ret = 0;
            int.TryParse(saveDictionary[key], out ret);

            return ret;
        }

        #endregion

        #region Float型のSet/Get

        /// <summary>
        /// float型のデータを保存する
        /// </summary>
        /// <param name="key">任意のキー</param>
        /// <param name="value">任意の値</param>
        public void SetFloat(string key, float value)
        {
            keyCheck(key);

            this.saveDictionary[key] = value.ToString();
        }

        /// <summary>
        /// float型で保存されたデータを取得する
        /// </summary>
        /// <param name="key">保存時に指定したキー</param>
        /// <param name="_default">取得できなかった場合のデフォルト値</param>
        /// <returns>取得したデータ</returns>
        public float GetFloat(string key, float _default)
        {
            keyCheck(key);

            if (!this.saveDictionary.ContainsKey(key))
            {
                return _default;
            }

            float ret = 0.0f;
            float.TryParse(this.saveDictionary[key], out ret);

            return ret;
        }

        #endregion

        #region List<T>型のSet/Get

        /// <summary>
        /// List<T>型のデータを保存する
        /// </summary>
        /// <typeparam name="T">任意の型</typeparam>
        /// <param name="key">任意のキー</param>
        /// <param name="list">保存したいList<T>型オブジェクト</param>
        public void SetList<T>(string key, List<T> list)
        {
            keyCheck(key);

            Serialization<T> serializableList = new Serialization<T>(list);
            this.saveDictionary[key] = JsonUtility.ToJson(serializableList);
        }

        /// <summary>
        /// List<T>型で保存されたデータを取得する
        /// </summary>
        /// <typeparam name="T">取得したいデータに合わせた任意の型</typeparam>
        /// <param name="key">保存時に指定したキー</param>
        /// <param name="_default">取得できなかった時のデフォルト値</param>
        /// <returns>取得したデータ</returns>
        public List<T> GetList<T>(string key, List<T> _default)
        {
            keyCheck(key);

            if (!this.saveDictionary.ContainsKey(key))
            {
                return _default;
            }
            Serialization<T> deserializeList = JsonUtility.FromJson<Serialization<T>>(this.saveDictionary[key]);

            return deserializeList.ToList();
        }

        #endregion

        #region 独自クラスのSet/Get

        /// <summary>
        /// 独自クラスのデータを保存する
        /// </summary>
        /// <typeparam name="T">任意の型</typeparam>
        /// <param name="key">任意のキー</param>
        /// <param name="obj">保存したい独自クラスオブジェクト</param>
        public void SetClass<T>(string key, T obj) where T : class, new()
        {
            try
            {
                keyCheck(key);

                this.saveDictionary[key] = JsonUtility.ToJson(obj);
            }
            catch (Exception ex)
            {
                // プロジェクトごとのエラー処理
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
                msgBox.GetComponent<MessageBoxManager>().Initialize_OK("JSON化に失敗しました。\n" + ex.Message + "\n" + ex.StackTrace, null);
            }
        }

        /// <summary>
        /// 独自クラスのデータを取得する
        /// </summary>
        /// <typeparam name="T">取得したいデータに合わせた任意の型</typeparam>
        /// <param name="key">保存時に指定したキー</param>
        /// <param name="_default">取得できなかった時のデフォルト値</param>
        /// <returns>取得したデータ</returns>
        public T GetClass<T>(string key, T _default) where T : class, new()
        {
            T obj = null;

            try
            {
                keyCheck(key);

                if (!this.saveDictionary.ContainsKey(key))
                {
                    return _default;
                }
                obj = JsonUtility.FromJson<T>(this.saveDictionary[key]);
            }
            catch (Exception ex)
            {
                // プロジェクトごとのエラー処理
                GameObject msgBox = (GameObject)Instantiate((GameObject)Resources.Load("Prefabs/MessageBox"));
                msgBox.GetComponent<MessageBoxManager>().Initialize_OK("JSONデータの復元に失敗しました。\n" + ex.Message + "\n" + ex.StackTrace, null);
            }
            return obj;
        }

        #endregion

        #region データの削除

        /// <summary>
        /// 保存されたデータを全て消去する
        /// </summary>
        public void Clear()
        {
            this.saveDictionary.Clear();
        }

        /// <summary>
        /// 保存された特定のデータを削除する
        /// </summary>
        /// <param name="key">削除したいデータのキー</param>
        public void Remove(string key)
        {
            keyCheck(key);

            if (this.saveDictionary.ContainsKey(key))
            {
                this.saveDictionary.Remove(key);
            }
        }

        #endregion

        #region キー存在チェック・キー一覧取得

        /// <summary>
        /// keyチェック
        /// </summary>
        private void keyCheck(string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentException("invalid key!!");
            }
        }

        /// <summary>
        /// 指定したキーが使用されているかチェックする
        /// </summary>
        /// <param name="_key">存在を確認したいキー</param>
        /// <returns>true:存在している, false:存在していない</returns>
        public bool ContainsKey(string _key)
        {
            return this.saveDictionary.ContainsKey(_key);
        }

        /// <summary>
        /// 保存されているキーの一覧を取得する
        /// </summary>
        /// <returns>セーブデータとして保存されているデータのキー一覧</returns>
        public List<string> Keys()
        {
            return this.saveDictionary.Keys.ToList<string>();
        }

        #endregion

        #region 暗号化／複合化

        /// <summary>
        /// 暗号化アルゴリズムの作成
        /// </summary>
        /// <param name="password">パスワード</param>
        public static RijndaelManaged CreateEncryptAlgorithm(string password)
        {
            if (password == null)
            {
                return null;
            }

            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;

            // キーと初期化ベクタを生成する
            byte[] salt = Encoding.UTF8.GetBytes("KwdWYigoUcMxfQnf");

            // JSSECガイドライン 5.6.2.5. パスワードから鍵を生成する場合は、適正なハッシュの繰り返し回数を指定する
            // 一般に 1,000 回以上の繰り返しであれば良いとされる。
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, 1079);
            aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
            aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);

            return aes;
        }

        /// <summary>
        /// AES64による暗号化
        /// </summary>
        /// <param name="text">暗号化対象文字列</param>
        /// <returns>暗号化後の文字列</returns>
        public string Encrypt(string text)
        {
            RijndaelManaged aes = CreateEncryptAlgorithm(PASSWORD);

            ICryptoTransform encrypt = aes.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);

            byte[] text_bytes = Encoding.UTF8.GetBytes(text);
            cryptStream.Write(text_bytes, 0, text_bytes.Length);
            cryptStream.FlushFinalBlock();

            byte[] encrypted = memoryStream.ToArray();

            return (Convert.ToBase64String(encrypted));
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="cryptText">暗号化された文字列</param>
        /// <returns>復号された文字列(JSON形式)</returns>
        public string Decrypt(string cryptText)
        {
            RijndaelManaged aes = CreateEncryptAlgorithm(PASSWORD);

            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] encrypted = Convert.FromBase64String(cryptText);
            byte[] planeText = new byte[encrypted.Length];

            MemoryStream memoryStream = new MemoryStream(encrypted);
            CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            cryptStream.Read(planeText, 0, planeText.Length);

            return (Encoding.UTF8.GetString(planeText));
        }

        #endregion
    }

    #endregion

    #region Serialization Class

    /// <summary>
    /// List<T>用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    private class Serialization<T>
    {
        public List<T> target;

        public List<T> ToList()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    /// <summary>
    /// Dictionary<TKey, TValue>用
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TValue">value</typeparam>
    [Serializable]
    private class Serialization<TKey, TValue>
    {
        public List<TKey> keys;
        public List<TValue> values;
        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            int count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            Enumerable.Range(0, count).ToList().ForEach(i => target.Add(keys[i], values[i]));
        }
    }

    #endregion
}
