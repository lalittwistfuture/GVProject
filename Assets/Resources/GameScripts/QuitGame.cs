using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Roullet
{
    public class QuitGame : MonoBehaviour
    {


        public GameObject msgText;
        // Use this for initialization
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }


        public void yesAction()
        {

            SceneManager.LoadSceneAsync("MainLobby");  

        }

        public void noAction()
        {
            transform.gameObject.SetActive(false);
        }
    }
}