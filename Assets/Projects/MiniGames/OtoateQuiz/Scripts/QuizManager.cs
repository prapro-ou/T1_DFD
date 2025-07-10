using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro対応に必須

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Quiz
    {
        public string prompt;        // お題テキスト
        public AudioClip[] options;  // 3つの音
        public int correctIndex;     // 正解の番号（0～2）
    }

    public Quiz[] quizList;             // クイズ配列
    public TextMeshProUGUI promptText;         // お題表示用TextMeshProUGUI
    public TextMeshProUGUI resultText;         // 結果表示用TextMeshProUGUI

    public Button[] playButtons;        // 音再生ボタン3つ
    public Button[] answerButtons;      // 回答ボタン3つ

    public SoundManager soundManager;   // 音再生担当

    public AudioClip correctSound;  // 正解音
    public AudioClip wrongSound;    // 不正解音
    private int currentQuizIndex = 0;


    void Start()
    {
        SetupQuiz();
    }

    void SetupQuiz()
    {
        resultText.text = "";
        var quiz = quizList[currentQuizIndex];
        promptText.text = quiz.prompt;

        for (int i = 0; i < 3; i++)
        {
            int index = i; // クロージャ対策

            playButtons[i].onClick.RemoveAllListeners();
            playButtons[i].onClick.AddListener(() => PlaySound(index));

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void PlaySound(int index)
    {
        soundManager.PlaySound(quizList[currentQuizIndex].options[index]);
    }

    void CheckAnswer(int selectedIndex)
    {
        if (quizList[currentQuizIndex].correctIndex == selectedIndex)
        {
            resultText.text = "正解！";
            soundManager.PlaySound(correctSound);
        }
        else
        {
            resultText.text = "不正解…";
            soundManager.PlaySound(wrongSound); 
        }
    }
}
