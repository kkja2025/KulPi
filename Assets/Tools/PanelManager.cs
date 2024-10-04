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

        Image loadingBar = GetSingleton("loading").transform.Find("Container/LoadingBarContainer/LoadingBar").GetComponent<Image>();
        loadingBar.fillAmount = 0;

        if (string.IsNullOrEmpty(sceneName))
        {
            float simulatedProgress = 0f;

            while (simulatedProgress < 1f)
            {
                simulatedProgress += Time.deltaTime * 0.2f; 
                loadingBar.fillAmount = simulatedProgress; 
                yield return null;
            }

            Close("loading"); 
            yield break; 
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadingBar.fillAmount = asyncLoad.progress / 0.9f;

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; 
        }

        Close("loading");
    }
}