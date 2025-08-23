using UnityEngine;

namespace Projects.Gacha
{
    public class Animation : MonoBehaviour
    {
        private float initialPosX;
        private float initialPosY;
        private float xd;
        private int num;
        private float b;
        private int i;
        private Color color;
        private int typeId;
        private int clothingId;
        private bool isRare;
        private GameObject imageScript;
        private GameObject audioScript;

        // ガチャで出た服装を受け取るメソッド
        public void SetClothingId(int a, int b, bool c)
        {
            typeId = a;
            clothingId = b;
            isRare = c;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Application.targetFrameRate = 30;

            initialPosX = 0f;
            initialPosY = 0f;
            num = 0;
            xd = 0f;

            float initialPosValue = Random.value * 6f;
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    initialPosX = -10f;
                    initialPosY = -6f + initialPosValue;
                    num = (int)(30f / (initialPosValue * 0.06 + 1));
                    break;
                case 1:
                    initialPosX = -10f + initialPosValue;
                    initialPosY = -6f;
                    num = 30;
                    break;
                case 2:
                    initialPosX = 10f - initialPosValue;
                    initialPosY = -6f;
                    num = 30;
                    break;
                case 3:
                    initialPosX = 10f;
                    initialPosY = -6f + initialPosValue;
                    num = (int)(30f / (initialPosValue * 0.06 + 1));
                    break;
            }
            xd = -1 * initialPosX / num;

            b = 0f;
            if (direction < 2)
            {
                b = (3 * initialPosX + Mathf.Sqrt(3 * initialPosX * initialPosX * (3 - initialPosY))) / initialPosY;
            }
            else
            {
                b = (3 * initialPosX - Mathf.Sqrt(3 * initialPosX * initialPosX * (3 - initialPosY))) / initialPosY;
            }

            transform.position = new Vector3(initialPosX, initialPosY, 0f);
            color = gameObject.GetComponent<SpriteRenderer>().material.color;

            imageScript = GameObject.Find("ImageScript");
            audioScript = GameObject.Find("AudioScript");
            i = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (i < num)
            {
                Vector3 pos = transform.position;
                pos.x += xd;
                pos.y = -3 / (b * b) * (pos.x - b) * (pos.x - b) + 3;
                transform.position = pos;
                i++;
            }
            else
            {
                if (i < num + 10)
                {
                    transform.localScale = new Vector3((0.1f * (i - num + 1) + 1), (0.1f * (i - num + 1) + 1), 0);

                    color.a -= 0.1f;
                    gameObject.GetComponent<Renderer>().material.color = color;

                    i++;
                }
                else
                {
                    imageScript.GetComponent<Image>().UpdateClothingImage(typeId, clothingId);
                    audioScript.GetComponent<Audio>().PlayGachaResultSound(isRare);
                    Destroy(gameObject);
                }
            }
        }
    }
}