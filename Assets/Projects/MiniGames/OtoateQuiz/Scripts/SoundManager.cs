using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 自動再生はオフ
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// 他の音を止めて1つだけ再生する
    /// </summary>
    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            audioSource.Stop();               // ✅ まず止める
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip is null!");
        }
    }

    /// <summary>
    /// 再生中の音を止める
    /// </summary>
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
