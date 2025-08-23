using UnityEngine;

// Assets/Createメニューから作成できるようにする属性
namespace Projects.Core
{

    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        // 共有したいポイント
        public int point; // ガチャポイント
        public int[] status; // [性別(男性:1,女性2), 若返りレベル(0～4), 着ている服, 履いている靴下]
        public int[] clothingItems; // 所持している服装のリスト

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
            status = new int[4];
        }
    }
}