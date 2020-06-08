using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Ludo
{
    public class NumberSelectionPanel : MonoBehaviour
    {

        public GameObject Panel;
        public GameObject[] Button;
        string playerID;

        int goti;
        void Start()
        {
            try
            {
               // Button = GameObject.FindGameObjectsWithTag("SelectionButton");
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public  void onShowSelection(string PlayerID, JSONNode TurnValue, Color color, Vector3 pos,int goti1)
        {
            transform.position = pos;
            hideButton();
            this.goti = goti1;
            transform.SetSiblingIndex(500);
            this.playerID = PlayerID;
            for (int i = 0; i < TurnValue.Count; i++)
            {
                Button[i].SetActive(true);
                Button[i].transform.GetComponent<Image>().color = color;
                Button[i].transform.GetChild(0).GetComponent<Text>().text = TurnValue[i];
            }
            MovePanel(TurnValue);
        }

        void MovePanel(JSONNode TurnValue)
        {
            int Position = GotiScript.getGotiPosition(transform.gameObject);
            switch (Position)
            {
                case GotiScript.LEFT:
                    if (TurnValue.Count == 2)
                    {
                        Panel.transform.localPosition = new Vector3(34.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    else
                    {
                        Panel.transform.localPosition = new Vector3(78.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    break;
                case GotiScript.RIGHT:
                    if (TurnValue.Count == 2)
                    {
                        Panel.transform.localPosition = new Vector3(-34.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    else
                    {
                        Panel.transform.localPosition = new Vector3(-78.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    break;
                case GotiScript.TOP:
                    Panel.transform.localPosition = new Vector3(0.0f, Panel.transform.localPosition.y, 0.0f);
                    break;
                case GotiScript.BOTTOM:
                    Panel.transform.localPosition = new Vector3(0.0f, Panel.transform.localPosition.y, 0.0f);
                    break;
                default:
                    Panel.transform.localPosition = new Vector3(0.0f, Panel.transform.localPosition.y, 0.0f);
                    break;
            }


        }

        void hideButton(){
            for (int i = 0; i < Button.Length; i++)
            {
                Button[i].SetActive(false);
               
            }
        }


        public void numberSelection(GameObject btn)
        {
            LudoDelegate.selectNumber(int.Parse(btn.transform.GetChild(0).GetComponent<Text>().text), this.playerID,this.goti);
            transform.SetSiblingIndex(0);
            hideButton();
          
        }

		private void OnEnable()
		{
            LudoDelegate.onShowSelection += onShowSelection;
		}

		private void OnDisable()
		{
            LudoDelegate.onShowSelection -= onShowSelection;
		}


        public int getGotiPosition(GameObject goti)
        {
            GameObject left = GameObject.Find("72");
            GameObject right = GameObject.Find("60");
            GameObject upper = GameObject.Find("53");
            GameObject bottom = GameObject.Find("65");
            if (transform.position.x < left.transform.position.x)
            {
                return GotiScript.LEFT;
            }
            if (transform.position.x > right.transform.position.x)
            {
                return GotiScript.RIGHT;
            }
            if (transform.position.y > upper.transform.position.y)
            {
                return GotiScript.TOP;
            }
            if (transform.position.y < bottom.transform.position.y)
            {
                return GotiScript.BOTTOM;
            }

            return GotiScript.CENTER;

        }

	}
}