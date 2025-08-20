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
    public AudioClip goodResultSound;
    public AudioClip badResultSound;


    private List<Quiz> randomizedQuizzes = new List<Quiz>();
    private int currentQuizIndex = 0;
    private int selectedIndex = -1;
    private int correctCount = 0; // ✅ 正解数カウント

    void Start()
{
    quizScreen.SetActive(false);
    resultScreen.SetActive(false);
}


    public void StartQuiz()
{
    currentQuizIndex = 0;
    correctCount = 0;
    selectedIndex = -1;
    randomizedQuizzes.Clear();

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

           // ✅ 再生ボタンを戻す
        foreach (Button btn in replayButtons)
        { 
        btn.gameObject.SetActive(true);
        }

        // ボタン色リセット
        for (int i = 0; i < answerButtons.Length; i++)
        {
            var btn = answerButtons[i];
            Image img = btn.GetComponent<Image>();
            img.color = Color.white;

            var colors = btn.colors;
            colors.normalColor = Color.white;
            btn.colors = colors;
        }

        var quiz = randomizedQuizzes[currentQuizIndex];
        promptText.text = quiz.prompt;

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

        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image img = answerButtons[i].GetComponent<Image>();
            img.color = Color.white;
        }

        Image selectedImg = answerButtons[index].GetComponent<Image>();
        selectedImg.color = Color.yellow;
    }

    void CheckAnswer()
    {
        var quiz = randomizedQuizzes[currentQuizIndex];
        bool isCorrect = (quiz.correctIndex == selectedIndex);

        if (isCorrect)
        {
            correctCount++; // ✅ 正解数カウント
        }

        resultTitleText.text = isCorrect
            ? "<color=#FF0000>正解！</color>"
            : "<color=#0000FF>不正解…</color>";

        explanationText.text =
            $"A: {quiz.explanationA}\n" +
            $"B: {quiz.explanationB}\n" +
            $"C: {quiz.explanationC}";

        soundManager.PlaySound(isCorrect ? correctSound : wrongSound);

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
        // ✅ 全問終了：最終結果を表示
        resultTitleText.text = $"<size=150%><color=green>全{randomizedQuizzes.Count}問中 {correctCount}問正解！</color></size>";
        explanationText.text = "おつかれさまでした！";

        nextButton.gameObject.SetActive(false); // 「次へ」非表示

        // ✅ 再生ボタンすべて非表示
        foreach (Button btn in replayButtons)
        {
            btn.gameObject.SetActive(false);
        }

        if (correctCount >= 3)
        {
            soundManager.PlaySound(goodResultSound); // 3問以上ならお祝い音
        }
        else
        {
            soundManager.PlaySound(badResultSound);     // それ以外は普通の音
        }

            return;
    }

    // 通常の問題へ
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
        nextButton.gameObject.SetActive(true); // 次ボタンは毎回表示
    }
} 