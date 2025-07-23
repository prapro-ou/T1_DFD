using System.Collections.Generic;
using Projects.Core;
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
        [SerializeField] private GameObject collectPanel;
        [SerializeField] private GameObject incollectPanel;
        public int quizCount = 5; // クイズの数
        
        public int correctCount = 0; // 正解数
        private TextMeshProUGUI choiceText1;
        private TextMeshProUGUI choiceText2;
        private TextMeshProUGUI choiceText3;

        [SerializeField] private GameObject quizResultShower;
        [SerializeField] private GameObject popupPanel;
        
        [SerializeField] private PlayerData playerData;

        private List<QuizQuestion> quizList;

        private List<QuizQuestion> answeredQuizList = new List<QuizQuestion>();
        // 現在のクイズの問題 
        private QuizQuestion currentQuizQuestion;

        private bool isShowingResult = false;

        private float clickTime = -1f;

        private bool isGameActive = true;

        void choiceQuestion()
        {
            quizCount--;
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
            if (isShowingResult || Time.time - clickTime < 0.2f || !isGameActive)
            { return; }

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
                // 正解の処理
                collectPanel.SetActive(true);
                incollectPanel.SetActive(false);
                correctCount++;
                Debug.Log("正解の選択肢: " + currentQuizQuestion.choices[index]);
            }
            else
            {
                collectPanel.SetActive(false);
                incollectPanel.SetActive(true);
                Debug.Log("不正解です。");
            }
            isShowingResult = true;
        }

        void setQuizQuestion(QuizQuestion quizQuestion)
        {
            currentQuizQuestion = quizQuestion;
            quizPanel.text = quizQuestion.question;
            choiceText1.text = quizQuestion.choices[0];
            choiceText2.text = quizQuestion.choices[1];
            choiceText3.text = quizQuestion.choices[2];
        }

        private void EndQuiz()
        {
            isGameActive = false; // ゲームを終了状態にする
            quizResultShower.SetActive(true);
            quizResultShower.GetComponent<QuizResultShower>().showResult(correctCount, answeredQuizList.Count);

            // [TODO] ポイントは後で調整
            playerData.AddPoint(correctCount * 10); // 正解数に応じてポイントを加算
            // クイズが終了したことをログに出力
            Debug.Log("クイズが終了しました。正解数: " + correctCount);
            
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
            // ここで最初のクイズを設定
            if (quizList.Count > 0 && quizCount > 0)
            {
                choiceQuestion();
            }

            collectPanel.SetActive(false);
            incollectPanel.SetActive(false);
            
            quizResultShower.SetActive(false);
        }
        // Update is called once per frame
        void Update()
        {   
            if (!isGameActive)
            {
                return; // ゲームが終了している場合は何もしない
            }
            if (Input.GetMouseButtonDown(0) && isShowingResult)
            {
                isShowingResult = false;
                collectPanel.SetActive(false);
                incollectPanel.SetActive(false);
                if (quizList.Count > 0 && quizCount > 0)
                {
                    choiceQuestion();
                }
                else if (quizCount == 0)
                {
                    EndQuiz();
                }
                clickTime = Time.time;

            }

        }
    }
}
