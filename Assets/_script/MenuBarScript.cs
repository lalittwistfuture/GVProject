using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class MenuBarScript : MonoBehaviour
    {

       // public GameObject MyAccountPanel;
       // public GameObject RedeemCoinPanel;
      //  public GameObject TransactionPanel;
        public GameObject ClosePanel;
       // public GameObject ChangPasswordPanel;
        public GameObject ChangPasswordBtn;
        public GameObject logoutPopUp;
        // Use this for initialization
        void Start()
        {
           // ChangPasswordPanel.SetActive(false);
            //Debug.Log ("" + PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_FBID));
            if (PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_FBID).Length > 0)
            {
                ChangPasswordBtn.SetActive(false);
            }
            else
            {
                ChangPasswordBtn.SetActive(true);
            }
            logoutPopUp.SetActive(false);
        }

        public void ShowMenuBar()
        {
           // MyAccountPanel.SetActive(false);
          //  RedeemCoinPanel.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MyAccountAction()
        {

           // MyAccountPanel.SetActive(true);
           // MyAccountPanel.GetComponent<PersonalProfile>().ShowPanel();
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void RedeemCoinAction()
        {
            //Debug.Log ("RedeemCoinAction working ");
          //  RedeemCoinPanel.SetActive(true);
         //   RedeemCoinPanel.GetComponent<RedeemPanel>().ShowPanel();
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void TransactionAction()
        {
            Debug.Log("TransactionAction working ");
          //  TransactionPanel.SetActive(true);
//TransactionPanel.GetComponent<TransactionPanel>().ShowPanel();
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void WinAction()
        {
            //Debug.Log ("WinAction working ");
            //Application.OpenURL (PlayerPrefs.GetString(Tags.HOW_TO_WIN));
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void TermConditionAction()
        {
            //Application.OpenURL (PlayerPrefs.GetString(Tags.TERMS_N_CONDITION));
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);

        }

        public void privacyPolicyAction()
        {
            //Debug.Log ("privacyPolicyAction working ");
            //transform.gameObject.SetActive (false);
            //Application.OpenURL (PlayerPrefs.GetString(Tags.PRIVACY_POLICY));
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void AboutUsAction()
        {
            //Application.OpenURL (PlayerPrefs.GetString(Tags.PRIVACY_POLICY));
            transform.gameObject.SetActive(false);
            ClosePanel.SetActive(false);
        }

        public void ReferfrndsAction()
        {
            SceneManager.LoadSceneAsync("ShareApp");
        }

        public void ChangePasswordAction()
        {
            //Debug.Log ("ChangePasswordAction working");
            transform.gameObject.SetActive(false);
           // ChangPasswordPanel.SetActive(true);
          //  ChangPasswordPanel.GetComponent<ChangePassword>().ShowPanel();
        }


        public void ComplaintAction()
        {
            //Debug.Log ("ChangePasswordAction working");
            transform.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync("ComplaintScene");
        }



        public void LogoutAction()
        {
            logoutPopUp.SetActive(true);

        }

        public void HidePanel()
        {
            ClosePanel.SetActive(false);
            transform.gameObject.SetActive(false);

        }



    }
}