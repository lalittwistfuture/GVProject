using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace jkq
{
    public class CurtonLoading : MonoBehaviour
    {
        public GameObject loadingScreenLeft;
        public GameObject loadingScreenRight;
        public GameObject rotate;

        // Use this for initialization
        void Start()
        {
            loadingScreenLeft.SetActive(true);
            loadingScreenRight.SetActive(true);
            rotate.SetActive(true);
        }

        private void OnEnable()
        {
            GameController.onStartGame += onStartGame;
        }

        private void OnDisable()
        {
            GameController.onStartGame -= onStartGame;
        }

        void onStartGame()
        {
            rotate.SetActive(false);
            iTween.MoveTo(loadingScreenLeft, iTween.Hash("position", new Vector3(-630, 0, 0), "easetype", iTween.EaseType.easeInOutSine, "time", 3f));
            iTween.MoveTo(loadingScreenRight, iTween.Hash("position", new Vector3(630, 0, 0), "easetype", iTween.EaseType.easeInOutSine, "time", 3f, "oncomplete", "closeWindow", "oncompletetarget", this.gameObject));

        }

        void closeWindow()
        {
            transform.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (rotate.activeSelf)
            {
                rotate.transform.Rotate(Vector3.back, 5f);
            }
        }
    }
}