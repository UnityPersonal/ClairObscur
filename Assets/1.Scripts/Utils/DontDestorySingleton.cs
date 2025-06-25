using UnityEngine;

public class DontDestorySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private bool singletonInitialized = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                DontDestroyOnLoad(instance.gameObject);
                
                DontDestorySingleton<T> singleton = instance.GetComponent<DontDestorySingleton<T>>();
                singleton.Initialize();
            }
            return instance;
        }
    }
    
    public virtual void Initialize() { singletonInitialized = true; }
    
    protected virtual void Awake()
    {
        if(singletonInitialized && instance.Equals(this))
        {
            Debug.LogWarning($"Singleton {typeof(T).Name} is already initialized. Destroying duplicate instance.");
            return;
        }

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Initialize();
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
