using UnityEngine;

public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _init;

    public static T Instance {
        get {
            if (_init != null)
                return _init;
            var obj = new GameObject(typeof(T).Name);
            _init = obj.AddComponent<T>();
            return _init;
        }
    }

    protected void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}

public class SingleTon<T> where T : new() {
    private static T _init;

    public static T Instance {
        get {
            if (_init != null)
                return _init;
            _init = new T();
            return _init;
        }
    }
}