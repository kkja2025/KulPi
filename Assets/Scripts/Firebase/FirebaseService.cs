using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseService : MonoBehaviour
{
    private bool initialized = false;
    private static FirebaseService singleton = null;
    private FirebaseApp app;
    private FirebaseAuth auth;
    public FirebaseAuth Auth => auth;

    public static FirebaseService Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<FirebaseService>();
                singleton.Initialize();
            }
            return singleton; 
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }
    
    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    public async void StartClientService()
    {    
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            return;
        }
    }
}