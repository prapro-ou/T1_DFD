using UnityEngine;

public class SuimarikoFlowMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;      // 右方向への移動速度
    public float jumpHeight = 1.0f;     // ジャンプの高さ（上下の振れ幅）
    public float jumpFrequency = 2.0f;  // ジャンプの頻度（サイン波の速さ）
    public float rotationSpeed = 180f;  // 回転速度（度/秒）

    private float screenRightEdge;
    private float screenLeftEdge;
    private float startY;
    private float jumpOffset; // 各個体でジャンプの開始位相をずらす
    private float actualRotationSpeed;

    void Start()
    {
        Camera mainCamera = Camera.main;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        screenLeftEdge = mainCamera.transform.position.x - halfWidth;
        screenRightEdge = mainCamera.transform.position.x + halfWidth;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteHalfWidth = sr.bounds.extents.x;
            transform.position = new Vector3(screenLeftEdge - spriteHalfWidth, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(screenLeftEdge - 1.0f, transform.position.y, transform.position.z);
        }

        startY = transform.position.y;
        jumpOffset = Random.Range(0f, 2f * Mathf.PI); // サイン波の開始位相をランダム化
        actualRotationSpeed = Random.Range(-rotationSpeed, rotationSpeed); // 回転速度をランダムに
    }

    void Update()
    {
        // 右へ移動
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);

        // 上下にジャンプ（サイン波）
        float jumpY = Mathf.Sin(Time.time * jumpFrequency + jumpOffset) * jumpHeight;
        transform.position = new Vector3(transform.position.x, startY + jumpY, transform.position.z);

        // 回転
        transform.Rotate(Vector3.forward, actualRotationSpeed * Time.deltaTime);

        // 画面外に出たら削除
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteHalfWidth = sr.bounds.extents.x;
            if (transform.position.x - spriteHalfWidth > screenRightEdge)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.x > screenRightEdge + 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
