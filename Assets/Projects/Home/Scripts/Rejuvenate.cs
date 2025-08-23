using UnityEngine;

namespace Projects.Home
{
    // 若返りレベルを管理するスクリプト
    public class Rejuvenate : MonoBehaviour
    {
        private int rejuvenateLevel; // 若返りレベル
        [SerializeField] private int rankupPoint = 400; // レベルアップに必要なポイント
        [SerializeField] private Projects.Core.PlayerData playerData; // プレイヤーデータ

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            int point = playerData.point;
            rejuvenateLevel = point / rankupPoint; // 400ポイントごとに若返りレベルが1上がる

            int sex = playerData.status[0]; // 性別を取得
            if (sex == 1)// 男性
            {
                if (rejuvenateLevel > 3)
                {
                    rejuvenateLevel = 3; // 男性の最大レベルは3
                }
            }
            else
            {
                if (rejuvenateLevel > 4)
                {
                    rejuvenateLevel = 4; // 女性の最大レベルは4
                }
            }

            int beforeLevel = playerData.status[1]; // 以前の若返りレベルを取得
            if (rejuvenateLevel > beforeLevel)
            {
                playerData.status[1] = rejuvenateLevel; // 若返りレベルを更新
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}