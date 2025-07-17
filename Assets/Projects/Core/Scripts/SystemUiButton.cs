using UnityEngine;
using UnityEngine.UI;

namespace Projects.Core
{
    /// <summary>
    /// システムUIボタンの基本クラス
    /// /// MonoBehaviourを継承し、システムUIボタンの基本的な機能を提供します。
    /// </summary>
    public class SystemUiButton : MonoBehaviour
    {

        [SerializeField] private Button backButton;
        [SerializeField] private Button settingsButton;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void OnBackButtonClicked()
        {
            // 戻るボタンがクリックされたときの処理
            StackedSceneManager.Instance.PopScene();
        }
        void Start()
        {
            if (backButton == null)
            {
                Debug.LogError("Back button is not assigned in the inspector.");
                return;
            }
            if (settingsButton == null)
            {
                Debug.LogError("Settings button is not assigned in the inspector.");
            }
            backButton.onClick.AddListener(OnBackButtonClicked);

            //[TODO] 設定ボタンのクリックイベントを追加
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}