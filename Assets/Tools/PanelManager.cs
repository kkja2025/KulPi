using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    
    private Dictionary<string, Panel> panels = new Dictionary<string, Panel>();
    private bool initialized = false;
    private Canvas[] canvas = null;
    private static PanelManager singleton = null;
    
    public static PanelManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<PanelManager>();
                if (singleton == null)
                {
                    singleton = new GameObject("PanelManager").AddComponent<PanelManager>();
                }
                singleton.Initialize();
            }
            return singleton; 
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
        panels.Clear();
        canvas = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (canvas != null)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                Panel[] list = canvas[i].gameObject.GetComponentsInChildren<Panel>(true);
                if (list != null)
                {
                    for (int j = 0; j < list.Length; j++)
                    {
                        if (string.IsNullOrEmpty(list[j].ID) == false && panels.ContainsKey(list[j].ID) == false)
                        {
                            list[j].Initialize();
                            list[j].Canvas = canvas[i];
                            panels.Add(list[j].ID, list[j]);
                        }
                    }
                }
            }
        }
    }
    
    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }
    
    public static Panel GetSingleton(string id)
    {
        if (Singleton.panels.ContainsKey(id))
        {
            return Singleton.panels[id];
        }
        return null;
    }
    
    public static void Open(string id)
    {
        var panel = GetSingleton(id);
        if (panel != null)
        {
            panel.Open();
        }
    }
    
    public static void Close(string id)
    {
        var panel = GetSingleton(id);
        if (panel != null)
        {
            panel.Close();
        }
    }
    
    public static bool IsOpen(string id)
    {
        if (Singleton.panels.ContainsKey(id))
        {
            return Singleton.panels[id].IsOpen;
        }
        return false;
    }
    
    public static void CloseAll()
    {
        foreach (var panel in Singleton.panels)
        {
            if (panel.Value != null)
            {
                panel.Value.Close();
            }
        }
    }

    public static void LoadSceneAsync(string sceneName)
    {
        try
        {
            Singleton.StartCoroutine(Singleton.LoadSceneCoroutine(sceneName));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading scene. Returning to Login: " + ex.Message);
            SceneManager.LoadScene("Login");
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        CloseAll();
        Open("loading");

        AudioSource soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        if (soundEffectsSource != null)
        {
            soundEffectsSource.Stop();
        }

        Sprite[] loadingSprites = Resources.LoadAll<Sprite>("Icons/Loading/Loading_Book_Animation");
        Image loadingImage = GetSingleton("loading").transform.Find("Container/LoadingBookContainer/LoadingImage").GetComponent<Image>();

        int currentFrame = 0;
        float frameInterval = 0.1f;
        float frameTimer = 0f;

        if (string.IsNullOrEmpty(sceneName))
        {
            float simulatedProgress = 0f;

            while (simulatedProgress < 1f)
            {
                simulatedProgress += Time.deltaTime * 0.2f;
                
                frameTimer += Time.deltaTime;
                if (frameTimer >= frameInterval)
                {
                    currentFrame = (currentFrame + 1) % loadingSprites.Length;
                    loadingImage.sprite = loadingSprites[currentFrame];
                    frameTimer = 0f;
                }

                yield return null;
            }

            Close("loading");
            yield break;
        }

        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // asyncLoad.allowSceneActivation = false;

        // while (!asyncLoad.isDone)
        // {
        //     frameTimer += Time.deltaTime;
        //     if (frameTimer >= frameInterval)
        //     {
        //         currentFrame = (currentFrame + 1) % loadingSprites.Length;
        //         loadingImage.sprite = loadingSprites[currentFrame];
        //         frameTimer = 0f;
        //     }

        //     if (asyncLoad.progress >= 0.9f)
        //     {
        //         asyncLoad.allowSceneActivation = true;
        //     }

        //     yield return null;
        // }

        SceneManager.LoadSceneAsync(sceneName);
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;

            // Update frame animation
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameInterval)
            {
                currentFrame = (currentFrame + 1) % loadingSprites.Length;
                loadingImage.sprite = loadingSprites[currentFrame];
                frameTimer = 0f;
            }

            yield return null;
        }
        Close("loading");
    }

    public void StartLoading(float loadingDuration, Action duringLoadingAction = null, Action postLoadingAction = null)
    {
        StartCoroutine(LoadingCoroutine(loadingDuration, duringLoadingAction, postLoadingAction));
    }

    private IEnumerator LoadingCoroutine(float loadingDuration, Action duringLoadingAction, Action postLoadingAction)
    {
        CloseAll();
        Open("loading");

        AudioSource soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        if (soundEffectsSource != null)
        {
            soundEffectsSource.Stop();
        }

        Sprite[] loadingSprites = Resources.LoadAll<Sprite>("Icons/Loading/Loading_Book_Animation");
        Image loadingImage = GetSingleton("loading").transform.Find("Container/LoadingBookContainer/LoadingImage").GetComponent<Image>();

        int currentFrame = 0;
        float frameInterval = 0.1f;
        float frameTimer = 0f;

        // If custom logic is passed, execute it
        duringLoadingAction?.Invoke();

        // Run the loading animation for the given duration
        float elapsedTime = 0f;
        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            // Update frame animation
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameInterval)
            {
                currentFrame = (currentFrame + 1) % loadingSprites.Length;
                loadingImage.sprite = loadingSprites[currentFrame];
                frameTimer = 0f;
            }

            yield return null;
        }

        // Close the loading screen after the specified time
        Close("loading");

        // Run any logic that should happen after the loading completes
        postLoadingAction?.Invoke();
    }
}