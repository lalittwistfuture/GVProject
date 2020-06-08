﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class yellowPanelScript : MonoBehaviour
    {

        private GameObject[] dices;
        // Use this for initialization
        void Start()
        {

            try
            {
                dices = GameObject.FindGameObjectsWithTag("yellowDices");
                foreach (GameObject g in dices)
                {
                    g.SetActive(false);
                }
            }
            catch (System.Exception ex)
            {

                Debug.Log("yellowDices Exception " + ex.Message);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}