using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private bool singletonInitialized = false;

    protected virtual void Initalize() {
        singletonInitialized = true;
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
                var singleTon = _instance.GetComponent<MonoSingleton<T>>();
                singleTon.Initalize();
            }
            return _instance;
        }
    }
}
