using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Lotto
{
    public class LogOutGame : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Cancel()
        {
            SoundManager.buttonClick();
            transform.gameObject.SetActive(false);

        }
        public void Yes()
        {
            SoundManager.buttonClick();

            SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
        }
    }
}