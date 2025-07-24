using UnityEngine;
using Projects.Core;
using Projects.Utils;

namespace Projects.Utils
{
    /// <summary>
    /// シーン遷移ボタンのスクリプト
    /// UnityのUIボタンを使用して、指定されたシーンに遷移する機能を提供します。
    /// </summary>

    public class SceneLoadButton : MonoBehaviour
    {
        // Unityのインスペクターから遷移したいシーン名を設定するための変数
        [SerializeField]
        private GameScenes sceneName = GameScenes.None;

        /// <summary>
        /// ボタンがクリックされたときに呼び出される関数
        /// </summary>

        void Start()
        {
            if (sceneName == GameScenes.None)
            {
                Debug.LogError("Scene Name is not set! Please assign a valid GameScenes value in the inspector.");
            }
            this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
        }
        public void OnClick()
        {
            Debug.Log($"click scene: {sceneName.ToSceneName()}");
            // 指定されたシーン名のシーンをロードする
            if (sceneName != GameScenes.None)
            {
                // 質問にあった独自のシーンマネージャーを呼び出す
                StackedSceneManager.Instance.PushScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene Name is not set!");
            }
        }
    }
}