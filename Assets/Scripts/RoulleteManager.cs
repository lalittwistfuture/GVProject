using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;
using UnityEngine.SceneManagement;
namespace Roullet
{
    public class RoulleteManager : MonoBehaviour
    {

        public Text betAmount;
        public Text winAmount;
        public AudioClip ChipSound;
        public AudioClip WinSound;
        public AudioClip ClockSound;
        private List<Bet> BetList;
        private List<Bet> BetListHistory;
        private List<int> CurrentBettingNumber;
        private int winingNumber = -1;
        private int totalWin = 0;
        private int ActualWin = 0;
        private int totalLoss = 0;
        private Text Timer;
        private Text Player_coin;
        private AudioSource Player;
        private bool isTable = true;

        private GameObject[] LastCell;
        private GameObject LastNumber;
        public GameObject RouletteWheelLocation;
        public GameObject RouletteTableLocation;
        private GameObject HistoryPanel;
        private GameObject chipSample;
        private GameObject BetPanel;
        int[] blackNumberSet = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
        int[] redNumberSet = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        public JSONNode Numbers;
        void Start()
        {
            Application.targetFrameRate = 30;
            Debug.Log("Start Manager");
            chipSample = GameObject.Find("CoinSample");
            BetPanel = GameObject.Find("BetPanel");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            HistoryPanel = GameObject.Find("PreviousBetPanel");
            LastNumber = GameObject.Find("LastCell");
            LastCell = new GameObject[6];
            BetList = new List<Bet>();
            BetListHistory = new List<Bet>();
            CurrentBettingNumber = new List<int>();
            Timer = GameObject.Find("Timer").GetComponent<Text>();
            Player = transform.GetComponent<AudioSource>();
            Player_coin = GameObject.Find("Player_Coin").GetComponent<Text>();
            Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);

            for (int i = 0; i < LastCell.Length; i++)
            {
                LastCell[i] = GameObject.Find("Lastcell (" + i + ")");
            }

            ManageHistoryPanels(this.Numbers);
        }


        public void showHistoryPanel()
        {
            if (HistoryPanel.transform.GetSiblingIndex() == 0)
            {
                HistoryPanel.transform.SetSiblingIndex(HistoryPanel.transform.parent.childCount - 1);
                HistoryPanel.GetComponent<Roulette_HistoryScript>().ManageHistoryPanels(Numbers);
            }
            else
            {
                HistoryPanel.transform.SetSiblingIndex(0);
            }
        }

        void OnEnable()
        {

            RouletteDelegate.onNumberSelected += onNumberSelected;
            RouletteDelegate.onOptionSelected += onOptionSelected;
            RouletteDelegate.onClearBet += onClearBet;
            RouletteDelegate.onMoveToTable += onMoveToTable;
            RouletteDelegate.onWarpChatRecived += onWarpChatRecived;
        }

        void OnDisable()
        {
            RouletteDelegate.onNumberSelected -= onNumberSelected;
            RouletteDelegate.onOptionSelected -= onOptionSelected;
            RouletteDelegate.onClearBet -= onClearBet;
            RouletteDelegate.onMoveToTable -= onMoveToTable;
            RouletteDelegate.onWarpChatRecived -= onWarpChatRecived;
        }

