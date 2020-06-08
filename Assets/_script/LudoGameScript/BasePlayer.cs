using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
namespace Ludo
{
    public class BasePlayer : MonoBehaviour
    {
        private GameObject[] boxs;
        public GameObject[] goti;
        string playerID;
        public bool isActive = false;
        string username;
        public Text playerName;
        public int color = 0;
        Color currentColor;
        public bool isComputer = false;
        public JSONArray TurnValue;
        DiceScript dice;
        GameObject diceGameObject;
        GameObject userPic;
        GameObject NameBar;
        GameObject Ring;
        GameObject Mask;
        GameObject diceSample;
        GameObject messagePanel;
        GameObject smileyPanel;
        GameObject dicePanel;
        public string playerTag;
        public GameObject TurnIndicator;
        void Start()
        {
            this.TurnValue = new JSONArray();
            try
            {
                playerTag = transform.tag;
                diceGameObject = transform.Find("Dice").gameObject;
                NameBar = transform.Find("NameBar").gameObject;
                boxs = GameObject.FindGameObjectsWithTag(transform.tag);
                goti = new GameObject[PlayerPrefs.GetInt(LudoTags.GOTI_LIMIT)];
                dice = transform.Find("Dice").GetComponent<DiceScript>();
                userPic = transform.Find("ImageMask").transform.Find("UserImage").gameObject;
                Ring = transform.Find("Ring").gameObject;
                messagePanel = transform.Find("Message Panel").gameObject;
                smileyPanel = transform.Find("Smiley").gameObject;
                Mask = transform.Find("ImageMask").gameObject;
                dicePanel = transform.Find("DicePanel").gameObject;
                diceSample = GameObject.Find("DiceSample");
                diceGameObject.SetActive(false);
                TurnIndicator = transform.Find("TurnIndicator").gameObject;
                Vector3[] vectorList = new Vector3[4];
                vectorList[0] = new Vector3(30, 30, 0);
                vectorList[1] = new Vector3(-30, 30, 0);
                vectorList[2] = new Vector3(-30, -30, 0);
                vectorList[3] = new Vector3(30, -30, 0);
                Transform Parent = GameObject.Find(transform.name + "_Panel").transform;
                playerName = transform.Find("PlayerName").GetComponent<Text>();
                for (int i = 0; i < goti.Length; i++)
                {
                    goti[i] = Instantiate(GameObject.Find("Goti"));
                    goti[i].transform.SetParent(Parent);
                    goti[i].GetComponent<GotiScript>().index = i + 1;
                    goti[i].transform.localPosition = vectorList[i];
                    goti[i].transform.localScale = new Vector3(1, 1, 1);
                    goti[i].GetComponent<GotiScript>().CellNumber = -1;
                    goti[i].transform.SetParent(Parent.parent);
                    goti[i].SetActive(false);

                }
                this.messagePanel.SetActive(false);
                this.smileyPanel.SetActive(false);
                this.Mask.SetActive(false);
                this.Ring.SetActive(false);
                this.NameBar.SetActive(false);
                playerName.text = "";
                this.userPic.SetActive(false);
                this.TurnIndicator.SetActive(false);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

        }

        public void showSmiley(string msg)
        {
            this.smileyPanel.SetActive(true);
            this.smileyPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>(msg);
            StartCoroutine(hideSmileyPanel());
        }

        IEnumerator hideSmileyPanel()
        {
            yield return new WaitForSeconds(2.0f);
            this.smileyPanel.SetActive(false);
        }

        public void showMessage(string msg){
            this.messagePanel.SetActive(true);
            this.messagePanel.transform.Find("Text").GetComponent<Text>().text = msg;
            StartCoroutine(hideMessagePanel());
        }

        IEnumerator hideMessagePanel(){
            yield return new WaitForSeconds(2.0f);
            this.messagePanel.SetActive(false);
        }


        public GameObject[] getPlayeGoti()
        {
            return this.goti;
        }


        void onStartGame()
        {
            try
            {

                for (int i = 0; i < goti.Length; i++)
                {
                    goti[i].SetActive(false);

                }


            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        public void setNewTurnValue(JSONArray data)
        {
            this.TurnValue = data;

            UpdateDicePanel();
        }

        public bool removeNumber(int num)
        {
            for (int i = 0; i < this.TurnValue.Count; i++)
            {
                int num1 = int.Parse(this.TurnValue[i]);
                if (num1 == num)
                {
                    this.TurnValue.Remove(i);
                    return true;
                }
            }
            return false;
        }

        public bool openLockGoti(int num)
        {
            GotiScript obj = this.getLockGoti();
            if (obj != null)
            {
                obj.isLock = false;
                obj.moveGoti(obj.currentPosition, 0);
                return true;
            }
            return false;
        }


        public bool hasOpenValue(int num)
        {
            for (int i = 0; i < this.TurnValue.Count; i++)
            {
                int num1 = int.Parse(this.TurnValue[i]);
                if (num1 == num)
                {
                    return true;
                }
            }
            return false;
        }


        public void resetTurnNumber()
        {
            this.TurnValue = new JSONArray();
        }

        public void addNumberInTurn(int num)
        {
            // this.TurnValue.Add("" + num);
            UpdateDicePanel();
        }

        public void UpdateDicePanel()
        {

            foreach (Transform t in dicePanel.transform)
            {
                Destroy(t.gameObject);
            }
            for (int i = 0; i < this.TurnValue.Count; i++)
            {
                int num = int.Parse(this.TurnValue[i]);
                GameObject g = Instantiate(diceSample);
                g.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dices/dice_" + num);
                g.transform.SetParent(dicePanel.transform);
                g.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }


        }


        public void setPlayer(string name, string id, bool computer, string ImageName)
        {
            this.Mask.SetActive(true);
            this.Ring.SetActive(true);
            this.userPic.SetActive(true);

            this.username = name;
            this.playerID = id;
            this.isComputer = computer;
            this.playerName.text = name;
            this.NameBar.SetActive(true);
            Sprite image = null;
            if (ImageName.Contains("AVTAR"))
            {
                string[] name2 =  ImageName.Split('-');
                if (name2.Length > 1)
                {
                    Debug.Log("Image Name " + name2[1]);
                    this.userPic.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avatar/" + name2[1]);
                }
            }
            if(image == null){
                StartCoroutine(loadImage(ImageName));
            }

            isActive = true;

            for (int i = 0; i < goti.Length; i++)
            {
                goti[i].SetActive(true);
                goti[i].GetComponent<GotiScript>().setPlayer(this.playerID, transform.tag, this.currentColor);
            }

        }

        IEnumerator loadImage(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                this.userPic.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
           
        }
        public int numberOfOpenGoti()
        {
            int num = 0;
            foreach (GameObject obj in goti)
            {
                if (!(obj.GetComponent<GotiScript>().isLock || obj.GetComponent<GotiScript>().isWin))
                {
                    num++;
                }
            }
            return num;
        }


        public int totalTurnNumber()
        {
            int number = 0;
            for (int i = 0; i < this.TurnValue.Count; i++)
            {
                int num = int.Parse(this.TurnValue[i]);
                number += num;
            }
            return number;
        }



        public int numberOfLockGoti()
        {
            int num = 0;
            foreach (GameObject obj in goti)
            {
                if (obj.GetComponent<GotiScript>().isLock)
                {
                    num++;
                }
            }
            return num;
        }



        public string getName()
        {
            return this.username;
        }

        public string getUser()
        {
            return this.playerID;
        }

        public void playerTurn(int number, bool enable)
        {
            this.TurnIndicator.SetActive(true);
            diceGameObject.SetActive(true);

            if (isComputer)
            {
                dice.onShowDic(playerID, number, false); 
            }
            else
            {
                dice.onShowDic(playerID, number, enable);
            }
        }

        public void startDiceAnimation(int number)
        {
            dice.onDiceRollStart(playerID, number);
            this.TurnIndicator.SetActive(false);
        }


        public bool isGotiReadyForMove()
        {

            bool flag = false;
            foreach (GameObject obj in goti)
            {
                if (obj.GetComponent<GotiScript>().moveValidation(this.TurnValue))
                {
                    obj.GetComponent<GotiScript>().readyGoti(this.TurnValue);
                    flag = true;
                }
            }
            return flag;
        }

        public void TurnAnimation()
        {

            // transform.GetComponent<Image>().color = Color.white;

            diceGameObject.SetActive(true);
            UpdateDicePanel();
            //  GetComponent<Animation>().Play();
        }

        public void stopTurnAnimation()
        {
            diceGameObject.SetActive(false);
            this.TurnIndicator.SetActive(false);
            UpdateDicePanel();
            // transform.GetComponent<Image>().color = this.currentColor;
            //  GetComponent<Animation>().Stop();
        }


        public bool MoveGotiByAI(int num)
        {

            if (num == 6 || num == 1)
            {

                if (this.openLockGoti(num))
                {
                    Debug.Log("Open Goti " + num);
                    return true;
                }
                Debug.Log("Open not Goti " + num);
            }

            for (int j = 0; j < goti.Length; j++)
            {

                GotiScript obj = goti[j].GetComponent<GotiScript>();
                if (obj != null && (!obj.isLock) && (obj.currentPosition + num) <= GotiScript.LIMIT)
                {
                    Debug.Log("Open Other Goti " + num);
                    obj.moveGoti(obj.currentPosition, obj.currentPosition + num);
                    return true;
                }
            }


            return false;
        }




        public GotiScript getLockGoti()
        {
            for (int j = 0; j < goti.Length; j++)
            {
                GotiScript obj = goti[j].GetComponent<GotiScript>();
                if (obj.isLock)
                {
                    return obj;
                }
            }
            return null;
        }



        public int getNumberOfLockGoti()
        {
            int count = 0;
            for (int j = 0; j < goti.Length; j++)
            {
                GotiScript obj = goti[j].GetComponent<GotiScript>();
                if (obj.isLock)
                {
                    count++;
                }
            }
            return count;
        }


        public void showGoti()
        {
            for (int i = 0; i < goti.Length; i++)
            {
                goti[i].SetActive(true);
            }
        }

        public void hideGoti()
        {

            for (int i = 0; i < goti.Length; i++)
            {
                goti[i].SetActive(false);

            }

        }


        public bool checkWinner()
        {
            foreach (GameObject obj in goti)
            {
                if (!obj.GetComponent<GotiScript>().isWin)
                {
                    return false;
                }
            }
            return true;
        }



        public GotiScript getGotiByNumber(int number)
        {
            foreach (GameObject obj in goti)
            {
                if (obj.GetComponent<GotiScript>().index == number)
                {
                    return obj.GetComponent<GotiScript>();
                }
            }
            return null;
        }


        public void moveGotibySelection(int number, int gotiNumber)
        {
            for (int i = 0; i < this.TurnValue.Count; i++)
            {
                int num = int.Parse(this.TurnValue[i]);
                if (number == num)
                {
                    this.TurnValue.Remove(i);
                    GotiScript obj = this.getGotiByNumber(gotiNumber);
                    if (obj != null)
                    {
                        if (obj.isLock && (number == 6 || number == 1))
                        {
                            number = 1;
                            obj.isLock = false;
                            obj.moveGoti(obj.currentPosition, 0);
                        }
                        else
                        {

                            obj.moveGoti(obj.currentPosition, obj.currentPosition + number);
                        }
                    }
                    break;
                }
            }
        }


        public void MoveGoti(int gotiNumber, int from, int to)
        {

            GotiScript obj = this.getGotiByNumber(gotiNumber);
            if (obj != null)
            {

                obj.moveGoti(from, to);
            }
            LudoDelegate.stopAnimation();
        }


        public void WinGoti(int gotiNumber)
        {

            GotiScript obj = this.getGotiByNumber(gotiNumber);
            if (obj != null)
            {
                obj.gotiWin();
            }
        }



        public void readyGoti(int gotiNumber, JSONNode steps)
        {


            GotiScript obj = this.getGotiByNumber(gotiNumber);

            if (obj != null)
            {
                obj.readyGoti(steps);
            }
        }

        void onDiceTap(string player)
        {
            if (player.Equals(this.playerID))
            {
                this.TurnIndicator.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void onSelectColorDone(Color colo, string playerTag, int colorIndex)
        {
            if (playerTag.Equals(transform.tag))
            {
                this.color = colorIndex;
                this.currentColor = colo;
                this.diceGameObject.GetComponent<Image>().color = colo;
                foreach (GameObject obj in boxs)
                {
                    obj.GetComponent<Image>().color = colo;

                }

                for (int i = 0; i < goti.Length; i++)
                {
                    goti[i].GetComponent<GotiScript>().setPlayer(playerID, playerTag, colo);
                }

            }

            Color c = GetComponent<Image>().color;
            c.a = 0.0f;
            GetComponent<Image>().color = c;

        }

        private void OnEnable()
        {
            LudoDelegate.onSelectColorDone += onSelectColorDone;
            LudoDelegate.onStartGame += onStartGame;
            LudoDelegate.onDiceTap += onDiceTap;
        }

        private void OnDisable()
        {
            LudoDelegate.onSelectColorDone -= onSelectColorDone;
            LudoDelegate.onStartGame -= onStartGame;
            LudoDelegate.onDiceTap -= onDiceTap;
        }

    }
}

