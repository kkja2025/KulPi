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
        Singleton.StartCoroutine(Singleton.LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        CloseAll();
        Open("loading");

        // Find the LoadingBar in the LoadingPanel
        Image loadingBar = GetSingleton("loading").transform.Find("Container/LoadingBarContainer/LoadingBar").GetComponent<Image>();
        loadingBar.fillAmount = 0; // Reset the loading bar fill amount to 0

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // This variable is to hold the progress value.
        float progress = 0f;

        while (!asyncLoad.isDone)
        {
            // Update the loading bar fill amount based on progress
            // The progress value will be between 0 and 1.
            // We can manually increase it until we reach 0.9 to show smooth loading.
            if (asyncLoad.progress < 0.9f)
            {
                progress = asyncLoad.progress;
            }
            else
            {
                progress = Mathf.Lerp(0.9f, 1f, Time.deltaTime * 10f); // Smoothly transition to 1
                if (progress >= 1f)
                {
                    progress = 1f;
                    asyncLoad.allowSceneActivation = true; // Allow scene activation
                }
            }
            Debug.Log($"Loading progress: {progress}");
            loadingBar.fillAmount = progress; // Update the loading bar

            yield return null; 
        }

        Close("loading"); // Close the loading panel after the scene is loaded
    }

}