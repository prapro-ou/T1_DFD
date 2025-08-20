using UnityEngine;

public class BouncingEnemy : MonoBehaviour
{
    public float speed = 3f;        // 横方向の速さ
    public float bounceHeight = 2f; // 跳ねる高さ
    public float bounceSpeed = 4f;  // 跳ねる速さ

    private Vector3 startPos;
    private float screenRightEdge;
    private float screenLeftEdge;

    void Start()
    {
        startPos = transform.position;

        // カメラの画面端を計算
        Camera mainCamera = Camera.main;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        screenLeftEdge = mainCamera.transform.position.x - halfWidth;
        screenRightEdge = mainCamera.transform.position.x + halfWidth;
    }

    void Update()
    {
        // 横に移動
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // sin波で上下に跳ねる
        float newY = startPos.y + Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed)) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 画面外に出たら消す
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
