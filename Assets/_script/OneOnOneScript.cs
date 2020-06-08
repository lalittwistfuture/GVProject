using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class OneOnOneScript : MonoBehaviour
    {


        int coin = 0;
        public GameObject click10Select;
        public GameObject click50Select;
        public GameObject click100Select;
        public GameObject click200Select;
        public GameObject click500Select;
        public GameObject click1000Select;
        public GameObject PlayNowBtn;
        public Text Title;

        // Use this for initialization
        void Start()
        {
            //PlayNowBtn.GetComponent<Button> ().interactable = false;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Showpopup(string msg)
        {
            Title.text = msg;
            Click10();
        }
        public void PlayNow()
        {
            //print ("coin is " + coin);
            if (coin > 0)
            {
                GameConstantData.entryFee = coin;
                SceneManager.LoadSceneAsync("GameScene");
            }
            else
            {
                GameConstantData.showToast(transform, "First select the coin to start game.");
            }
        }
        public void ClosedPanelAction()
        {

            //print ("Closedpanel working");
            coin = 0;
            transform.gameObject.SetActive(false);
        }

        public void selectCoin(GameObject background, int value)
        {
            //PlayNowBtn.GetComponent<Button> ().interactable = true;
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins_selected");
            coin = value;
        }

        public void DeSelectAll()
        {
            click10Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            click50Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            click100Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            click200Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            click500Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            click1000Select.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
        }

        public void Click10()
        {
            DeSelectAll();
            selectCoin(click10Select, 10);
        }

        public void Click50()
        {
            DeSelectAll();
            selectCoin(click50Select, 50);

        }

        public void Click100()
        {
            DeSelectAll();
            selectCoin(click100Select, 100);

        }

        public void Click200()
        {
            DeSelectAll();
            selectCoin(click200Select, 200);

        }

        public void Click500()
        {
            DeSelectAll();
            selectCoin(click500Select, 500);

        }

        public void Click1000()
        {
            DeSelectAll();
            selectCoin(click1000Select, 1000);
        }

    }
}