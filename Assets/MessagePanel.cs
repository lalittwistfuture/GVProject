using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class MessagePanel : MonoBehaviour
    {


        string[] message = { "Play Fast", "Hahahahaha", "Game is still on","Today is my day","LOL","#$%@#$@#%$", "Well Played","Hard Luck","I am Lucky","You can't beat me","All the best","I am the Best" };
        public GameObject cellSample;
        // Use this for initialization
        void Start()
        {

            GameObject Parent = transform.Find("Viewport").Find("Content").gameObject;
            foreach (string msg in message)
            {
                GameObject go = Instantiate(cellSample);
                go.transform.SetParent(Parent.transform);
                go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                go.transform.Find("Text").GetComponent<Text>().text = msg;
                go.GetComponent<Button>().onClick.AddListener(() => clickBtn(go));
            }


        }

        void clickBtn(GameObject btn)
        {
            Debug.Log(btn.transform.Find("Text").GetComponent<Text>().text);
            appwarp.sendMessageChat(btn.transform.Find("Text").GetComponent<Text>().text);
            transform.gameObject.SetActive(false);
        }

       
    }
}