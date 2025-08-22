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
        public GameObject bouncingEnemyPrefab; // 跳ねる動きの邪魔者
        public GameObject zigzagEnemyPrefab; // ジグザグ動きの邪魔者
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

        private int correctCount;           // 正解の水マリコの数
        private int currentScore = 0;       // 現在のスコア
        private bool isGamePlaying = false; // ゲームがプレイ中かどうか

        [SerializeField] private Transform CatHouse; // 作った：水マリコの親オブジェクト

        [SerializeField] private Button startButton;
        // [SerializeField] private Textarea inputtextarea;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button backtotitleButton;
        [SerializeField] private Button retryButton2;
        [SerializeField] private Button backtotitleButton2;
        
        // playerDataの参照
        [SerializeField] private Projects.Core.PlayerData playerData; // PlayerDataの参照


        // ゲーム開始時に呼ばれる
        void Start()
        {
            // まだゲームを流さない（タイトル画面だけ出す）
            uiManager.ShowTitle();
            startButton.onClick.AddListener(OnStartButton);

            // scoreText.text = "スコア: " + currentScore.ToString();
        }

        // ゲームを開始する
        public void OnStartButton()
        {
            if (isGamePlaying) return;
            isGamePlaying = true;

            uiManager.ShowGame();     // ゲーム画面に切り替え（幕開きアニメは次ステップ）
            // scoreText.text = "スコア: " + currentScore.ToString(); //みんなに合わせる
            StartGame();              // 既存のゲームシーケンス開始
        }

        public void StartGame()
        {
            Debug.Log("start game");
            // if (isGamePlaying) return;

            // isGamePlaying = true;
            ClearSuimariko();                      // 前回の水マリコを消す（念のため）
            correctCount = 0;
            answerInputField.text = null;
            StartCoroutine(GameSequence());        // ゲームシーケンスを開始
            isGamePlaying = false; // StartGame の直後 or GameSequence の最後でリセット
        }

        // ゲームのシーケンス（流れ）を制御するコルーチン
        IEnumerator GameSequence()
        {
            // 水マリコ表示フェーズ
            Debug.Log(isGamePlaying);
            // StartCoroutine(SpawnEnemies());
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

            uiManager.ShowInputAnswer();
            answerInputField.interactable = true;
            submitButton.onClick.RemoveAllListeners(); // ★これが重要
            submitButton.onClick.AddListener(Result);
            
        }

        // 水マリコを生成するコルーチン
        IEnumerator SpawnSuimarikoRoutine()
        {
            int totalSuimarikoToSpawn = Random.Range(minSuimariko, maxSuimariko + 1);
            correctCount = totalSuimarikoToSpawn; // 正解の数を保存

            Debug.Log("正解の水マリ子数: " + correctCount);

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

                // 水マリコ生成
                var obj = Instantiate(suimarikoPrefab, CatHouse);
                obj.transform.localPosition = spawnPos;

                // // ここから邪魔者を追加（ランダムで出す）
                // if (Random.value < 0.8f) // 80%の確率で跳ねる邪魔者
                // {
                //     var enemy = Instantiate(bouncingEnemyPrefab, CatHouse);
                //     enemy.transform.localPosition = spawnPos + Vector3.up * 2;
                // }
                // if (Random.value < 0.8f) // 80%の確率でジグザグ邪魔者
                // {
                //     var enemy = Instantiate(zigzagEnemyPrefab, CatHouse);
                //     enemy.transform.localPosition = spawnPos + Vector3.up * 2;
                // }

                // 跳ねる敵（必ず出す）
                Vector3 bouncingPos = spawnPos + Vector3.up * 1f; // 少し上にずらす
                var bouncingEnemy = Instantiate(bouncingEnemyPrefab, bouncingPos, Quaternion.identity);
                bouncingEnemy.transform.SetParent(CatHouse, true);

                // ジグザグ敵（必ず出す）
                Vector3 zigzagPos = new Vector3(screenLeftEdge, spawnPos.y + 1.5f, 0f);
                var zigzagEnemy = Instantiate(zigzagEnemyPrefab, zigzagPos, Quaternion.identity);
                zigzagEnemy.transform.SetParent(CatHouse, true);
                // Debug.Log("zigzag");
                
                yield return new WaitForSeconds(suimarikoSpawnInterval);
            }
        }

        // IEnumerator SpawnEnemies()
        // {
        //     while (isGamePlaying)
        //     {
        //         // // 1. バウンドする敵を出す
        //         // Instantiate(bouncingEnemyPrefab, new Vector3(10, 0, 0), Quaternion.identity);

        //         // // 2. ジグザグする敵を出す
        //         // Instantiate(zigzagEnemyPrefab, new Vector3(10, 2, 0), Quaternion.identity);

        //         var enemy1 = Instantiate(bouncingEnemyPrefab, CatHouse);
        //         enemy1.transform.localPosition = new Vector3(10, 0, 0);

        //         var enemy2 = Instantiate(zigzagEnemyPrefab, CatHouse);
        //         enemy2.transform.localPosition = new Vector3(10, 2, 0);
        //         // 次の出現まで待つ（調整可能）
        //         yield return new WaitForSeconds(3f);
        //     }
        // }


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

        // 入力された文字列を整数に変換し，正解の数と比較して結果を処理
        // 正解だった場合はtrue、不正解または変換できなかった場合はfalse

        public bool CheckAnswer(string inputString)
        {
            int playerAnswer; // プレイヤーの回答を格納する変数

            // inputStringを整数に変換を試みる
            // int.TryParse は、変換に成功すればtrueを返し、playerAnswerに結果を格納します。
            // 失敗すればfalseを返し、playerAnswerは0になります。
            if (int.TryParse(inputString, out playerAnswer))
            {
                Debug.Log("check: " + correctCount);
                // 整数への変換に成功した場合
                if (playerAnswer == correctCount)
                {
                    // 正解の場合
                    currentScore += 100; // スコア加算
                    // scoreText.text = "スコア: " + currentScore.ToString();
                    return true; // 正解なのでtrueを返す
                }
                else
                {
                    // 不正解の場合
                    currentScore -= 50; // スコア減点
                    // scoreText.text = "スコア: " + currentScore.ToString();
                    return false; // 不正解なのでfalseを返す
                }
            }
            else
            {
                // 整数への変換に失敗した場合（例: 文字列に数字以外のものが含まれていた）
                // Debug.LogWarning("入力が無効です。数字を入力してください。");
                // currentScore -= 20; // 無効な入力へのペナルティ（例）
                // scoreText.text = "スコア: " + currentScore.ToString();
                return false; // 変換失敗なのでfalseを返す
            }
        }

        public void Result() // 正解の時には「正解」とピンポンを出す
        {
            Debug.Log("result");
            bool isCorrect = CheckAnswer(answerInputField.text); // 回答をチェック
            Debug.Log(isCorrect);

            if(isCorrect){
                Debug.Log("good");
                ResultGood();
            }else{
                Debug.Log("bad");
                ResultBad();
            }
        }

        public void ResultGood()
        {
            Debug.Log("resultgood");
            uiManager.ShowGoodResult();
            // answerInputField.text = null;
            retryButton.onClick.RemoveAllListeners();  // ★追加
            retryButton.onClick.AddListener(OnRetryButton);

            backtotitleButton.onClick.RemoveAllListeners();  // ★追加
            backtotitleButton.onClick.AddListener(OnBackToTitleButton);

            // ポイントの付与
            playerData.AddPoint(50); // PlayerDataのAddPointメソッドを呼び出す
            
        }

        public void ResultBad()
        {
            Debug.Log("resultbad");
            uiManager.ShowBadResult();
            // answerInputField.text = null;
            retryButton2.onClick.RemoveAllListeners();    // ★追加
            retryButton2.onClick.AddListener(OnRetryButton2);

            backtotitleButton2.onClick.RemoveAllListeners();  // ★追加
            backtotitleButton2.onClick.AddListener(OnBackToTitleButton2);
        }

        public void OnRetryButton()
        {
            // uiManager.ShowGame();
            // StartGame();
            OnStartButton();
        }

        public void OnBackToTitleButton()
        {
            uiManager.ShowTitle();
            // Start();
        }

        public void OnRetryButton2()
        {
            // uiManager.ShowGame();
            // StartGame();
            OnStartButton();
        }

        public void OnBackToTitleButton2()
        {
            uiManager.ShowTitle();
            // Start();
        }
    }
}