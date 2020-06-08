using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Roullet
{
    public class ChipScript : MonoBehaviour
    {

        private GameObject[] BetButton;

        void Start()
        {
            BetButton = GameObject.FindGameObjectsWithTag("chipCoin");
            PlayerPrefs.SetInt(RouletteTag.RouletteSelectedCoin, 0);
            foreach (GameObject btn in BetButton)
            {
                btn.GetComponent<Button>().onClick.AddListener(() => TaskOnClick(btn));
                //btn.getcom.onClick.AddListener(TaskOnClick);
            }
        }

        void TaskOnClick(GameObject btn)
        {

            foreach (GameObject btn1 in BetButton)
            {
                //btn1.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                iTween.ScaleTo(btn1, new Vector3(1.0f, 1.0f, 1.0f), 0.5f);
                //btn.getcom.onClick.AddListener(TaskOnClick);
            }
            iTween.ScaleTo(btn, new Vector3(1.2f, 1.2f, 1.2f), 0.5f);

            //btn.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            print("Chip " + btn.transform.name);
            PlayerPrefs.SetInt(RouletteTag.RouletteSelectedCoin, int.Parse(btn.transform.name));
            switch (int.Parse(btn.transform.name))
            {

                case 500:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettec5");
                    }
                    break;
                case 100:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettec4");
                    }
                    break;
                case 50:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettec3");
                    }
                    break;
                case 10:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettec2");
                    }
                    break;
                case 5:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettec1");
                    }
                    break;
                case 1:
                    {
                        PlayerPrefs.SetString(RouletteTag.RouletteCoinImage, "Roulettechip");
                    }
                    break;
            }
        }

    }
}