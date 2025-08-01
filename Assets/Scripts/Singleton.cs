using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindAnyObjectByType<T>();
            if (_instance != null) return _instance;

            _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}