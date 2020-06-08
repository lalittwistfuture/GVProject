using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SoundManager : MonoBehaviour
    {
        AudioClip buttonSound;
        AudioClip backgroundSound;
        AudioClip StopBetting;
        AudioClip winnerSound;
        // Use this for initialization
        void Start()
        {
            buttonSound = Resources.Load<AudioClip>("sound/click");
            backgroundSound = Resources.Load<AudioClip>("sound/roulette_background_sound");
            StopBetting = Resources.Load<AudioClip>("sound/stopBetSound");
            winnerSound = Resources.Load<AudioClip>("sound/winner Sound");
            if (GetComponents<AudioSource>().Length > 1)
            {
                GetComponents<AudioSource>()[1].clip = backgroundSound;
                GetComponents<AudioSource>()[1].loop = true;
                GetComponents<AudioSource>()[1].Play();
            }
        }
        public delegate void ButtonClick();
        public static event ButtonClick onButtonClick;
        public static void buttonClick()
        {
            if (onButtonClick != null)
            {
                onButtonClick();
            }
        }


        private void OnEnable()
        {
            SoundManager.onButtonClick += SoundButton;
            GameController.onBettingStop += onBettingStop;
            GameController.onStartAnimation += WinnerSound;
        }

        private void OnDisable()
        {
            SoundManager.onButtonClick -= SoundButton;
            GameController.onBettingStop -= onBettingStop;
            GameController.onStartAnimation -= WinnerSound;
        }

        void onBettingStop()
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                GetComponents<AudioSource>()[0].clip = StopBetting;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void SoundButton()
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                GetComponents<AudioSource>()[0].clip = buttonSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void WinnerSound()
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                GetComponents<AudioSource>()[0].clip = winnerSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
