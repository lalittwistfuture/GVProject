using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lotto
{
    public class Splash_Page : MonoBehaviour
    {
        public GameObject Login;
        public GameObject signup;
        public GameObject Play;
        // Use this for initialization
        void Start()
        {
          
            if (PlayerPrefs.GetString(PlayerDetails.Name) != null)
            {

                SceneManager.LoadSceneAsync(SceneClass.MAIN_LOBBY);
            }

        }





        public void DoStuff()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.LOGIN);

        }
        public void Do()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.SIGN_UP);
        }

        public void Guest()
        {
            SoundManager.buttonClick();
            SceneManager.LoadSceneAsync(SceneClass.MAIN_LOBBY);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