        void onWarpChatRecived(string sender, string message)
        {
            JSONNode s = JSON.Parse(message);

            switch (s[RouletteTag.TAG])
            {

                case RouletteTag.MOVE_TO_TABLE:
                    {

                        onMoveToTable();
                        Numbers = JSON.Parse(s["PREVIOUS_NUMBER"]);
                        ManageHistoryPanels(Numbers);
                    }
                    break;
                case RouletteTag.START_WHEEL:
                    {
                        this.winingNumber = int.Parse(s[RouletteTag.VALUE]);
                        manageCoinOnServer();
                    }
                    break;
                case RouletteTag.START_DEAL:
                    {
                        this.winingNumber = -1;
                    }
                    break;
                case RouletteTag.MOVE_TO_WHEEL:
                    {
                        moveToWheel();
                        try
                        {
                            HistoryPanel.transform.SetSiblingIndex(0);
                        }
                        catch (System.Exception ex)
                        {

                        }
                        isTable = false;
                    }
                    break;

                case RouletteTag.BETTING_STOP:
                    {
                        sendBetttingToServer();
                    }
                    break;



                case RouletteTag.TIME:
                    {
                        try
                        {
                            Timer.text = int.Parse(s[RouletteTag.VALUE]) + ":00";
                            if (PlayerPrefs.GetInt("Sound") == 1)
                            {
                                Player.PlayOneShot(ClockSound);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }
                    break;

            }
        }

        public void ManageHistoryPanels(JSONNode Number)
        {
            setLastPanelNumber(Number);
            try
            {
                int number = int.Parse("" + Number[Number.Count - 1]);
                LastNumber.transform.GetChild(0).GetComponent<Text>().text = "" + number;
                LastNumber.GetComponent<Image>().color = getColor(number);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        void setLastPanelNumber(JSONNode Number)
        {
            for (int i = 0; i < LastCell.Length; i++)
            {
                try
                {
                    if (Number.Count - (i + 2) >= 0)
                    {
                        if (LastCell[i] != null)
                        {
                            int number = int.Parse("" + Number[Number.Count - (i + 2)]);
                            if (LastCell[i].transform.childCount > 0)
                            {
                                LastCell[i].transform.GetChild(0).GetComponent<Text>().text = "" + number;
                            }
                            LastCell[i].GetComponent<Image>().color = getColor(number);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }

        }

        Color getColor(int number)
        {

            for (int i = 0; i < blackNumberSet.Length; i++)
            {
                if (blackNumberSet[i] == number)
                {
                    return Color.black;
                }
            }
            for (int i = 0; i < redNumberSet.Length; i++)
            {
                if (redNumberSet[i] == number)
                {
                    return Color.red;
                }
            }
            return new Color(0.02f, 0.5f, 0.14f, 1.0f);
        }

        void onMoveToTable()
        {
            iTween.MoveTo (transform.gameObject, RouletteTableLocation.transform.position, 2.0f);
            if (!isTable)
            {
                try
                {
                    isTable = true;
                    Debug.Log("Move to table");
                    iTween.MoveTo(transform.gameObject, GameObject.Find("RouletteTableLocation").transform.position, 2.0f);
                    Invoke("winningAnimation", 0.5f);

                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }

        }


        void manageCoinOnServer()
        {
            bool flag = false;


            totalLoss = 0;
            ActualWin = 0;
            Debug.Log("Total Bet :" + BetList.Count);
            foreach (Bet b in BetList)
            {
                flag = false;
                JSONArray num = new JSONArray();
                foreach (int n in b.number)
                {
                    if (n == this.winingNumber)
                    {
                        flag = true;

                    }
                    num.Add("" + n);
                }
                if (flag)
                {

                    ActualWin += b.coin * ((36 / b.number.Count) - 1);
                    RoulleteHistory(b.coin, b.coin * ((36 / b.number.Count) - 1), num);

                }
                else
                {
                    totalLoss += b.coin;
                    RoulleteHistory(b.coin, b.coin * -1, num);

                }
            }

            if (ActualWin > 0)
            {
                sendCoin(ActualWin, "ROLETTE_WIN");
            }

            if (totalLoss > 0)
            {
                sendCoin(totalLoss, "ROLETTE_LOSS");
            }

        }

        void RoulleteHistory(int coin, int resultCoin, JSONArray numbers)
        {
            Debug.Log(tag + ": " + coin);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "ROLETTE_HISTORY");
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            form.AddField("coin", "" + coin);
            form.AddField("result_amount", "" + resultCoin);
            form.AddField("number", "" + numbers.ToString());
            WWW w = new WWW(Tags.URL, form);
           StartCoroutine(ServerRequesthistory(w));

        }

        private IEnumerator ServerRequesthistory(WWW www)
        {
            yield return www;

            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("History Response " + response);

            }

        }

        void winningAnimation()
        {
            bool flag = false;
            bool win = false;
            totalWin = 0;
            totalLoss = 0;
            ActualWin = 0;
            Debug.Log("Total Bet :" + BetList.Count);
            foreach (Bet b in BetList)
            {
                flag = false;
                foreach (int n in b.number)
                {
                    if (n == this.winingNumber)
                    {
                        flag = true;
                        win = true;
                    }
                }
                if (flag)
                {
                    totalWin += (b.coin + b.coin * ((36 / b.number.Count) - 1));
                    ActualWin += b.coin * ((36 / b.number.Count) - 1);
                    winAmount.text = "Win: " + totalWin;
                    iTween.MoveTo(b.coinSample, winAmount.transform.position, 2.0f);
                }
                else
                {
                    totalLoss += b.coin;
                    foreach (int n in b.number)
                    {
                        if (n > 0)
                        {
                            GameObject btn = GameObject.Find("" + n);
                            Color c = btn.GetComponent<Image>().color;
                            c.a = 0.0f;
                            btn.GetComponent<Image>().color = c;
                        }
                    }
                    iTween.MoveTo(b.coinSample, new Vector3(0.0f, -400.0f, 0.0f), 2.0f);
                }
            }
            Debug.Log("sdsds " + PlayerPrefs.GetString(PlayerDetails.Coin));
            PlayerPrefs.SetString(PlayerDetails.Coin, ""+(int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)) + totalWin));
            Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);


            if (win)
            {
                win = false;
                if (PlayerPrefs.GetInt("Sound") == 1)
                {
                    Player.PlayOneShot(WinSound);
                }
                RouletteDelegate.winnerFound();
            }
            else
            {
                winAmount.text = "Win: 0";
            }
            Invoke("clearTable", 2.0f);

            this.winingNumber = -1;
        }

        void sendCoin(int coin, string tag)
        {
            Debug.Log(tag + ": " + coin);
            WWWForm form = new WWWForm();
            form.AddField("TAG", tag);
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            form.AddField("coin", "" + coin);
            Debug.Log("Url dddd " + Tags.URL);
            WWW w = new WWW(Tags.URL, form);
            StartCoroutine(ServerRequest(w));

        }



        private IEnumerator ServerRequest(WWW www)
        {
            yield return www;

            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("result is " + response);
                JSONNode node = JSON.Parse(response);
                if (node != null)
                {
                    string result = node["status"];
                    string msg = node["message"];

                    if (result.Equals("OK"))
                    {
                        try{
                            JSONNode data1 = node["data"];
                           
                            string coin = data1["balance"];
                            PlayerPrefs.SetString(PlayerDetails.Coin, "" + coin);
                            GameController.coin = int.Parse(coin);
                            GameController.practic_coin = int.Parse(coin);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }

                }

            }

        }


       public void clearTable()
        {
            BetListHistory.Clear();
            foreach (Bet b in BetList)
            {
                BetListHistory.Add(b);
            }
            this.BetList.Clear();
            RouletteDelegate.removeAllBet();

        }

        public void reBetonTable()
        {
            if (PlayerPrefs.GetInt(RouletteTag.BET_ENABLE) == RouletteTag.ENABLE)
            {
                int bet = 0;
                foreach (Bet b in this.BetListHistory)
                {
                    bet += b.coin;
                }
                if (bet <= int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)))
                {
                    //if (PlayerPrefs.GetInt (RouletteTag.TOTAL_BET) + bet <= 500) {
                    foreach (Bet b1 in BetListHistory)
                    {
                        Bet b = new Bet();
                        b.number = new List<int>(b1.number);
                        b.coinSample = Instantiate(chipSample);
                        b.coinSample.transform.position = b1.coinPosition;
                        b.coinPosition = b1.coinPosition;
                        b.coin = b1.coin;
                        b.coinSample.transform.SetParent(BetPanel.transform);
                        b.coinSample.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        setCoinImage(b.coinSample, b.coin);
                        BetPanel.GetComponent<BetPanelScript>().chipSmaples.Add(b.coinSample);
                        PlayerPrefs.SetString(PlayerDetails.Coin,""+(int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)) - b.coin));
                        Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);
                        foreach (int n in b.number)
                        {
                            if (n > 0)
                            {
                                GameObject btn = GameObject.Find("" + n);
                                Color c = btn.GetComponent<Image>().color;
                                c.a = 1.0f;
                                btn.GetComponent<Image>().color = c;
                            }
                        }

                        this.BetList.Add(b);
                        getBetAmount();
                        //}
                    }
                }
                else
                {
                    GameController.showToast("You don't have suffiient balance.");
                }
            }
        }



        void setCoinImage(GameObject btn, int number)
        {
            switch (number)
            {

                case 500:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettec5");
                    }
                    break;
                case 100:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettec4");
                        PlayerPrefs.SetString (RouletteTag.RouletteCoinImage, "Roulettec4");
                    }
                    break;
                case 50:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettec3");
                        PlayerPrefs.SetString (RouletteTag.RouletteCoinImage, "Roulettec3");
                    }
                    break;
                case 10:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettec2");
                        PlayerPrefs.SetString (RouletteTag.RouletteCoinImage, "Roulettec2");
                    }
                    break;
                case 5:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettec1");
                        PlayerPrefs.SetString (RouletteTag.RouletteCoinImage, "Roulettec1");
                    }
                    break;
                case 1:
                    {
                        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Roulettechip");
                        PlayerPrefs.SetString (RouletteTag.RouletteCoinImage, "Roulettechip");
                    }
                    break;
            }

        }
        void checkNumberSelection()
        {

        }

        public void unDoBet()
        {
            if (PlayerPrefs.GetInt(RouletteTag.BET_ENABLE) == RouletteTag.ENABLE)
            {
                if (BetList.Count > 0)
                {
                    Bet b = BetList[BetList.Count - 1];
                    Destroy(b.coinSample);
                    PlayerPrefs.SetString(PlayerDetails.Coin, ""+ (int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)) + b.coin));
                    PlayerPrefs.SetInt(RouletteTag.TOTAL_BET, PlayerPrefs.GetInt(RouletteTag.TOTAL_BET) - b.coin);
                    Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);
                    foreach (int n in b.number)
                    {
                        if (n > 0)
                        {
                            GameObject btn = GameObject.Find("" + n);
                            Color c = btn.GetComponent<Image>().color;
                            c.a = 0.0f;
                            btn.GetComponent<Image>().color = c;
                        }
                    }
                    BetList.RemoveAt(BetList.Count - 1);
                }
                getBetAmount();
            }
        }


       void onClearBet()
        {

            betAmount.text = "Bet:0";

            foreach (Bet b in BetList)
            {
                PlayerPrefs.SetString(PlayerDetails.Coin, ""+(int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)) + b.coin));
                Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);
            }
            PlayerPrefs.SetInt(RouletteTag.TOTAL_BET, 0);
            this.BetList.Clear();

        }



        void onNumberSelected(List<int> number, GameObject obj)
        {
            winAmount.text = "Win: 0";
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                Player.PlayOneShot(ChipSound);
                Player.Play();
            }

            Bet b = new Bet();
            b.number = new List<int>(number);
            b.coinSample = obj;
            b.coinPosition = obj.transform.position;
            b.coin = PlayerPrefs.GetInt(RouletteTag.RouletteSelectedCoin);
            PlayerPrefs.SetString(PlayerDetails.Coin,""+(int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)) - b.coin));
            Player_coin.text = "Coin:" + PlayerPrefs.GetString(PlayerDetails.Coin);
            foreach (int n in b.number)
            {
                if (n > 0)
                {
                    GameObject btn = GameObject.Find("" + n);
                    Color c = btn.GetComponent<Image>().color;
                    c.a = 1.0f;
                    btn.GetComponent<Image>().color = c;
                }
            }

            this.BetList.Add(b);
            getBetAmount();
        }

        void getBetAmount()
        {
            int bet = 0;
            foreach (Bet b in this.BetList)
            {
                bet += b.coin;
            }
            PlayerPrefs.SetInt(RouletteTag.TOTAL_BET, bet);
            betAmount.text = "Bet: " + bet;

        }


        void sendBetttingToServer()
        {
            try
            {
                JSONClass data = new JSONClass();
                JSONArray array = new JSONArray();
                foreach (Bet b in BetList)
                {
                    foreach (int n in b.number)
                    {
                        JSONClass number = new JSONClass();
                        number.Add("NUMBER", "" + n);
                        int time = (36 / b.number.Count) - 1;
                        number.Add("TIMES", "" + time);
                        number.Add("TAG", "ADD_COIN");
                        number.Add("COIN", "" + b.coin);
                        //data.Add ("NUMBER", number);
                        Roulette_AppWarpClass.add_coin(number);
                        //array.Add (number);
                    }
                }
                data.Add("TAG", "ADD_COIN");
                data.Add("NUMBER", array);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Ex Handle " + ex);
            }
            //Roulette_AppWarpClass.add_coin (data);
        }




        public void moveToWheel()
        {

            //iTween.MoveTo (transform.gameObject, RouletteWheelLocation.transform.position, 2.0f);
            try
            {
                iTween.MoveTo(transform.gameObject, GameObject.Find("RouletteWheelLocation").transform.position, 2.0f);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        void onOptionSelected(string tag, GameObject obj)
        {
            CurrentBettingNumber.Clear();
            switch (tag)
            {
                case "19-36":
                    {

                        for (int i = 19; i <= 36; i++)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "1-18":
                    {
                        for (int i = 1; i <= 18; i++)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "0":
                    {
                        CurrentBettingNumber.Add(0);
                    }
                    break;
                case "1-12":
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "13-24":
                    {
                        for (int i = 13; i <= 24; i++)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "25-36":
                    {
                        for (int i = 25; i <= 36; i++)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "Even":
                    {
                        for (int i = 2; i <= 36; i = i + 2)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "Odd":
                    {
                        for (int i = 1; i <= 36; i = i + 2)
                        {
                            CurrentBettingNumber.Add(i);
                        }
                    }
                    break;
                case "Black":
                    {
                        int[] number = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
                        for (int i = 0; i < number.Length; i++)
                        {
                            CurrentBettingNumber.Add(number[i]);
                        }
                    }
                    break;
                case "Red":
                    {
                        int[] number = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
                        for (int i = 0; i < number.Length; i++)
                        {
                            CurrentBettingNumber.Add(number[i]);
                        }
                    }
                    break;
                case "3rd":
                    {
                        int[] number = { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
                        for (int i = 0; i < number.Length; i++)
                        {
                            CurrentBettingNumber.Add(number[i]);
                        }
                    }
                    break;
                case "2nd":
                    {
                        int[] number = { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35 };
                        for (int i = 0; i < number.Length; i++)
                        {
                            CurrentBettingNumber.Add(number[i]);
                        }
                    }
                    break;
                case "1st":
                    {
                        int[] number = { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34 };
                        for (int i = 0; i < number.Length; i++)
                        {
                            CurrentBettingNumber.Add(number[i]);
                        }
                    }
                    break;
            }
            onNumberSelected(CurrentBettingNumber, obj);
        }

    }

    public class Bet
    {
        public List<int> number;
        public int coin;
        public GameObject coinSample;
        public Vector3 coinPosition;
    }
}