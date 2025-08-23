using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Projects.Core;


namespace Projects.Home
{

    public class PointShower : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // 
        [SerializeField] private TextMeshProUGUI pointText;
        [SerializeField] private PlayerData playerData;
        private int point;
        void Start()
        {
            if (playerData == null)
            {
                Debug.LogError("PlayerData is not assigned in the inspector!");
                return;
            }
            if (pointText == null)
            {
                Debug.LogError("PointText is not assigned in the inspector!");
                return;
            }
            point = playerData.point;

            pointText.text = "ポイント: " + this.point.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if(playerData.point != point)
            {
                point = playerData.point;
                pointText.text = "ポイント: " + this.point.ToString();
            }
        }
    }

}