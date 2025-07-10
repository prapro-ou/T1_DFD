using UnityEngine;
using UnityEngine.SceneManagement;
namespace Project.Minigame.Stopwatch
{
    public class StopwatchScenemain : MonoBehaviour
    {
        public void change2main_button()
        {
            SceneManager.LoadScene("Game_Template");
        }
    }
}
