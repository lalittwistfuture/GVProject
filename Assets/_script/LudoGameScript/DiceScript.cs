using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class DiceScript : MonoBehaviour
    {

        bool isRotation = false;
        int number = 0;
        string currentPlayer = "";
        public bool enable = false;
        // Use this for initialization
        void Start()
        {

        }

        private void OnEnable()
        {
            LudoDelegate.onShowDic += onShowDic;
            LudoDelegate.onDiceRollStart += onDiceRollStart;
        }

        private void OnDisable()
        {
            LudoDelegate.onShowDic -= onShowDic;
            LudoDelegate.onDiceRollStart -= onDiceRollStart;
        }

        public void onShowDic(string player, int num,bool enable)
        {
            this.currentPlayer = player;
            this.number = num;
            this.enable = enable;
        }
        public void onDiceRollStart(string player, int num)
        {
            this.currentPlayer = player;
            this.number = num;
            isRotation = true;
            LudoDelegate.playDiceSound();
            StartCoroutine(stopRotationByOpponent());

        }



        void playerWithDic()
        {
            LudoDelegate.diceClick(this.currentPlayer);
            LudoDelegate.playDiceSound();
            enable = false;
            isRotation = true;
            StartCoroutine(stopRotation());
        }

        public void clickOnDice()
        {
            if (enable)
            {
                playerWithDic();
            }
        }

        IEnumerator stopRotation()
        {
            yield return new WaitForSeconds(0.5f);
            isRotation = false;
            enable = false;
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dices/dice_" + this.number);
            LudoDelegate.diceRollDone(this.currentPlayer, this.number);
            //transform.gameObject.SetActive(false);
        }

        IEnumerator stopRotationByOpponent()
        {
            yield return new WaitForSeconds(0.5f);
            isRotation = false;
            enable = false;
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dices/dice_" + this.number);
            // transform.gameObject.SetActive(false);
        }


        void onHideDic(string player)
        {

        }

        private void FixedUpdate()
        {
            if (isRotation)
            {
                int i = Random.Range(1, 6);
                transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dices/d-" + i);
            }

        }


    }
}
