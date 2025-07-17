using UnityEngine;

namespace TaikoGame
{
    /// <summary>
    /// UI の表示・非表示を切り替えるだけのヘルパー
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject titlePanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject aftergamePanel;
        [SerializeField] private GameObject resultPanel;

        public void ShowTitle()
        {
            titlePanel.SetActive(true);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(false);
            resultPanel.SetActive(false);
        }

        public void ShowGame()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(true);
            aftergamePanel.SetActive(false);
            resultPanel.SetActive(false);
        }

        public void ShowInputAnswer()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(true);
            resultPanel.SetActive(false);
        }

        public void ShowResult()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(false);
            resultPanel.SetActive(true);
        }
    }
}
