using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Lotto
{
    public class ProfileScript : MonoBehaviour
    {
        public GameObject userName;
        public GameObject password;
        public GameObject mobile;
        public GameObject state;
        public GameObject city;



        // Use this for initialization
        void Start()
        {
            userName.GetComponent<InputField>().text = PlayerPrefs.GetString(PlayerDetails.Name);
            password.GetComponent<InputField>().text = "**********";
            mobile.GetComponent<InputField>().text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_MOBILE);

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Cancel()
        {
            transform.gameObject.SetActive(false);
        }
        public void Saves()
        {
            transform.gameObject.SetActive(false);
        }
    }
}