using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class ShareRefrelCode : MonoBehaviour
    {

        public Text refrelCode;

        // Use this for initialization
        void Start()
        {
            refrelCode.text = PlayerPrefs.GetString(GetPlayerDetailsTags.REFREL_CODE);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadSceneAsync("LobiScene");

            }
        }

        public void BackbuttonAction()
        {
            SceneManager.LoadSceneAsync("LobiScene");
        }

        public void ShareAction()
        {
            ///string msg = "Enter this code to earn mony " + PlayerPrefs.GetString (GetPlayerDetailsTags.REFREL_CODE);
            /// 
            /// Sumit Kumar invited you to play Ludo Money. Please enter referral code 0975345and get free 50 Rs worth of coins. Please download the game from: 
            string msg = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME) + " invited you to play Dreamz Club Ludo. Please enter referral code " + PlayerPrefs.GetString(GetPlayerDetailsTags.REFREL_CODE) + " and get free " + PlayerPrefs.GetString(Tags.REFERRAL_COIN_FOR_OLD_PLAYER) + " Rs worth of coins. Please download the game from: " + PlayerPrefs.GetString(Tags.APP_DOWNLOAD_URL);
            GameConstantData.shareText(msg);
        }
    }
}