using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Project.Minigame.Stopwatch
{

    public class StopwatchScript : MonoBehaviour
    {
        private float countup = 0.0f;

        public TMP_Text timeText;
        public TMP_Text goaltimeText;
        public TMP_Text ButtonText;

        // playerDataへの参照
        [SerializeField] private Projects.Core.PlayerData playerData;

        private bool isClick = false;

        private bool isbActive = false;

        private double goalTime = 0.0f;

        private AudioSource audioSource = null;
        public AudioClip StartSE;
        public AudioClip EndSE;

        public void PlaySE(AudioClip clip)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.Log("audioSource=null");
            }
        }

        public void OnStart()
        {
            isbActive = true;
            ButtonText.text = "Stop";
            PlaySE(StartSE);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            ButtonText.text = "Start";
            timeText.text = "0.00";

            goalTime = Random.Range(8, 20);
            goaltimeText.text = (goalTime + "秒で止めよう");

        }

        // Update is called once per frame
        void Update()
        {
            if (isbActive)
            {
                if (isClick)
                {
                    timeText.enabled = true;
                    return;
                }
                countup += Time.deltaTime;
                timeText.text = countup.ToString("f2");
                if (countup >= 2)
                {
                    timeText.enabled = false;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    // stopしたときの処理
                    timeText.text = countup.ToString("f2");
                    isClick = true;
                    PlaySE(EndSE);

                    double result = countup - goalTime;
                    if (result < 1e-6)
                    {
                        // 目標時間より早い場合の処理
                        float tmp = 1.0f + (float)result;
                        int score = (int)Mathf.Max(0.0f, tmp * 50);
                        Debug.Log("目標時間より早い: " + score);
                        playerData.AddPoint(score); // スコア加算
                    }
                    else if (result > 1e-6)
                    {
                        // 目標時間より遅い場合の処理
                        float tmp = 1.0f - (float)result;
                        int score = (int)Mathf.Max(0.0f, tmp * 50);
                        Debug.Log("目標時間より遅い: " + score);
                        playerData.AddPoint(score); // スコア加算
                    }
                    else
                    {
                        // 目標時間に近い場合の処理
                        Debug.Log("ピッタリ");
                        playerData.AddPoint(100); // スコア加算
                    }
                }
            }
        }
    }
}
