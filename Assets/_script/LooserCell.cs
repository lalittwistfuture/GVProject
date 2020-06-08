using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class LooserCell : MonoBehaviour
    {

        public GameObject playerName;
        public GameObject PlayerImage;
        public GameObject PlayerRank;

        // Use this for initialization
        void Start()
        {

        }

        public void updateLoserCell(string name, string rank)
        {
            playerName.GetComponent<Text>().text = name;

            //	PlayerRank.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("");

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}