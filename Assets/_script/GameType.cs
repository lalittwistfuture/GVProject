using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class GameType : MonoBehaviour
    {

        public Text Type;
        public Text EntryFee;
        public Text winnerCoin;

        // Use this for initialization
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {


        }


        public void updateGameType()
        {

            Type.text = "1 on 1";

        }

        public void selectCoin()
        {
            GameConstantData.entryFee = int.Parse(EntryFee.text);
            GameConstantData.winingAmount = int.Parse(winnerCoin.text);
            SceneManager.LoadSceneAsync("GameScene");
        }

    }
}