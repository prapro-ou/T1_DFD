using UnityEngine;

// Assets/Createメニューから作成できるようにする属性
namespace Projects.Core
{

    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        // 共有したいポイント
        public int point;

        // ポイントを加算するメソッド
        public void AddPoint(int amount)
        {
            point += amount;
            Debug.Log($"Point added! Current points: {point}");
        }

        // ゲーム開始時などにポイントをリセットするメソッド
        public void ResetPoint()
        {
            point = 0;
        }
    }
}