using UnityEngine;

namespace Core
{
    // 常に目標のアスペクト比を維持するように、カメラの表示領域を調整するスクリプト
    [RequireComponent(typeof(Camera))]
    public class AspectRatioController : MonoBehaviour
    {
        // 目標のアスペクト比 (例: 16:9 = 1.777)
        [SerializeField]
        private float targetAspect = 16.0f / 9.0f;

        private Camera targetCamera;

        void Start()
        {
            targetCamera = GetComponent<Camera>();
            UpdateViewportRect();
        }

        // 画面サイズが変更されたときに呼び出されるように、Updateで監視する
        // （エディタでの変更や、PCでのウィンドウサイズ変更に対応するため）
        void Update()
        {
            UpdateViewportRect();
        }

        private void UpdateViewportRect()
        {
            // 現在の画面のアスペクト比
            float screenAspect = (float)Screen.width / (float)Screen.height;

            // 目標のアスペクト比に対する、現在の画面の比率
            float scaleRatio = screenAspect / targetAspect;

            Rect newRect = new Rect(0, 0, 1, 1);

            if (scaleRatio > 1.0f) // 画面が目標より「横長」の場合
            {
                // 左右に黒帯（ピラーボックス）を入れる
                newRect.width = 1.0f / scaleRatio;
                newRect.x = (1.0f - newRect.width) / 2.0f;
            }
            else // 画面が目標より「縦長」の場合
            {
                // 上下に黒帯（レターボックス）を入れる
                newRect.height = scaleRatio;
                newRect.y = (1.0f - newRect.height) / 2.0f;
            }

            // 計算したRectをカメラのViewport Rectに設定
            if (targetCamera.rect != newRect)
            {
                targetCamera.rect = newRect;
            }
        }
    }
}