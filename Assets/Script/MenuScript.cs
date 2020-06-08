using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Lotto
{
    public class MenuScript : MonoBehaviour
    {
        public GameObject log;
        public GameObject myProfile;
        public GameObject logOut;
        // Use this for initialization
        void Start()
        {
            logOut.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void myAccount()
        {
            SoundManager.buttonClick();
            myProfile.SetActive(true);
            transform.gameObject.SetActive(false);
        }

        public void closeAction()
        {
            SoundManager.buttonClick();
            transform.gameObject.SetActive(false);
        }
        public void LogOut()
        {
            SoundManager.buttonClick();
            PlayerPrefs.DeleteAll();
            logOut.SetActive(true); ;

        }
    }
}