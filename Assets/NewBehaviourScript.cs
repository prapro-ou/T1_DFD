using UnityEngine;
using System.Collections; // IEnumerator を使うために必要
using TMPro; // UIテキストを使う場合 (UnityのTextMeshProを別途インポート)

public class GameManager : MonoBehaviour
{
    public GameObject suimarikoPrefab; // 水マリコのプレハブ
    public int minSuimariko = 5;      // 表示する水マリコの最小数
    public int maxSuimariko = 15;     // 表示する水マリコの最大数
    public float displayTime = 3.0f;  // 水マリコが表示される時間 (秒)
    public float answerTime = 5.0f;   // 回答入力猶予時間 (秒)

    public TextMeshProUGUI timerText;    // タイマー表示用UIテキスト
    public TextMeshProUGUI scoreText;    // スコア表示用UIテキスト
    public TMP_InputField answerInputField; // 回答入力用UIインプットフィールド
    public GameObject gamePanel;        // ゲーム表示パネル (水マリコ、タイマー、入力欄などを含む)
    public GameObject resultPanel;      // 結果表示パネル

    private int correctCount;           // 正解の水マリコの数
    private int currentScore = 0;       // 現在のスコア
    private bool isGamePlaying = false; // ゲームがプレイ中かどうか

    // ゲーム開始時に呼ばれる
    void Start()
    {
        // 初期状態では結果パネルは非表示
        if (resultPanel != null) resultPanel.SetActive(false);
        // 最初はゲームパネルを表示
        if (gamePanel != null) gamePanel.SetActive(true);

        scoreText.text = "スコア: " + currentScore.ToString();
        StartGame();
    }

    // ゲームを開始する
    public void StartGame()
    {
        if (isGamePlaying) return;

        isGamePlaying = true;
        answerInputField.interactable = false; // 回答中は入力不可
        answerInputField.text = "";            // 入力欄をクリア
        ClearSuimariko();                      // 前回の水マリコを消す
        StartCoroutine(GameSequence());        // ゲームシーケンスを開始
    }

    // ゲームのシーケンス（流れ）を制御するコルーチン
    IEnumerator GameSequence()
    {
        // 水マリコ表示フェーズ
        SpawnSuimariko();
        yield return new WaitForSeconds(displayTime); // 指定時間表示
        ClearSuimariko(); // 水マリコを非表示にする

        // 回答フェーズ
        answerInputField.interactable = true; // 回答可能にする
        float timer = answerTime;
        while (timer > 0)
        {
            timerText.text = "残り時間: " + Mathf.CeilToInt(timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }
        timerText.text = "時間切れ！";
        SubmitAnswer(); // 時間切れで強制的に回答を提出

        isGamePlaying = false; // ゲーム終了
        // 必要に応じてゲーム終了後の処理（リトライボタン表示など）
    }

    // 水マリコを生成する
    void SpawnSuimariko()
    {
        int count = Random.Range(minSuimariko, maxSuimariko + 1);
        correctCount = count; // 正解の数を保存

        // スプライトの表示範囲を調整
        // カメラの大きさに応じて調整が必要
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        for (int i = 0; i < count; i++)
        {
            // 画面内のランダムな位置に生成
            float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
            float randomY = Random.Range(-screenHeight / 2, screenHeight / 2);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            Instantiate(suimarikoPrefab, spawnPos, Quaternion.identity);
        }
    }

    // 現在表示されている水マリコを全て消す
    void ClearSuimariko()
    {
        GameObject[] suimarikoObjects = GameObject.FindGameObjectsWithTag("Suimariko");
        foreach (GameObject obj in suimarikoObjects)
        {
            Destroy(obj);
        }
    }

    // 回答を提出する（UIボタンなどから呼び出す）
    public void SubmitAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (playerAnswer == correctCount)
            {
                currentScore += 100; // 正解で100点加算
                scoreText.text = "スコア: " + currentScore.ToString();
                Debug.Log("正解！");
            }
            else
            {
                currentScore -= 50; // 不正解で50点減点
                scoreText.text = "スコア: " + currentScore.ToString();
                Debug.Log("不正解！ 正解は " + correctCount + " でした。");
            }
        }
        else
        {
            Debug.Log("数字を入力してください。");
        }

        // 次のラウンドに進むか、ゲーム終了
        StartCoroutine(NextRoundDelay());
    }

    // 次のラウンドへ移行するまでのディレイ
    IEnumerator NextRoundDelay()
    {
        yield return new WaitForSeconds(2.0f); // 2秒待ってから次のラウンド
        StartGame(); // 次のラウンドを開始
    }
}