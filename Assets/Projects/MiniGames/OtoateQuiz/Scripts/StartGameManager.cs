using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    public GameObject startScreenUI;   // スタート画面UI
    public GameObject quizManagerUI;   // QuizManagerがついているUI

    public void OnStartButtonPressed()
    {

        if (startScreenUI != null)
            startScreenUI.SetActive(false);

        if (quizManagerUI != null)
            quizManagerUI.SetActive(true);

        QuizManager quizManager = quizManagerUI.GetComponentInChildren<QuizManager>();
        if (quizManager != null)
        {
            quizManager.StartQuiz();
        }
        else
        {
            Debug.LogWarning("QuizManager not found on quizManagerUI!");
        }
    }
}