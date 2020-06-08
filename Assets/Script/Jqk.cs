using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace jkq
{
    public class Jqk : MonoBehaviour
    {
        public GameObject play;
        public GameObject history;


        // Use this for initialization
        void Start()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            history.SetActive(false);
        }
        public void Foo()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.JKQ_GAME);
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
            SceneManager.LoadSceneAsync("MainLobby");
        }
        public void History()
        {
            SoundManager.buttonClick();
            history.SetActive(true);
        }
    }
}