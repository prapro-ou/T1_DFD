using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Quiz
    {
        public string prompt;         // お題
        public AudioClip[] options;   // 3つの選択肢の音
        public int correctIndex;      // 正解のインデックス（0〜2）
        public string explanationA;   // Aの解説
        public string explanationB;   // Bの解説
        public string explanationC;   // Cの解説

    }

    public Quiz[] quizList;

    [Header("問題表示")]
    public GameObject quizScreen;
    public TMP_Text promptText;
    public Button[] playButtons;      // 音再生ボタン
    public Button[] answerButtons;    // 回答ボタン

    [Header("結果表示")]
    public GameObject resultScreen;
    public TMP_Text resultTitleText;   // 「正解！」 or 「不正解…」
    public TMP_Text explanationText;   // 解説文
    public Button nextButton;          // 「次へ」ボタン

    [Header("音声")]
    public SoundManager soundManager;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private int currentQuizIndex = 0;

    void Start()
    {
        ShowQuizScreen();
        SetupQuiz();
    }

    void SetupQuiz()
    {
        var quiz = quizList[currentQuizIndex];
        promptText.text = quiz.prompt;

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            playButtons[i].onClick.RemoveAllListeners();
            playButtons[i].onClick.AddListener(() => PlaySound(index));

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void PlaySound(int index)
    {
        var quiz = quizList[currentQuizIndex];
        soundManager.PlaySound(quiz.options[index]);
    }

    void CheckAnswer(int selectedIndex)
    {
        var quiz = quizList[currentQuizIndex];
        bool isCorrect = (quiz.correctIndex == selectedIndex);

        // 正解／不正解テキストと音を設定
        resultTitleText.text = isCorrect
        ? "<color=#FF0000>正解！</color>"     // 赤（正解）
        : "<color=#0000FF>不正解…</color>";  // 青（不正解）
        explanationText.text =  $"A: {quiz.explanationA}\n" +
                                $"B: {quiz.explanationB}\n" +
                                $"C: {quiz.explanationC}";
        soundManager.PlaySound(isCorrect ? correctSound : wrongSound);

        ShowResultScreen();
    }

    public void OnNextButton()
    {
        currentQuizIndex++;
        if (currentQuizIndex >= quizList.Length)
        {
            Debug.Log("全問終了！");
            // 必要なら終了画面へ
            return;
        }

        ShowQuizScreen();
        SetupQuiz();
    }

    void ShowQuizScreen()
    {
        quizScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    void ShowResultScreen()
    {
        quizScreen.SetActive(false);
        resultScreen.SetActive(true);
    }
}

