using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 勝利するためのラウンド数
    public int numRoundsToWin = 2;
    // RoundStarting の開始と RoundPlaying 段階の間の遅延
    public float startDelay;
    // RoundPlaying の終わりと RoundEnding 段階の間の遅延
    public float endDelay = 3.0f;
    // プレイヤープレハブ
    private GameObject playerPrefab;
    // CPUプレハブ
    public GameObject cpuPrefab;
    // キャラクターマネージャー配列
    public CharacterManager[] characters;
    // 現在のラウンド数
    private int roundNum;
    // ラウンドの開始を遅延させる
    //private WaitForSeconds startWait;
    // ラウンド/ゲーム終了を遅延させる
    private WaitForSeconds endWait;
    // ラウンド勝者
    private CharacterManager roundWinner;
    // ゲーム勝者
    private CharacterManager gameWinner;
    // Playerスクリプトへの参照
    private Player playerScript;
    // CPUスクリプトへの参照
    private CPU cpuScript;
    // ゲームタイマー
    public Timer timer;
    // ログディスプレイ
    public LogDisplay logDisp;
    // ボタン
    public Buttons buttons;
    // 応援キャラクターイメージ
    public Image[] images;
    // 応援キャラクターテクスチャ
    private Texture2D[] textures = new Texture2D[2];
    // 表示したい1辺の長さ
    private int size;
    // textureのパス
    private const string path = "Textures/";
    // 豆知識用イメージ
    private GameObject tipsImg;
    // 豆知識キャラクターイメージ
    private GameObject charaImg;
    // 豆知識テキスト
    private GameObject text;
    // くまモンテキスト
    private string[] characterText001 = new string[] { "くまモン\n1番1番1番", "くまモン\n2番2番2番", "くまモン\n3番3番3番" };
    // 太閤はんテキスト
    private string[] characterText002 = new string[] { "太閤はん\n1番1番1番", "太閤はん\n2番2番2番", "太閤はん\n3番3番3番" };


    private void Start()
    {
        playerPrefab = AvatarManager.Instance.player;
        playerScript = playerPrefab.GetComponent<Player>();
        size = 200;

        for (int i = 0; i < textures.Length; ++i)
        {
            textures[i] = Resources.Load(path + string.Format("character{0:D3}", i + 1), typeof(Texture2D)) as Texture2D;
            int width = textures[i].width;
            int height = textures[i].height;
            if (width == height)
            {
                float ratio = (float)size / (float)width;
                TextureScale.Bilinear(textures[i], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
            }
            else
            {
                int max = Mathf.Max(width, height);
                float ratio = (float)size / (float)max;
                TextureScale.Bilinear(textures[i], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
            }

            RectTransform rt = images[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(textures[i].width, textures[i].height);
            images[i].sprite = Sprite.Create(textures[i], new Rect(0, 0, textures[i].width, textures[i].height), Vector2.zero);
        }

        charaImg = GameObject.Find("Canvas/BlackImage/WhiteImage/CharacterImage");
        tipsImg = GameObject.Find("Canvas/BlackImage");
        text = GameObject.Find("Canvas/BlackImage/WhiteImage/Text");
        tipsImg.SetActive(false);

        //startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnCharacter();

        StartCoroutine(GameLoop());
    }

    private void SpawnCharacter()
    {
        characters[0].instance = playerPrefab;
        characters[1].instance = cpuPrefab;
        playerScript = playerPrefab.GetComponent<Player>();
        cpuScript = cpuPrefab.GetComponent<CPU>();
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        // 勝者がいるかチェックする
        if (gameWinner != null)
        {
            // ゲーム勝者がいる場合の処理
            tipsImg.SetActive(true);
            int rnd = Random.Range(1, 3);
            size = 700;

            if (gameWinner == characters[0])
            {
                logDisp.DebagLog("ゲーム勝利", 3);
                int width = textures[0].width;
                int height = textures[0].height;
                if (width == height)
                {
                    float ratio = (float)size / (float)width;
                    TextureScale.Bilinear(textures[0], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
                }
                else
                {
                    int max = Mathf.Max(width, height);
                    float ratio = (float)size / (float)max;
                    TextureScale.Bilinear(textures[0], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
                }

                RectTransform rt = charaImg.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(textures[0].width, textures[0].height);
                charaImg.GetComponent<Image>().sprite = Sprite.Create(textures[0], new Rect(0, 0, textures[0].width, textures[0].height), Vector2.zero);
                charaImg.GetComponent<Image>().sprite = images[0].sprite;
                text.GetComponent<Text>().text = characterText001[rnd];
            }
            else
            {
                logDisp.DebagLog("ゲーム敗北", 3);
                int width = textures[1].width;
                int height = textures[1].height;
                if (width == height)
                {
                    float ratio = (float)size / (float)width;
                    TextureScale.Bilinear(textures[1], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
                }
                else
                {
                    int max = Mathf.Max(width, height);
                    float ratio = (float)size / (float)max;
                    TextureScale.Bilinear(textures[1], (int)Mathf.Floor(width * ratio), (int)Mathf.Floor(height * ratio));
                }

                RectTransform rt = charaImg.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(textures[1].width, textures[1].height);
                charaImg.GetComponent<Image>().sprite = Sprite.Create(textures[1], new Rect(0, 0, textures[1].width, textures[1].height), Vector2.zero);
                charaImg.GetComponent<Image>().sprite = images[1].sprite;
                text.GetComponent<Text>().text = characterText002[rnd];
            }
        }
        else
        {
            // 勝者がいない場合は、このコルーチンをリスタートしてループを続ける
            if (roundNum < 3)
            {
                StartCoroutine(GameLoop());
            }
            else
            {
                ResetCharacters();
                DisableCharacterControl();
                logDisp.DebagLog("引き分け", 3);
            }
        }
    }

    private IEnumerator RoundStarting()
    {
        // タイムをリセットしてテキストを表示
        startDelay = 3.0f;
        logDisp.text[9].enabled = true;

        // ラウンドが開始したらキャラクターをリセットして制御できないようにする
        ResetCharacters();
        DisableCharacterControl();

        // ラウンド数を増加させる
        roundNum++;

        while (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            int second = (int)startDelay;
            logDisp.DebagLog(second.ToString(), 9);
            yield return null;
        }

        // 指定した時間を待機
        //yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        // テキストを消す
        logDisp.text[9].enabled = false;

        // ゲームタイマースタート
        timer.GameStart();

        // 操作可能にする
        EnableCharacterControl();

        // 勝敗が決定していない、もしくは時間切れになっていない場合ループ
        while (!OneCharacterLeft() && !timer.TimeOut())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        // タイマーをとめる
        timer.Stop();

        // 操作不可にする
        DisableCharacterControl();

        // 前のラウンドの勝者をnullにする
        roundWinner = null;

        // ラウンドが終了した時点で時間切れで無い場合、勝者がいるかを確認
        if (!timer.TimeOut())
        {
            roundWinner = GetRoundWinner();
        }

        // 勝者がいる場合はスコアを増加
        if (roundWinner != null)
        {
            roundWinner.roundWins++;
        }

        // スコアを表示
        logDisp.DebagLog(characters[0].roundWins.ToString(), 4);
        logDisp.DebagLog(characters[1].roundWins.ToString(), 5);

        // ゲームの勝者がいるかを確認
        gameWinner = GetGameWinner();

        // 指定した時間を待機
        yield return endWait;

        // ゲームタイマーをリセット
        timer.Reset();

        // 勝敗判定テキストを削除
        logDisp.DebagLog(null, 3);
    }

    // ラウンド終了判定
    private bool OneCharacterLeft()
    {
        int numCharacterLeft = 0;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].instance.gameObject.activeSelf)
            {
                numCharacterLeft++;
            }
        }

        return numCharacterLeft <= 1;
    }

    // ラウンドの勝者がいるかを確認
    private CharacterManager GetRoundWinner()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].instance.gameObject.activeSelf)
            {
                return characters[i];
            }
        }

        return null;
    }

    // ゲームの勝者がいるかを確認
    private CharacterManager GetGameWinner()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].roundWins == numRoundsToWin)
            {
                return characters[i];
            }
        }

        // 第3ラウンド終了時判定
        if (roundNum == 3)
        {
            if (characters[0].roundWins > characters[1].roundWins)
            {
                return characters[0];
            }
            else if (characters[0].roundWins < characters[1].roundWins)
            {
                return characters[1];
            }
        }

        return null;
    }

    private void ResetCharacters()
    {
        playerScript.Reset();
        cpuScript.Reset();

        playerPrefab.SetActive(false);
        cpuPrefab.SetActive(false);
        playerPrefab.SetActive(true);
        cpuPrefab.SetActive(true);
    }

    private void EnableCharacterControl()
    {
        buttons.Enable();
        playerScript.enabled = true;
        cpuScript.enabled = true;
    }

    private void DisableCharacterControl()
    {
        buttons.Disable();
        playerScript.enabled = false;
        cpuScript.enabled = false;
    }
}
