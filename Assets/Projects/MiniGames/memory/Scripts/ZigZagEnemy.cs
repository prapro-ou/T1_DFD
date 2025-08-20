using UnityEngine;

public class ZigZagEnemy : MonoBehaviour
{
    public float speed = 3f;       // 前進の速さ
    public float zigzagWidth = 2f; // ジグザグの幅
    public float zigzagSpeed = 3f; // ジグザグの速さ

    private float startY;
    private float screenRightEdge;
    private float screenLeftEdge;
    void Start()
    {
        startY = transform.position.y;

        // カメラの画面端を計算
        Camera mainCamera = Camera.main;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        screenLeftEdge = mainCamera.transform.position.x - halfWidth;
        screenRightEdge = mainCamera.transform.position.x + halfWidth;
    }

    void Update()
    {
        // 前に進む
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 上下にジグザグ
        float newY = startY + Mathf.Sin(Time.time * zigzagSpeed) * zigzagWidth;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 画面外に出たら消す
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float spriteHalfWidth = sr.bounds.extents.x;
            if (transform.position.x - spriteHalfWidth > screenRightEdge ||
                transform.position.x + spriteHalfWidth < screenLeftEdge)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.x > screenRightEdge + 1.0f ||
                transform.position.x < screenLeftEdge - 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
