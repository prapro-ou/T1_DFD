using UnityEngine;
using UnityEngine.UI;
using System.Collections; // IEnumerator を使うために必要
using TMPro; // UIテキストを使う場合 (UnityのTextMeshProを別途インポート)


namespace TaikoGame
{
    public class GameManager : MonoBehaviour
    {
        public UIManager uiManager;   // インスペクタで割り当て
        public GameObject suimarikoPrefab; // 水マリコのプレハブ

        public int minSuimariko = 10;      // 表示する水マリコの最小数（10～20個に調整）
        public int maxSuimariko = 20;     // 表示する水マリコの最大数（10～20個に調整）
        public float displayTime = 3.0f;  // 水マリコが表示される時間 (秒)
        public float answerTime = 5.0f;   // 回答入力猶予時間 (秒)

        public int suimarikoSpawnBatchSize = 1; // バラバラ感を出すため1に近づけるか、後述の遅延に変更
        public float suimarikoSpawnInterval = 0.1f; // 各水マリコ間の生成間隔

        public float spawnStartXOffset = 2.0f; // 画面左端からどれくらい手前から出現するか（調整用）
        public float initialXSpread = 5.0f; // 水マリコが生成されるX座標のバラつき範囲（調整用）


        public TextMeshProUGUI timerText;    // タイマー表示用UIテキスト
        public TextMeshProUGUI scoreText;    // スコア表示用UIテキスト
        public TMP_InputField answerInputField; // 回答入力用UIインプットフィールド
        public GameObject gamePanel;        // ゲーム表示パネル (水マリコ、タイマー、入力欄などを含む)
        public GameObject resultPanel;      // 結果表示パネル

        private int correctCount;           // 正解の水マリコの数
        private int currentScore = 0;       // 現在のスコア
        private bool isGamePlaying = false; // ゲームがプレイ中かどうか

        [SerializeField] private Transform CatHouse; // 作った：水マリコの親オブジェクト

        [SerializeField] private Button startButton;

        // ゲーム開始時に呼ばれる
        void Start()
        {
            // まだゲームを流さない。タイトル画面だけ出す
            uiManager.ShowTitle();
            // resultPanel.SetActive(false);  // 念のため
            if (resultPanel != null) resultPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(true);

            startButton.onClick.AddListener(OnStartButton);

            // scoreText.text = "スコア: " + currentScore.ToString();
            // StartGame();
        }

        // ゲームを開始する
        public void OnStartButton()
        {
            uiManager.ShowGame();     // ゲーム画面に切り替え（幕開きアニメは次ステップ）
            // currentScore = 0;
            // scoreText.text = "スコア: 0";
            // scoreText.text = "スコア: " + currentScore.ToString(); //みんなに合わせる
            StartGame();              // 既存のゲームシーケンス開始
        }

        public void StartGame()
        {
            Debug.Log("start game");
            if (isGamePlaying) return;

            isGamePlaying = true;
            // answerInputField.interactable = false; // 回答中は入力不可
            // answerInputField.text = "";            // 入力欄をクリア
            ClearSuimariko();                      // 前回の水マリコを消す（念のため）
            StartCoroutine(GameSequence());        // ゲームシーケンスを開始
        }

        // ゲームのシーケンス（流れ）を制御するコルーチン
        IEnumerator GameSequence()
        {
            // 水マリコ表示フェーズ
            yield return StartCoroutine(SpawnSuimarikoRoutine());
            yield return new WaitForSeconds(displayTime); // 指定時間表示

            // ここから追加・修正: 全ての水マリコが画面外に流れ終わるまで待機
            // CatHouseの子として管理している場合
            if (CatHouse != null)
            {
                // CatHouseの子がなくなるまで待機
                while (CatHouse.childCount > 0)
                {
                    yield return null; // 1フレーム待機
                }
            }
            else // CatHouseが設定されていない場合のフォールバック（タグ検索）
            {
                // "Suimariko"タグを持つオブジェクトがなくなるまで待機
                while (GameObject.FindGameObjectsWithTag("Suimariko").Length > 0)
                {
                    yield return null; // 1フレーム待機
                }
            }
            // ClearSuimariko(); // 各水マリコが自分で消滅するため、ここでは不要。
                               // ただし、displayTime中に強制的に消したい場合は残しても良い。
                               // 完全に流れ終わるのを待つ場合は、この行は削除またはコメントアウト。
            uiManager.ShowInputAnswer();

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

        // 水マリコを生成するコルーチン
        IEnumerator SpawnSuimarikoRoutine()
        {
            int totalSuimarikoToSpawn = Random.Range(minSuimariko, maxSuimariko + 1);
            correctCount = totalSuimarikoToSpawn; // 正解の数を保存

            float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
            float screenHeight = Camera.main.orthographicSize * 2;
            float screenLeftEdge = Camera.main.transform.position.x - (screenWidth / 2);

            float suimarikoSpriteHalfWidth = 0f;
            if (suimarikoPrefab != null)
            {
                SpriteRenderer sr = suimarikoPrefab.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    suimarikoSpriteHalfWidth = sr.bounds.extents.x;
                }
            }

            float spawnStartX = screenLeftEdge - suimarikoSpriteHalfWidth - spawnStartXOffset;

            for (int i = 0; i < totalSuimarikoToSpawn; i++)
            {
                float initialOffsetX = Random.Range(0f, initialXSpread);
                float currentSpawnX = spawnStartX - initialOffsetX;

                float randomY = Random.Range(-screenHeight / 2 + suimarikoSpriteHalfWidth, screenHeight / 2 - suimarikoSpriteHalfWidth);

                Vector3 spawnPos = new Vector3(currentSpawnX, randomY, 0);

                var obj = Instantiate(suimarikoPrefab, CatHouse);
                obj.transform.localPosition = spawnPos;

                yield return new WaitForSeconds(suimarikoSpawnInterval);
            }
        }

        // 現在表示されている水マリコを全て消す（念のための処理、通常はSuimarikoFlowMovementで消える）
        void ClearSuimariko()
        {
            // CatHouseの子オブジェクトとして水マリコを管理している場合、
            // その子を削除する方が確実です。
            if (CatHouse != null)
            {
                for (int i = CatHouse.childCount - 1; i >= 0; i--)
                {
                    Destroy(CatHouse.GetChild(i).gameObject);
                }
            }
            else // CatHouseが設定されていない場合のフォールバック
            {
                GameObject[] suimarikoObjects = GameObject.FindGameObjectsWithTag("Suimariko");
                foreach (GameObject obj in suimarikoObjects)
                {
                    Destroy(obj);
                }
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

            // StartCoroutine(NextRoundDelay());
        }

        public void OnRetryButton()
        {
            uiManager.ShowGame();
            currentScore = 0;
            scoreText.text = "スコア: 0";
            StartGame();
        }

        public void OnBackToTitleButton()
        {
            uiManager.ShowTitle();
        }


        // // 次のラウンドへ移行するまでのディレイ
        // IEnumerator NextRoundDelay()
        // {
        //     yield return new WaitForSeconds(2.0f); // 2秒待ってから次のラウンド
        //     StartGame(); // 次のラウンドを開始
        // }
    }
}