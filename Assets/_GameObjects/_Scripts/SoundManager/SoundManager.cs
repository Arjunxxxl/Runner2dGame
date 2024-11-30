using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioData
    {
        public string audioName;
        public AudioClip clip;
        public List<AudioSource> sources;
    }

    public List<AudioData> audioDatas;

    #region SingleTon
    public static SoundManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    internal void PlayAudio(string audioName)
    {
        for (int i = 0; i < audioDatas.Count; i++)
        {
            AudioData audioData = audioDatas[i];

            if (audioData.audioName == audioName)
            {
                List<AudioSource> sources = audioData.sources;
                for (int j = 0; j < sources.Count; j++)
                {
                    if (!sources[j].isPlaying)
                    {
                        sources[j].clip = audioData.clip;
                        sources[j].pitch = Random.Range(0.9f, 1.1f);
                        sources[j].Play();
                        StopCoroutine(StopAudioSource(sources[j], audioData.clip.length));
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator StopAudioSource(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
    }
}
