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

            pointText.text = "ポイント: " + playerData.point.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}