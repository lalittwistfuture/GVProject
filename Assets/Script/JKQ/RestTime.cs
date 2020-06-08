using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace jkq
{
    public class RestTime : MonoBehaviour
    {
        public Text time;
        public Text timetype;
        DateTime endBetTime;
        int lastSeconds = 0;
        // Use this for initialization
        void Start()
        {

        }

        private void OnEnable()
        {

            DateTime.TryParse(PlayerPrefs.GetString(JKQGame.NEXT_SESSION), out this.endBetTime);
            timetype.text = "Next session will start in...";
        }

        // Update is called once per frame
        void Update()
        {
            if (DateTime.Now < this.endBetTime)
            {
                if (lastSeconds != (this.endBetTime - DateTime.Now).Seconds)
                {
                    SoundManager.buttonClick();
                    lastSeconds = (this.endBetTime - DateTime.Now).Seconds;
                }
                time.text = "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + (this.endBetTime - DateTime.Now).Seconds;
            }
            if ((this.endBetTime - DateTime.Now).Seconds < 10 && (this.endBetTime - DateTime.Now).Seconds >= 0)
            {
                time.text = "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + "0" + (this.endBetTime - DateTime.Now).Seconds;

            }
        }
    }
}