using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class TimerScript : MonoBehaviour
    {


        Text tim;
        int counter = 0; 
        public void startTimer(int value)
        {
            counter = value;
             
        }

        void timerUpdate(){
            counter--;
            if (counter == 0){
                transform.gameObject.SetActive(false);
            }
        }

        // Use this for initialization
        void Start()
        {
            try{
                tim = transform.Find("Text").GetComponent<Text>();
                InvokeRepeating("timerUpdate", 1.0f, 1.0f);
            }catch(System.Exception ex){
                Debug.Log(ex.Message);
            }
        }

        // Update is called once per frame
        void Update()
        {
            tim.text = ""+counter;
        }
    }
}