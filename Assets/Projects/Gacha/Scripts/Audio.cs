using UnityEngine;

namespace Projects.Gacha
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip gachaStartClip; // ガチャ開始音
        [SerializeField] private AudioClip gachaResultNormalClip; // ガチャ結果音（ノーマル）
        [SerializeField] private AudioClip gachaResultRareClip; // ガチャ結果音（レア）

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // ガチャ開始音を再生するメソッド
        public void PlayGachaStartSound()
        {
            audioSource.PlayOneShot(gachaStartClip);
        }

        // ガチャ結果音を再生するメソッド
        public void PlayGachaResultSound(bool isRare)
        {
            if (isRare)
            {
                audioSource.PlayOneShot(gachaResultRareClip);
            }
            else
            {
                audioSource.PlayOneShot(gachaResultNormalClip);
            }
        }
    }
}