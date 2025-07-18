using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Quiz
    {
        public string prompt;
        public AudioClip[] options;
        public int correctIndex;
        public string explanationA;
        public string explanationB;
        public string explanationC;
    }

    public Quiz[] quizList;

    [Header("問題表示")]
    public GameObject quizScreen;
    public TMP_Text promptText;
    public Button[] playButtons;       // A/B/C 音再生ボタン
    public Button[] answerButtons;     // A/B/C 選択ボタン

    [Header("結果表示")]
    public GameObject resultScreen;
    public TMP_Text resultTitleText;
    public TMP_Text explanationText;
    public Button nextButton;

    [Header("解説再生ボタン")]
    public Button[] resultPlayButtons; // 解説画面用A/B/C音再生ボタン

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

            // 初期色
            SetButtonColor(answerButtons[i], Color.white);
        }

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            resultPlayButtons[i].onClick.RemoveAllListeners();
            resultPlayButtons[i].onClick.AddListener(() => PlaySound(index));
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        var quiz = quizList[currentQuizIndex];
        bool isCorrect = (quiz.correctIndex == selectedIndex);

        resultTitleText.text = isCorrect
            ? "<color=#FF0000>正解！</color>"
            : "<color=#0000FF>不正解…</color>";

        explanationText.text = $"A: {quiz.explanationA}\n" +
                               $"B: {quiz.explanationB}\n" +
                               $"C: {quiz.explanationC}";

        soundManager.PlaySound(isCorrect ? correctSound : wrongSound);
        ShowResultScreen();
    }

    void PlaySound(int index)
    {
        var quiz = quizList[currentQuizIndex];
        soundManager.PlaySound(quiz.options[index]);
    }

    public void OnNextButton()
    {
        currentQuizIndex++;
        if (currentQuizIndex >= quizList.Length)
        {
            Debug.Log("全問終了！");
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

    void SetButtonColor(Button btn, Color color)
    {
        var colors = btn.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        colors.highlightedColor = color;
        btn.colors = colors;
    }
}
