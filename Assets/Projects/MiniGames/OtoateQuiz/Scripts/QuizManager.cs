using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    public Button[] playButtons;          // 問題画面での音声ボタン A〜C
    public Button[] answerButtons;        // 回答選択ボタン A〜C
    public Button confirmButton;

    [Header("結果表示")]
    public GameObject resultScreen;
    public TMP_Text resultTitleText;
    public TMP_Text explanationText;
    public Button nextButton;
    public Button[] replayButtons;        // 解説画面での音声再生ボタン A〜C

    [Header("音声")]
    public SoundManager soundManager;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private List<Quiz> randomizedQuizzes = new List<Quiz>();
    private int currentQuizIndex = 0;
    private int selectedIndex = -1;

    void Start()
    {
        GenerateRandomQuizzes();
        ShowQuizScreen();
        SetupQuiz();
    }

    void GenerateRandomQuizzes()
    {
        List<Quiz> originalList = new List<Quiz>(quizList);
        for (int i = 0; i < 5 && originalList.Count > 0; i++)
        {
            int randIndex = Random.Range(0, originalList.Count);
            randomizedQuizzes.Add(originalList[randIndex]);
            originalList.RemoveAt(randIndex);
        }
    }

void SetupQuiz()
{
    selectedIndex = -1;
    confirmButton.interactable = false;

    // 🎯 ここで選択肢ボタンの色をリセットする（Image.color と Button.colors 両方）
    for (int i = 0; i < answerButtons.Length; i++)
    {
        var btn = answerButtons[i];

        // Image の色をリセット
        Image img = btn.GetComponent<Image>();
        img.color = Color.white;

        // 必要であれば Button.colors もリセット
        var colors = btn.colors;
        colors.normalColor = Color.white;
        btn.colors = colors;
    }

    var quiz = randomizedQuizzes[currentQuizIndex];
    promptText.text = quiz.prompt;

    // 問題画面：音声再生と選択肢のボタン設定
    for (int i = 0; i < 3; i++)
    {
        int index = i;
        playButtons[i].onClick.RemoveAllListeners();
        playButtons[i].onClick.AddListener(() => PlaySound(index));

        answerButtons[i].onClick.RemoveAllListeners();
        answerButtons[i].onClick.AddListener(() => OnSelectAnswer(index));
    }

    confirmButton.onClick.RemoveAllListeners();
    confirmButton.onClick.AddListener(() => CheckAnswer());
}

    void PlaySound(int index)
    {
        var quiz = randomizedQuizzes[currentQuizIndex];
        soundManager.PlaySound(quiz.options[index]);
    }

void OnSelectAnswer(int index)
{
    selectedIndex = index;
    confirmButton.interactable = true;

    // 全てのボタンの色を元に戻す
    for (int i = 0; i < answerButtons.Length; i++)
    {
        Image img = answerButtons[i].GetComponent<Image>();
        img.color = Color.white;  // 元の色
    }

    // 選択したボタンだけ色を変更
    Image selectedImg = answerButtons[index].GetComponent<Image>();
    selectedImg.color = Color.yellow;  // 強調色
}


    void CheckAnswer()
    {
        var quiz = randomizedQuizzes[currentQuizIndex];
        bool isCorrect = (quiz.correctIndex == selectedIndex);

        resultTitleText.text = isCorrect
            ? "<color=#FF0000>正解！</color>"
            : "<color=#0000FF>不正解…</color>";

        explanationText.text =
            $"A: {quiz.explanationA}\n" +
            $"B: {quiz.explanationB}\n" +
            $"C: {quiz.explanationC}";

        soundManager.PlaySound(isCorrect ? correctSound : wrongSound);

        // 解説画面のレプレイボタン設定
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            replayButtons[i].onClick.RemoveAllListeners();
            replayButtons[i].onClick.AddListener(() => PlaySound(index));
        }

        ShowResultScreen();
    }

    public void OnNextButton()
    {
        currentQuizIndex++;
        if (currentQuizIndex >= randomizedQuizzes.Count)
        {
            Debug.Log("全問終了！");
            // TODO: 終了処理をここに追加（例：再スタート、スコア表示など）
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
