using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.MiniGames.WakamonoKotoba
{
    public class QuizManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI quizPanel;

        [SerializeField] private Button choicePanel1;
        [SerializeField] private Button choicePanel2;
        [SerializeField] private Button choicePanel3;

        private TextMeshProUGUI choiceText1;
        private TextMeshProUGUI choiceText2;
        private TextMeshProUGUI choiceText3;

        private List<QuizQuestion> quizList;

        private List<QuizQuestion> answeredQuizList = new List<QuizQuestion>();
        // 現在のクイズの問題 
        private QuizQuestion currentQuizQuestion;

        void choiceQuestion()
        {
            Random.Range(0, quizList.Count);
            // ここでランダムにクイズを選ぶ処理を実装する
            // 例えば、quizListからランダムに1問選んでcurrentQuizQuestionに設定する
            if (quizList.Count == 0)
            {
                Debug.LogWarning("クイズリストが空です。");
                return;
            }
            int randomIndex = Random.Range(0, quizList.Count);
            currentQuizQuestion = quizList[randomIndex];
            answeredQuizList.Add(currentQuizQuestion);
            setQuizQuestion(currentQuizQuestion);
            quizList.RemoveAt(randomIndex); // 選んだクイズはリストから削除
        }
        void OnSelectButton(int index)
        {
            Debug.Log(index + "番の選択肢が選ばれました。");

            if (currentQuizQuestion == null)
            {
                Debug.LogWarning("現在のクイズが設定されていません。");
                return;
            }
            if (index < 0 || index >= currentQuizQuestion.choices.Length)
            {
                Debug.LogWarning("選択肢のインデックスが範囲外です。");
                return;
            }

            if (index == currentQuizQuestion.correctIndex)
            {
                Debug.Log("正解です！");
            }
            else
            {
                Debug.Log("不正解です。");
            }
            //[TODO] ここで正誤判定などを行う
        }

        void setQuizQuestion(QuizQuestion quizQuestion)
        {
            currentQuizQuestion = quizQuestion;
            quizPanel.text = quizQuestion.question;
            choiceText1.text = quizQuestion.choices[0];
            choiceText2.text = quizQuestion.choices[1];
            choiceText3.text = quizQuestion.choices[2];
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            choiceText1
                = choicePanel1.GetComponentInChildren<TextMeshProUGUI>();
            choiceText2
                = choicePanel2.GetComponentInChildren<TextMeshProUGUI>();
            choiceText3
                = choicePanel3.GetComponentInChildren<TextMeshProUGUI>();

            choicePanel1.onClick.AddListener(() => OnSelectButton(0));
            choicePanel2.onClick.AddListener(() => OnSelectButton(1));
            choicePanel3.onClick.AddListener(() => OnSelectButton(2));

            // LoadQuizFromJsonAndConv();
            quizList = QuizDataConverter.LoadAndConvert();

            Debug.Log("クイズの数: " + quizList.Count);
            Debug.Log("1問目の問題: " + quizList[0].question);
            Debug.Log("1問目の選択肢: " + string.Join(", ", quizList[0].choices));

            // ここで最初のクイズを設定
            if (quizList.Count > 0)
            {
                choiceQuestion();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
