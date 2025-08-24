using UnityEngine;
using UnityEngine.UI;

namespace Projects.Home
{
    // 性別変換スクリプト
    public class TransSeibetu : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private Projects.Core.PlayerData playerData; // プレイヤーデータ

        [SerializeField] private Projects.Gacha.Image imageScript; // Imageスクリプトがアタッチされているオブジェクト


        void onClick()
        {
            Debug.Log("TransSeibetu onClick");
            if (playerData.status[0] == 1) // 男性
            {
                playerData.status[0] = 2; // 女性に変更
            }
            else
            {
                playerData.status[0] = 1; // 男性に変更
            }


            // 服を初期化
            imageScript.resetClothes();
            // imageScript.reload(); // 画像を更新

        }
        void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(onClick);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}