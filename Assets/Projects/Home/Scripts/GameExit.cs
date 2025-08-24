using UnityEngine;

namespace Projects.Home
{
    public class GameExit : MonoBehaviour
    {

        void QuitGame()
        {
            // Unityエディタで実行している場合
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドされたアプリケーションで実行している場合
        Application.Quit();
#endif
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(QuitGame);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}