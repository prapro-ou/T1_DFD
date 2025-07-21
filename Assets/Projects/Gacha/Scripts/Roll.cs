using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gacha : MonoBehaviour
{
    public GachaData inputJson = new GachaData();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string inputString = Resources.Load<TextAsset>("GachaContents").ToString();
        inputJson = JsonUtility.FromJson<GachaData>(inputString);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Roll(int num)
    {
        int rateSum = 0;
        int rarityResult = 0;
        int contentResult = 0;

        for (int i = 0; i < inputJson.rate.Length; i++)
        {
            rateSum += inputJson.rate[i];
        }
        int rarityValue = (int)(Random.value * rateSum) + 1;

        rateSum = 0;
        for (int i = 0; i < inputJson.rate.Length; i++)
        {
            if (rarityValue > rateSum)
            {
                rarityResult = i;
            }
            rateSum += inputJson.rate[i];
        }

        switch (rarityResult) { 
            case 0:
                contentResult = Random.Range(0, inputJson.contents.rarity0.Length);
                Debug.Log("Rarity 0: " + inputJson.contents.rarity0[Random.Range(0, inputJson.contents.rarity0.Length)]);
                break;
            case 1:
                contentResult = Random.Range(0, inputJson.contents.rarity1.Length);
                Debug.Log("Rarity 1: " + inputJson.contents.rarity1[Random.Range(0, inputJson.contents.rarity1.Length)]);
                break;
            case 2:
                contentResult = Random.Range(0, inputJson.contents.rarity2.Length);
                Debug.Log("Rarity 2: " + inputJson.contents.rarity2[Random.Range(0, inputJson.contents.rarity2.Length)]);
                break;
        }
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
    public string[] rarity0;
    public string[] rarity1;
    public string[] rarity2;
}