using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class MiddlePanel : MonoBehaviour
    {


        public GameObject LeftBtn;
        public GameObject RightBtn;
        public GameObject[] Indicator;
        public Transform Left;
        public Transform Rignt;
        public Transform Mid;
        public GameObject OneOnOne;
        public GameObject friends;
        public GameObject practice;
        public GameButtonType[] GameButton;
        int count = 0;

        void Start()
        {
            this.GameButton = new GameButtonType[3];

            GameButtonType g = new GameButtonType();
            g.Button = OneOnOne;
            g.index = 0;
            this.GameButton[0] = g;
            GameButtonType g1 = new GameButtonType();
            g1.Button = friends;
            g1.index = 1;
            this.GameButton[1] = g1;
            GameButtonType g2 = new GameButtonType();
            g2.Button = practice;
            g2.index = 1;
            this.GameButton[2] = g2;

            foreach (GameButtonType te in this.GameButton)
            {
                this.DirectMove(te);
            }

            UnSelectedGame();
            SelectedGame(this.getCurrentCenterIndex());
        }

        void DirectMove(GameButtonType button)
        {
            switch (button.index)
            {
                case 0:
                    button.Button.transform.position = Mid.position;
                    break;
                case 1:
                    button.Button.transform.position = Rignt.position;
                    break;
                case -1:
                    button.Button.transform.position = Left.position;
                    break;
            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        int getCurrentCenterIndex()
        {
            for (int i = 0; i < 3; i++)
            {
                GameButtonType g = GameButton[i];
                if (g.index == 0)
                {
                    return i;
                }

            }
            return -1;
        }

        public void LeftClick()
        {
            int currentIndex = this.getCurrentCenterIndex();
            //Debug.Log (currentIndex);
            this.GameButton[currentIndex].index = -1;
            Move(this.GameButton[currentIndex].Button, Left);
            currentIndex++;
            if (currentIndex == 3)
            {
                currentIndex = 0;
            }
            //Debug.Log ("Next Button " + currentIndex);
            this.GameButton[currentIndex].index = 1;
            DirectMove(this.GameButton[currentIndex]);
            this.GameButton[currentIndex].index = 0;
            Move(this.GameButton[currentIndex].Button, Mid);

            UnSelectedGame();
            SelectedGame(currentIndex);

        }

        public void RightClick()
        {
            int currentIndex = this.getCurrentCenterIndex();
            ///Debug.Log (currentIndex);
            this.GameButton[currentIndex].index = 1;
            Move(this.GameButton[currentIndex].Button, Rignt);
            currentIndex++;
            if (currentIndex == 3)
            {
                currentIndex = 0;
            }
            //Debug.Log ("Next Button " + currentIndex);
            this.GameButton[currentIndex].index = -1;
            DirectMove(this.GameButton[currentIndex]);
            this.GameButton[currentIndex].index = 0;
            Move(this.GameButton[currentIndex].Button, Mid);
            UnSelectedGame();
            SelectedGame(currentIndex);


        }

        IEnumerator EnableButton()
        {
            yield return new WaitForSeconds(0.5f);
            LeftBtn.GetComponent<Button>().interactable = true;
            LeftBtn.GetComponent<Button>().interactable = true;
        }

        public void SelectedGame(int myCount)
        {
            GameObject g = Indicator[myCount];
            g.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected");
        }

        public void UnSelectedGame()
        {
            foreach (GameObject g in Indicator)
            {
                g.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected");
            }
        }

        void Move(GameObject g, Transform pos)
        {
            iTween.MoveTo(g, pos.position, 1.0f);

        }




    }


    public class GameButtonType
    {
        public int index = 0;
        public GameObject Button;
    }





}