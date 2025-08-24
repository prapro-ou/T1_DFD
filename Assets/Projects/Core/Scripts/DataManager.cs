using UnityEngine;
using System.IO; // ファイルの読み書きに必要
using Projects.Utils;

namespace Projects.Core
{
    // データの保存と読み込みを担当するマネージャークラス
    public class DataManager : Singleton<DataManager>
    {
        // インスペクターから操作したいScriptableObjectをセットする
        [SerializeField] private PlayerData gameData;

        // publicプロパティを用意して、外部からgameDataを読み取れるようにする
        public PlayerData GameData => gameData;

        private string savePath;
        private const string SAVE_FILE_NAME = "gamedata.json";

        // このシングルトンはシーンをまたいで永続化させたいので、プロパティをオーバーライドしてtrueを返す
        protected override bool IsPersistent => true;

        // Awakeメソッドをオーバーライドする
        // 必ず基底クラスのAwakeを最初に呼び出すことが重要！
        protected override void Awake()
        {
            // ★★★ シングルトンの初期化処理を正しく実行するために必須 ★★★
            base.Awake();

            // 独自の初期化処理
            savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
            LoadData();
        }

        // アプリケーション終了時に呼ばれる
        private void OnApplicationQuit()
        {
            SaveData();
        }

        // アプリがバックグラウンドに移った時などに呼ばれる（モバイル向け）
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveData();
            }
        }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("データを保存しました: " + savePath);
        }

        public void LoadData()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                JsonUtility.FromJsonOverwrite(json, gameData);
                Debug.Log("データをロードしました: " + savePath);
            }
            else
            {
                Debug.Log("セーブファイルが見つかりません。デフォルト値を使用します。");
            }
        }
    }
}