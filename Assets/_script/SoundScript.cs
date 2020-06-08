using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ludo
{
    public class SoundScript : MonoBehaviour
    {

        public AudioClip GotiSound;
        public AudioClip DiceSound;
        public AudioClip closeSound;
        public AudioClip TurnSound;
        public AudioClip ClockSound;
        public AudioClip ClappingSound;
        public AudioClip CutGotiSound;
        public AudioClip safePlaceSound;
        public AudioClip backGroundSound;


        private void Start()
        {

            Debug.Log("Sound is now " + PlayerPrefs.GetInt(GameTags.MUSIC_ON));

            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[1].clip = backGroundSound;
                GetComponents<AudioSource>()[1].loop = true;
                GetComponents<AudioSource>()[1].Play();

            }
            else
            {
                GetComponents<AudioSource>()[1].Stop();
            }
        }
        private void OnEnable()
        {

            LudoDelegate.onDiceSound += onDiceSound;
            LudoDelegate.onGotiWin += onClappingSound;
            LudoDelegate.onClapSound += onClapSound;
            LudoDelegate.onTurnChange += onTurnChange;
            LudoDelegate.onGotiSound += onGotiSound;
            LudoDelegate.onSafePlaceSound += onSafePlaceSound;
            LudoDelegate.onCutGoti += onCloseGoti;
        }

        private void OnDisable()
        {
            LudoDelegate.onGotiSound -= onGotiSound;
            LudoDelegate.onDiceSound -= onDiceSound;
            LudoDelegate.onGotiWin -= onClappingSound;
            LudoDelegate.onClapSound -= onClapSound;
            LudoDelegate.onTurnChange -= onTurnChange;
            LudoDelegate.onSafePlaceSound -= onSafePlaceSound;
            LudoDelegate.onCutGoti -= onCloseGoti;
        }



        void onSafePlaceSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[0].clip = safePlaceSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void onGotiSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[0].clip = GotiSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void onCloseGoti()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[0].clip = closeSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void onTurnChange(string playerID)
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[0].clip = TurnSound;
                GetComponents<AudioSource>()[0].Play();
            }
            if (playerID.Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (PlayerPrefs.GetInt(GameTags.VIBRATION_ON) == 1)
                    {
#if UNITY_ANDROID
                        Handheld.Vibrate();
#endif
                    }
                }
            }
        }

        void onDiceSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {

                GetComponents<AudioSource>()[0].clip = DiceSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }


        void onTimerSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponents<AudioSource>()[0].clip = ClockSound;
                GetComponents<AudioSource>()[0].Play();
            }

        }

        void onClapSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponent<AudioSource>().Stop();
                GetComponents<AudioSource>()[0].clip = ClappingSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void onClappingSound(int index, string playerID)
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponent<AudioSource>().Stop();
                GetComponents<AudioSource>()[0].clip = ClappingSound;
                GetComponents<AudioSource>()[0].Play();
            }
        }

        void onCutGotiSound()
        {
            if (PlayerPrefs.GetInt(GameTags.SOUND_ON) == 1)
            {
                GetComponent<AudioSource>().Stop();
                GetComponents<AudioSource>()[0].clip = CutGotiSound;
                GetComponents<AudioSource>()[0].Play();
            }

        }
    }
}