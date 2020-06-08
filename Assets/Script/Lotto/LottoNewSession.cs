using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Lotto
{
    public class LottoNewSession : MonoBehaviour
    {

        [SerializeField]
        Text Timer;
        DateTime nextSessionTime;
        bool callNextSession = false;
        // Use this for initialization
        private void OnEnable()
        {
            callNextSession = true;
            DateTime.TryParse(PlayerPrefs.GetString(LOTTOGame.NEXT_SESSION), out this.nextSessionTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (this.callNextSession)
            {
                Timer.text = "0" + (this.nextSessionTime - DateTime.Now).Minutes + ":" + (this.nextSessionTime - DateTime.Now).Seconds;

                if (DateTime.Now >= this.nextSessionTime)
                {
                    this.callNextSession = false;
                    Debug.Log("Call next session");
                    LottoGameController.startSession();
                }
                if ((this.nextSessionTime - DateTime.Now).Seconds < 10)
                {
                    Timer.text = "0" + (this.nextSessionTime - DateTime.Now).Minutes + ":" + "0" + (this.nextSessionTime - DateTime.Now).Seconds;
                }
            }
        }
    }
}