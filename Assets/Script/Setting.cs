using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Lotto
{
    public class Setting : MonoBehaviour
    {
        public GameObject close;
        public GameObject music;
        public GameObject sound;
        public GameObject vibration;

        // Use this for initialization
        void Start()
        {

        }
        public void Clear()
        {
            SoundManager.buttonClick();
            transform.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void MusicBtn()
        {
            SoundManager.buttonClick();
            if (music.GetComponent<Image>().sprite == Resources.Load<Sprite>("Images/box"))
            {
                music.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/check-box11");
            }
            else
            {
                music.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/box");
            }

        }
        public void SoundBtn()
        {
            SoundManager.buttonClick();
            if (sound.GetComponent<Image>().sprite == Resources.Load<Sprite>("Images/box"))
            {
                sound.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/check-box11");
            }
            else
            {
                sound.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/box");
            }


        }
        public void VibrationBtn()
        {
            SoundManager.buttonClick();
            if (vibration.GetComponent<Image>().sprite == Resources.Load<Sprite>("Images/box"))
            {
                vibration.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/check-box11");
            }
            else
            {
                vibration.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/box");
            }
        }

    }
}