using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Lotto
{
    public class LottoTicket : MonoBehaviour
    {

        [SerializeField]
        Text ticketNumber;
        int ticketID;
        public string ticketNumberValue;
        public bool isSelected = false;
        public bool isOpen = true;
        // Use this for initialization
        void Start()
        {

        }
        public void addTicket(int id, string number)
        {
            this.ticketID = id;
            this.ticketNumberValue = number;
            this.ticketNumber.text = this.ticketNumberValue;
        }



        public void click()
        {
            if (isOpen)
            {
                if (isSelected)
                {
                    isSelected = false;
                    GetComponent<Image>().color = Color.white;
                }
                else
                {
                    GetComponent<Image>().color = Color.grey;
                    isSelected = true;
                }
                LottoGameController.clickTicket();
            }
        }



    }
}