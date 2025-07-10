using UnityEngine;

public class SuimarikoFlowMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 水マリコの移動速度（調整可能）

    private float screenRightEdge; // 画面の右端のワールド座標
    private float screenLeftEdge;  // 画面の左端のワールド座標

    void Start()
    {
        // メインカメラの情報を取得
        Camera mainCamera = Camera.main;

        // カメラのorthographicSizeとaspectから画面の端のワールド座標を計算
        // orthographicSizeはカメラの半分の高さを表す
        // aspectは画面の横縦比
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // 画面の左端と右端のワールド座標を計算
        screenLeftEdge = mainCamera.transform.position.x - halfWidth;
        screenRightEdge = mainCamera.transform.position.x + halfWidth;

        // 初期位置を画面の左端より少し外側に設定して、最初から流れ始めるようにする
        // オブジェクトの幅も考慮すると自然に見える
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // スプライトの半分の幅を取得
            float spriteHalfWidth = sr.bounds.extents.x;
            transform.position = new Vector3(screenLeftEdge - spriteHalfWidth, transform.position.y, transform.position.z);
        }
        else
        {
            // SpriteRendererがない場合のフォールバック（例：単純に画面外から開始）
            transform.position = new Vector3(screenLeftEdge - 1.0f, transform.position.y, transform.position.z);
        }
    }

    void Update()
    {
        // 右方向に移動させる
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 画面の右端を超えたら、左端に戻す
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteHalfWidth = sr.bounds.extents.x;
            // スプライトの右端が画面の右端を超えたかチェック
            if (transform.position.x - spriteHalfWidth > screenRightEdge)
            {
                Destroy(gameObject);
                // // 左端に戻す（スプライトの幅を考慮して、完全に画面外から始まるように）
                // transform.position = new Vector3(screenLeftEdge - spriteHalfWidth, transform.position.y, transform.position.z);
            }
        }
        else
        {
            // SpriteRendererがない場合のフォールバック
            if (transform.position.x > screenRightEdge + 1.0f) // 適当なマージンを設ける
            {
                transform.position = new Vector3(screenLeftEdge - 1.0f, transform.position.y, transform.position.z);
            }
        }
    }
}