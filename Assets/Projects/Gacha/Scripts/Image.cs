using UnityEngine;
using UnityEngine.UI;
using Projects.Core;

namespace Projects.Gacha
{
    public class Image : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData; // プレイヤーデータ
        [SerializeField] private Sprite[] bodyImage; // からだの画像
        [SerializeField] private Sprite[] faceImage; // 顔の画像
        [SerializeField] private Sprite[] clothingImage; // 服装の画像
        public GameObject bodyImageObject; // からだの画像を表示するオブジェクト
        public GameObject faceImageObject; // 顔の画像を表示するオブジェクト
        public GameObject clothesImageObject; // 服の画像を表示するオブジェクト
        public GameObject socksImageObject; // 靴下の画像を表示するオブジェクト

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // からだと顔の表示
            bodyImageObject.GetComponent<SpriteRenderer>().sprite = bodyImage[playerData.status[0]];
            faceImageObject.GetComponent<SpriteRenderer>().sprite = faceImage[5 * (playerData.status[0] - 1) + (playerData.status[1] + 1)];
            clothesImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[2]];
            socksImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[3]];
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // 服装の画像を更新するメソッド
        public void UpdateClothingImage(int typeId, int clothingId)
        {
            if (typeId == 1)
            {
                clothesImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[clothingId];
            }
            else if (typeId == 2)
            {
                socksImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[clothingId];
            }
        }
    }
}