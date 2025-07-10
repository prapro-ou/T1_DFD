using UnityEngine;
using UnityEngine.SceneManagement;
namespace Project.Minigame.Stopwatch
{
    public class StopwatchChangescene : MonoBehaviour
    {
        public void change_button()
        {
            SceneManager.LoadScene("StopwatchGame.GameScene");
        }
    }
}
