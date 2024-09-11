using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseService : MonoBehaviour
{
    private bool initialized = false;
    private static FirebaseService singleton = null;
    private FirebaseApp app;
    private FirebaseAuth auth;
    private DatabaseReference reference;

    public FirebaseAuth Auth => auth;
    public DatabaseReference Reference => reference;

    public static FirebaseService Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<FirebaseService>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("FirebaseService not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;

        initialized = true;
        DontDestroyOnLoad(gameObject);
        Debug.Log("FirebaseService Initialized");
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
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Firebase initialized successfully.");
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            return;
        }
    }

    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }
}
