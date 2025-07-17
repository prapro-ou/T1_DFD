using UnityEngine;
using TMPro;

using UnityEngine.UI;

namespace Projects.MiniGames.WakamonoKotoba
{

    public class QuizResultShower : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // 
        [SerializeField] private TextMeshProUGUI resultText;
        public int correctCount = 0; // 正解数
        public int quizCount = 5; // クイズの数
        void Start()
        {

        }

        public void showResult()
        {
            resultText.text = "正解数: " + correctCount + " / " + quizCount;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}