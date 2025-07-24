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
        [SerializeField] private GameObject resultgoodPanel;
        [SerializeField] private GameObject resultbadPanel;

        public void ShowTitle()
        {
            titlePanel.SetActive(true);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(false);
            resultgoodPanel.SetActive(false);
            resultbadPanel.SetActive(false);
        }

        public void ShowGame()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(true);
            aftergamePanel.SetActive(false);
            resultgoodPanel.SetActive(false);
            resultbadPanel.SetActive(false);
        }

        public void ShowInputAnswer()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(true);
            resultgoodPanel.SetActive(false);
            resultbadPanel.SetActive(false);
        }

        public void ShowGoodResult()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(false);
            resultgoodPanel.SetActive(true);
            resultbadPanel.SetActive(false);
        }

        public void ShowBadResult()
        {
            titlePanel.SetActive(false);
            gamePanel.SetActive(false);
            aftergamePanel.SetActive(false);
            resultgoodPanel.SetActive(false);
            resultbadPanel.SetActive(true);
        }
    }
}
