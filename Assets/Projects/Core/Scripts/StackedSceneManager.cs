using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Projects.Utils;

/// <summary>
/// シーン遷移の履歴をスタックで管理し、コルーチンを使って非同期でシーンをロードするSceneManagerです。
/// </summary>
/// 
namespace Projects.Core
{
    public enum GameScenes
    {
        None = 0, // 未設定状態
        Home,
        Title,

        Gacha, // ガチャのシーン
        // miniゲームのシーン
        StopwatchGame,
        WakamonoKotoba,
        OtoateQuiz,
        MemoryGame,
    }

    public static class SceneHelper
    {
        /// <summary>
        /// シーン名をGameScenes列挙型に変換します。
        /// </summary>
        public static GameScenes ToGameScene(this string sceneName)
        {
            return sceneName switch
            {
                "Home" => GameScenes.Home,
                "Title" => GameScenes.Title,
                "Gacha" => GameScenes.Gacha,
                "StopwatchScene" => GameScenes.StopwatchGame,
                "WakamonoKotoba" => GameScenes.WakamonoKotoba,
                "OtoateQuiz" => GameScenes.OtoateQuiz,
                "MemoryGame" => GameScenes.MemoryGame,
                _ => GameScenes.None,
            };
        }

        /// <summary>
        /// GameScenes列挙型をシーン名に変換します。
        /// </summary>
        public static string ToSceneName(this GameScenes gameScene)
        {
            return gameScene switch
            {
                GameScenes.Home => "Home",
                GameScenes.Title => "Title",
                GameScenes.Gacha => "Gacha",
                GameScenes.StopwatchGame => "StopwatchScene",
                GameScenes.WakamonoKotoba => "WakamonoKotoba",
                GameScenes.OtoateQuiz => "OtoateQuiz",
                GameScenes.MemoryGame => "MemoryGame",
                _ => string.Empty,
            };
        }
    }
    public class StackedSceneManager : Singleton<StackedSceneManager>
    {
        // --- Public Events ---
        [Header("シーンロード処理イベント")]
        public UnityEvent OnSceneLoadStart;
        public UnityEvent OnSceneLoadEnd;

        // --- Private Fields ---
        private readonly Stack<string> _sceneHistory = new Stack<string>();
        private bool _isLoading = false;
        protected override bool IsPersistent => true; // このシングルトンは永続化されるべき
                                                      // --- IEnumerable / LINQ Related Public Properties ---
        public IEnumerable<string> SceneHistory => _sceneHistory;
        public int HistoryCount => _sceneHistory.Count;

        // --- Scene Transition Methods (Public API) ---

        /// <summary>
        /// 新しいシーンに非同期で遷移します。現在のシーンは履歴としてスタックに保存されます。
        /// </summary>
        public void PushScene(GameScenes gameScene)
        {
            string sceneName = gameScene.ToSceneName();
            Debug.Log($"Pushing scene: {sceneName} to history stack.");
            if (_isLoading) return;
            if (string.IsNullOrEmpty(sceneName)) return;

            Debug.Log($"Pushing scene: {sceneName} to history stack.");
            _sceneHistory.Push(SceneManager.GetActiveScene().name);
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// 履歴を元に一つ前のシーンに非同期で戻ります。
        /// </summary>
        public void PopScene()
        {
            if (_isLoading) return;

            if (_sceneHistory.Count > 0)
            {
                string previousScene = _sceneHistory.Pop();
                StartCoroutine(LoadSceneCoroutine(previousScene));
            }
            else
            {
                Debug.LogWarning("これ以上戻るシーンはありません。");
            }
        }

        /// <summary>
        /// シーンの履歴を全てクリアし、指定されたシーンに非同期で遷移します。
        /// </summary>
        public void LoadScene(GameScenes gameScene)
        {
            string sceneName = gameScene.ToSceneName();
            if (_isLoading) return;
            if (string.IsNullOrEmpty(sceneName)) return;

            _sceneHistory.Clear();
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// 指定したシーンが履歴の最上部に来るまでスタックを巻き戻してから、そのシーンをロードします。
        /// </summary>
        public void PopUntil(GameScenes gameScene)
        {
            string sceneName = gameScene.ToSceneName();
            if (_isLoading || !ContainsInHistory(sceneName))
            {
                Debug.LogWarning($"シーン履歴に '{sceneName}' が見つからないため、処理を中断しました。");
                return;
            }

            while (_sceneHistory.Count > 0 && _sceneHistory.Peek() != sceneName)
            {
                _sceneHistory.Pop();
            }

            // 目的のシーンをロード
            PopScene();
        }

        // --- IEnumerable / LINQ Related Public Methods ---

        /// <summary>
        /// LINQを使い、履歴内に指定したシーンが存在するかどうかを確認します。
        /// </summary>
        public bool ContainsInHistory(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return false;
            return _sceneHistory.Contains(sceneName);
        }
        public bool ContainsInHistory(GameScenes gameScene)
        {
            string sceneName = gameScene.ToSceneName();
            return ContainsInHistory(sceneName);
        }

        // --- Core Coroutine for Scene Loading ---

        /// <summary>
        /// シーンを非同期でロードするコルーチン
        /// </summary>
        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            _isLoading = true;
            OnSceneLoadStart?.Invoke();

            // SceneManager.LoadSceneAsyncはAsyncOperationオブジェクトを返す
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // isDoneがtrueになるまで、毎フレーム待機する
            while (!asyncLoad.isDone)
            {
                // ここでロードの進捗をUIに表示することも可能 (例: asyncLoad.progress)
                yield return null; // 次のフレームまで処理を中断
            }

            OnSceneLoadEnd?.Invoke();
            _isLoading = false;
        }
    }

}