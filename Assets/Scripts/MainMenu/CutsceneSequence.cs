using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; 

public class CutsceneSequence : Panel
{
    [SerializeField] private VideoPlayer videoPlayer; 
    [SerializeField] private VideoClip[] videoClips;
    [SerializeField] private string nextSceneName;
    private int currentVideoIndex = 0;

    public override void Open()
    {
        base.Open();
        AudioManager.Singleton.GetBackgroundMusicSource().Stop();
        Play();
    }

    private void Play()
    {
        if (videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[currentVideoIndex]; 
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play(); 
        }
        else
        {
            Debug.LogWarning("No videos assigned to the sequence!");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        currentVideoIndex++;
        if (currentVideoIndex < videoClips.Length)
        {
            videoPlayer.clip = videoClips[currentVideoIndex];
            videoPlayer.Play();
        }
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                PanelManager.LoadSceneAsync(nextSceneName);
            }
        }
    }
}
