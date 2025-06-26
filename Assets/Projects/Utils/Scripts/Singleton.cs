// Singleton.cs
using UnityEngine;
namespace Projects.Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                // DontDestroyOnLoad(this.gameObject); // シーンをまたいで存在させたい場合
            }
            else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}