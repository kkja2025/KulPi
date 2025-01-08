using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; 

public class CutsceneSequence : Panel
{
    public VideoPlayer videoPlayer; 
    public VideoClip[] videoClips;
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
            Debug.Log("All videos finished. Redirecting to Chapter 1.");
            SceneManager.LoadScene("Chapter1"); 
        }
    }
}
