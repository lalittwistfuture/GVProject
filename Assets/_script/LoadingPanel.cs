using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class LoadingPanel : MonoBehaviour
    {

        public GameObject loading;
        public GameObject msgText;

        bool isTimerActivated = false;
        int counter = 0;
        int Timerlimit = 0; 
        string message = "";

        public void startTimer(int limit,string text){
            Timerlimit = limit;
            message = text;
            isTimerActivated = true;
            InvokeRepeating("timerUpdate", 1.0f, 1.0f);
        }

        // Use this for initialization
        void Start()
        {

        }

        void timerUpdate(){
            if (isTimerActivated)
            {
                if (counter < Timerlimit)
                {
                    counter++;
                }
                else
                {
                    CancelInvoke("timerUpdate");
                    appwarp.ServerMessage = message;
                    isTimerActivated = false;
                    appwarp.Disconnect();
                }
            }
        }


		private void OnDisable()
		{
            Debug.Log("Timer disable");
            stopTimer();
		}
		public void stopTimer(){
            CancelInvoke("timerUpdate");
        }



        public void UpdateMessage(string message){
           // msgText.GetComponent<Text>().text = message;
        }



        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (loading.activeSelf)
            {
                int i = Random.Range(1, 6);
                loading.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dices/d-" + i);
            }
            msgText.GetComponent<Text>().text = appwarp.ServerMessage;
        }


    }
}
