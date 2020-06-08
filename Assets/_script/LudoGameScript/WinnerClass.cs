using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class WinnerClass : MonoBehaviour
    {
        
        private GameObject winnerPlayer;
        private Image winnerLogo;
        // Use this for initialization
        void Start()
        {
            try
            {
                winnerLogo = transform.Find("Trophy").GetComponent<Image>();
                winnerPlayer = transform.Find("PlayerName").gameObject;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public void showWinner(string playerName,bool isWin)
        {
            winnerPlayer.transform.Find("Text").GetComponent<Text>().text = playerName;
            if (isWin)
            {
                winnerLogo.sprite = Resources.Load<Sprite>("LudoImage/Trophy");
            }
            else
            {
                winnerLogo.sprite = Resources.Load<Sprite>("LudoImage/Bet_Ludo");
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}