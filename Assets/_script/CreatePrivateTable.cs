using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class CreatePrivateTable : MonoBehaviour
    {


        int coin = 0;
        public GameObject click50Select;
        public GameObject click100Select;
        public GameObject click200Select;
        public GameObject click500Select;

        public GameObject[] btnArray;


        void Start()
        {
            // closeBtn.GetComponent<Button>().onClick.AddListener(ClosedPanelAction);
            // CreateTableBtn.GetComponent<Button>().onClick.AddListener(CreateTableAction);
            //  JointableBtn.GetComponent<Button>().onClick.AddListener(JoinTableAction);



            //click50Select.GetComponent<Button>().onClick.AddListener(Click50);
            //click100Select.GetComponent<Button>().onClick.AddListener(Click100);
            //click200Select.GetComponent<Button>().onClick.AddListener(Click200);
            //click500Select.GetComponent<Button>().onClick.AddListener(Click500);
            // Click50();
           

           
            // coin = 50;

        }

        void clickBtn(GameObject btn)
        {
            DeSelectAll();
            selectCoin(btn, int.Parse(btn.name));
        }


        public void startGame(){

           
            SceneManager.LoadSceneAsync("GameScene");

        }

		private void OnEnable()
		{
            btnArray = GameObject.FindGameObjectsWithTag("PriceBtn");
            foreach (GameObject btn in btnArray)
            {
                btn.GetComponent<Button>().onClick.AddListener(() => clickBtn(btn));
            }

            btnArray[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins_selected");
            coin = 50;
            PlayerPrefs.SetInt(LudoTags.ENTRY_FEE, coin);

            if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PRIVATE)
            {
                PlayerPrefs.SetString(GameTags.PRIVATE_TABLE_TYPE, GameTags.CREATE_TABLE);
            }

		}

		
        public void CreateTableAction()
        {

            if (coin > 0)
            {

                if (coin <= int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN)))
                {
                    PlayerPrefs.SetInt(LudoTags.ENTRY_FEE, coin);
                    if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PRIVATE)
                    {
                        PlayerPrefs.SetString(GameTags.PRIVATE_TABLE_TYPE, GameTags.CREATE_TABLE);
                    }
                   
                    // SceneManager.LoadSceneAsync("GameScene");
                }
                else
                {
                    GameConstantData.showToast("You do not have sufficient coin.");
                }


            }
            else
            {
                GameConstantData.showToast("first select the coin to start game?");
            }
        }

        public void JoinTableAction()
        {

            PlayerPrefs.SetString(GameTags.PRIVATE_TABLE_TYPE, GameTags.JOIN_TABLE);
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void ClosedPanelAction()
        {

            //print ("Closedpanel working");
            coin = 0;
            transform.gameObject.SetActive(false);
        }

        public void selectCoin(GameObject background, int value)
        {
            background.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins_selected");
            coin = value;
            Debug.Log("Coin selected " + value);
            CreateTableAction();
        }

        public void DeSelectAll()
        {
            foreach (GameObject btn in btnArray)
            {
                btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Button_coins");
            }
        }

    }

}
