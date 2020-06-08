using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Lotto
{

    public class Create : MonoBehaviour
    {
        public GameObject history;

        // Use this for initialization
        void Start()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            history.SetActive(false);
        }
        public void New()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.LUTTO_GAME);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
        public void Back()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.MAIN_LOBBY);
        }
        public void Open()
        {
            SoundManager.buttonClick();
            history.SetActive(true);
        }
    }


}