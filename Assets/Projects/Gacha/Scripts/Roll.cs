using System;
using System.IO;
using Projects.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

namespace Projects.Gacha
{
    public class Gacha : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData; // プレイヤーデータ
        private GachaData inputJson = new GachaData(); // ガチャデータ
        private ClothingItems[] clothingItems = new ClothingItems[27]; // 服装データ（種類の数+1を指定）
        [SerializeField] private TextAsset clothingData;　// 服装データのCSVファイル
        public TextMeshProUGUI clothingNameText; // 服装名の表示用テキスト
        public GameObject sample; // 画面外から飛んでくる服が入った箱的なもの
        public GameObject audioScript; // Audioスクリプトがアタッチされているオブジェクト

        class ClothingItems // 服装データのクラス
        {
            public int id;
            public string name;
            public string file;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // ガチャデータの読み込み
            string inputString;
            if (playerData.status[0] == 1) // 男性
            {
                inputString = Resources.Load<TextAsset>("GachaContents1").ToString();
            }
            else if (playerData.status[0] == 2) // 女性
            {
                inputString = Resources.Load<TextAsset>("GachaContents2").ToString();
            }
            else
            {
                Debug.LogError("性別が設定されていません。");
                return;
            }
            inputJson = JsonUtility.FromJson<GachaData>(inputString);

            Debug.Log("Gacha data loaded successfully.");

            // 服装データの読み込み
            StringReader dataReader = new StringReader(clothingData.text);
            int i = 0;
            while (dataReader.Peek() != -1)
            {
                string lineData = dataReader.ReadLine();
                string[] lineSplit = lineData.Split(',');
                clothingItems[i] = new ClothingItems();
                clothingItems[i].id = int.Parse(lineSplit[0]);
                clothingItems[i].name = lineSplit[1];
                i++;
            }

            // 服装名を表示
            clothingNameText.text = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Roll()
        {
            int rateSum = 0;
            int rarityResult = 0;
            int contentResult = 0;

            // ここでポイントの消費処理を行う
            if (playerData.point < 100)
            {
                Debug.Log("ポイントが足りません。");
                return;
            }
            
            playerData.point -= 100; // ポイントを消費
            Debug.Log("ポイントを消費しました。残りポイント: " + playerData.point);

            // 全てのレアリティの合計値を計算
            for (int i = 0; i < inputJson.rate.Length; i++)
            {
                rateSum += inputJson.rate[i];
            }

            // レアリティの決定
            int rarityValue = (int)(Random.value * rateSum) + 1;
            rateSum = 0;
            rarityResult = -1;
            for (int i = 0; i < inputJson.rate.Length; i++)
            {
                if (rarityValue > rateSum)
                {
                    rarityResult = i;
                }
                rateSum += inputJson.rate[i];
            }

            // レアリティに応じたコンテンツの決定
            switch (rarityResult)
            {
                case 0:
                    contentResult = inputJson.contents.rarity0[Random.Range(0, inputJson.contents.rarity0.Length)];
                    break;
                case 1:
                    contentResult = inputJson.contents.rarity1[Random.Range(0, inputJson.contents.rarity1.Length)];
                    break;
                case 2:
                    contentResult = inputJson.contents.rarity2[Random.Range(0, inputJson.contents.rarity2.Length)];
                    break;
                case 3:
                    contentResult = inputJson.contents.rarity3[Random.Range(0, inputJson.contents.rarity3.Length)];
                    break;
                case 4:
                    contentResult = inputJson.contents.rarity4[Random.Range(0, inputJson.contents.rarity4.Length)];
                    break;
                default:
                    contentResult = 0;
                    break;
            }
            Debug.Log("Rarity " + rarityResult + ": " + contentResult);
            clothingNameText.text = clothingItems[contentResult].name;

            // 手に入れた服を保存
            if (clothingItems[contentResult].id == 1)
            {
                playerData.status[2] = contentResult;
            }
            else if (clothingItems[contentResult].id == 2)
            {
                playerData.status[3] = contentResult;
            }
            else
            {
                Debug.LogError("不明な服装ID: " + clothingItems[contentResult].id);
            }

            // ガチャアニメーション
            audioScript.GetComponent<Audio>().PlayGachaStartSound();
            GameObject instance = Instantiate(sample, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
            Animation component = instance.GetComponent<Animation>();
            component.SetClothingId(clothingItems[contentResult].id, contentResult, (rarityResult >= 2));
        }
    }

    [Serializable]
    public class GachaData
    {
        public int[] rate;
        public GachaContents contents;
    }

    [Serializable]
    public class GachaContents
    {
        public int[] rarity0;
        public int[] rarity1;
        public int[] rarity2;
        public int[] rarity3;
        public int[] rarity4;
    }
}