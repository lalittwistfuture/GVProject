using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
namespace Ludo
{


    public class ColorSelection : MonoBehaviour
    {
        Color Red;
        Color Blue;
        Color Green;
        Color Yellow;

        GameObject[] colorButton;


        private void Start()
        {
            Red = new Color(255.0f / 255.0f, 17.0f / 255.0f, 5.0f / 255.0f);
            Blue = new Color(0.0f / 255.0f, 156.0f / 255.0f, 249.0f / 255.0f);
            Green = new Color(150.0f / 255.0f, 207.0f / 255.0f, 6.0f / 255.0f);
            Yellow = new Color(255.0f / 255.0f, 192.0f / 255.0f, 5.0f / 255.0f);
            this.colorButton = new GameObject[4];
            this.colorButton[0] = GameObject.Find("RED");
            this.colorButton[1] = GameObject.Find("GREEN");
            this.colorButton[2] = GameObject.Find("YELLOW");
            this.colorButton[3] = GameObject.Find("BLUE");
        }



        void onReceivedServerMessage(string sender, string message)
        {
            try{
            JSONNode s = JSON.Parse(message);
            switch (s[ServerTags.TAG])
            {

                case ServerTags.ROOM_INFO:
                    {
                        JSONNode pl = s[ServerTags.ROOM_DATA];
                        string player = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
                        for (int i = 0; i < pl.Count; i++)
                        {
                            JSONNode data = pl[i];
                            Debug.Log(data[ServerTags.DISPLAY_NAME] + "Color " + data[ServerTags.COLOR]);
                            if (int.Parse(data[ServerTags.COLOR]) > -1)
                            {
                                LudoDelegate.usedColor(data[ServerTags.DISPLAY_NAME], int.Parse(data[ServerTags.COLOR]));
                                if(player.Equals(data[ServerTags.PLAYER_ID])){
                                    colorAssign(int.Parse(data[ServerTags.COLOR]));
                                }


                            }
                        }


                    }

                    break;
            }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        void onColorUsed(string playerName, int color)
        {
            try
            {
                Debug.Log("Button is false " + color);
                if (PlayerPrefs.GetInt(LudoTags.USER_LIMIT) == 2)
                {
                    
                    int i = 1;
                    switch (color)
                    {
                        case 1:
                            i = 3;
                            break;
                        case 2:
                            i = 4;
                            break;
                        case 3:
                            i = 1;
                            break;
                        case 4:
                            i = 2;
                            break;
                        default:
                            break;
                    }

                    for (int j = 0; j < 4; j++){
                        if(!(j == color-1 || j == i-1)){
                           
                            this.colorButton[j].GetComponent<Button>().enabled = false;
                            Color c = this.colorButton[j].GetComponent<Image>().color;
                            c.a = 0.5f;
                            this.colorButton[j].GetComponent<Image>().color = c;
                        }else{
                           
                            this.colorButton[j].GetComponent<Button>().enabled = true;
                            Color c = this.colorButton[j].GetComponent<Image>().color;
                            c.a = 1.0f;
                            this.colorButton[j].GetComponent<Image>().color = c; 
                        } 
                    }
                }
              
                this.colorButton[color - 1].transform.Find("Text").GetComponent<Text>().text = playerName;
                this.colorButton[color - 1].GetComponent<Button>().enabled = false;
              
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void OnEnable()
        {
            LudoDelegate.onColorUsed += onColorUsed;
            LudoDelegate.onReceivedServerMessage += onReceivedServerMessage;
        }

        private void OnDisable()
        {
            LudoDelegate.onColorUsed -= onColorUsed;
            LudoDelegate.onReceivedServerMessage -= onReceivedServerMessage;
        }

        public void defaultColor()
        {
            LudoDelegate.selectColor(this.Red, "Player1", 1);
            LudoDelegate.selectColor(this.Yellow, "Player2", 3);
            LudoDelegate.selectColor(this.Green, "Player3", 2);
            LudoDelegate.selectColor(this.Blue, "Player4", 4);
        }


        void colorAssign(int index){
            switch (index)
            {
                case 1:
                    LudoDelegate.selectColor(this.Red, "Player1", 1);
                    LudoDelegate.selectColor(this.Yellow, "Player2", 3);
                    LudoDelegate.selectColor(this.Green, "Player3", 2);
                    LudoDelegate.selectColor(this.Blue, "Player4", 4);

                    break;
                case 2:
                    LudoDelegate.selectColor(this.Green, "Player1", 2);
                    LudoDelegate.selectColor(this.Blue, "Player2", 4);
                    LudoDelegate.selectColor(this.Yellow, "Player3", 3);
                    LudoDelegate.selectColor(this.Red, "Player4", 1);

                    break;
                case 4:
                    LudoDelegate.selectColor(this.Blue, "Player1", 4);
                    LudoDelegate.selectColor(this.Green, "Player2", 2);
                    LudoDelegate.selectColor(this.Red, "Player3", 1);
                    LudoDelegate.selectColor(this.Yellow, "Player4", 3);

                    break;
                case 3:
                    LudoDelegate.selectColor(this.Yellow, "Player1", 3);
                    LudoDelegate.selectColor(this.Red, "Player2", 1);
                    LudoDelegate.selectColor(this.Blue, "Player3", 4);
                    LudoDelegate.selectColor(this.Green, "Player4", 2);

                    break;
            } 
        }

        public void selectColor(GameObject btn)
        {

            switch (btn.transform.name)
            {
                case "RED":

                    colorAssign(1);
                    break;
                case "GREEN":
                  
                    colorAssign(2);
                    break;
                case "BLUE":
                  
                    colorAssign(4);
                    break;
                case "YELLOW":
                    
                    colorAssign(3);
                    break;
            }
        }
    }
}
