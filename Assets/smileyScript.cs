using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class smileyScript : MonoBehaviour
    {

        public GameObject buttonSample;

        // Use this for initialization
        void Start()
        {

            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                gameObject.SetActive(true);

                for (int i = 1; i <= 46; i++)
                {
                    GameObject news = Instantiate(buttonSample);
                    news.transform.SetParent(transform);
                    news.transform.localScale = new Vector3(1, 1, 1);
                    news.GetComponent<Image>().sprite = Resources.Load<Sprite>("Emoji/E_" + i);
                    news.name = "Emoji/E_" + i;
                }
            }else{
                gameObject.SetActive(false);
            }
        }

        public void sendSmiley(GameObject btn)
        {
            Debug.Log("Smiley " + btn.name);
            appwarp.sendSmiley(btn.name);
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
