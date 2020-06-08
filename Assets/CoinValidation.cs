using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LudoGame
{
    public class CoinValidation : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void backAction(){
            SceneManager.LoadSceneAsync("LobiScene");
        }
    }
}