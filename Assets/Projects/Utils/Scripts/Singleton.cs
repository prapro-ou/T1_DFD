using UnityEngine;

namespace Projects.Utils
{
    /// <summary>
    /// 堅牢なシングルトン基底クラス。
    /// 永続化するかどうかは、派生クラスが仮想プロパティをオーバーライドすることで決定します。
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        /// <summary>
        /// シングルトンインスタンスを取得します。
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // シーンから既存のインスタンスを探す
                    _instance = FindObjectOfType<T>();

                    // シーンに存在しない場合は動的に生成する
                    if (_instance == null)
                    {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// このシングルトンが永続的（DontDestroyOnLoad）であるべきかを示します。
        /// 派生クラスでこのプロパティをオーバーライドして、永続化の挙動を定義してください。
        /// デフォルトは false (永続化しない) です。
        /// </summary>
        protected virtual bool IsPersistent => false;

        /// <summary>
        /// インスタンスの初期化と重複インスタンスの破棄を処理します。
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                // このインスタンスをシングルトンとして設定
                _instance = this as T;
            }
            else if (_instance != this)
            {
                // 既にインスタンスが存在する場合は、重複なのでこのオブジェクトを破棄
                Debug.LogWarning($"Singleton of type {typeof(T).Name} already exists. Destroying duplicate.");
                Destroy(this.gameObject);
                return;
            }

            // このインスタンスがシングルトンであることが確定した後、
            // IsPersistentプロパティの値に基づいて永続化を決定する
            if (IsPersistent)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
