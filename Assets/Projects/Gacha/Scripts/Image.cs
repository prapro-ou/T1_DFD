using UnityEngine;
using UnityEngine.UI;
using Projects.Core;

namespace Projects.Gacha
{
    public class Image : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData; // �v���C���[�f�[�^
        [SerializeField] private Sprite[] bodyImage; // ���炾�̉摜
        [SerializeField] private Sprite[] faceImage; // ��̉摜
        [SerializeField] private Sprite[] clothingImage; // �����̉摜

        [SerializeField] private int defaultManClothesId = 15; // 男性の基本の服装ID
        [SerializeField] private int defaultWomenClothesId = 1; // 女性の基本の服装ID
        
        [SerializeField] private int DefaultManSocksId = 19; // 男,靴下の基本ID
        [SerializeField] private int DefaultWomenSocksId = 5; // 女,靴下の基本ID


        public GameObject bodyImageObject; // ���炾�̉摜��\������I�u�W�F�N�g
        public GameObject faceImageObject; // ��̉摜��\������I�u�W�F�N�g
        public GameObject clothesImageObject; // ���̉摜��\������I�u�W�F�N�g
        public GameObject socksImageObject; // �C���̉摜��\������I�u�W�F�N�g

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // 
        public void reload()
        {
            // ���炾�Ɗ�̕\��
            bodyImageObject.GetComponent<SpriteRenderer>().sprite = bodyImage[playerData.status[0]];
            faceImageObject.GetComponent<SpriteRenderer>().sprite = faceImage[5 * (playerData.status[0] - 1) + (playerData.status[1] + 1)];
            clothesImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[2]];
            socksImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[3]];
        }

        // 服をリセット
        public void resetClothes()
        {
            if (playerData.status[0] == 1) // 男性
            {
                playerData.status[2] = defaultManClothesId; // 服装を初期化
                playerData.status[3] = DefaultManSocksId; // 靴下を初期化
            }
            else // 女性
            {
                playerData.status[2] = defaultWomenClothesId; // 服装を初期化
                playerData.status[3] = DefaultWomenSocksId; // 靴下を初期化
            }
            reload(); // 画像を更新
        }

        void Start()
        {
            // ���炾�Ɗ�̕\��
            bodyImageObject.GetComponent<SpriteRenderer>().sprite = bodyImage[playerData.status[0]];
            faceImageObject.GetComponent<SpriteRenderer>().sprite = faceImage[5 * (playerData.status[0] - 1) + (playerData.status[1] + 1)];
            clothesImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[2]];
            socksImageObject.GetComponent<SpriteRenderer>().sprite = clothingImage[playerData.status[3]];
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // �����̉摜���X�V���郁�\�b�h
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