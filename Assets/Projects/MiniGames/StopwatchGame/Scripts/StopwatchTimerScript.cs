using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Project.Minigame.Stopwatch
{

    public class StopwatchScript : MonoBehaviour
    {
        private float countup = 0.0f;

        public TMP_Text timeText;

        private bool isClick = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isClick)
            {
                timeText.enabled = true;
                return;
            }
            countup += Time.deltaTime;
            timeText.text = countup.ToString("f1");
            if (countup > 2)
            {
                timeText.enabled = false;
            }

            if (Input.GetMouseButtonDown(0))
                {
                    timeText.text = countup.ToString("f1");
                    isClick = true;
                }
        }
    }
}
