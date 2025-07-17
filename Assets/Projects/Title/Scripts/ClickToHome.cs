using UnityEngine;


namespace Projects.Title
{
    public class ClickToHome : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void OnClick()
        {
            // シーンをHomeに遷移
            Projects.Core.StackedSceneManager.Instance.PushScene(Projects.Core.GameScenes.Home);
        }

        void Start()
        {   
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse clicked, transitioning to Home scene.");
                OnClick();
            }
        }
    }

}