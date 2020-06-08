using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;

namespace Ludo
{
    public class LudoManager : MonoBehaviour
    {
        public BasePlayer[] playerList;
        int currentPlayer = 0;

        bool responded = false;

        bool gotiAnimationComplete = false;
        int counter = 0;
        bool reChance = false;
        //GameObject dice;
        // Use this for initialization
        GameObject winAnimation;
        GameObject winnerPanel;
        GameObject timerPanel;
        GameObject lossPanel;
        GameObject CoinValidationPanel;
        public static int[] safePlace = new int[8] { 2, 10, 15, 49, 23, 28, 36, 41 };
        void Start()
        {
            try
            {
                winAnimation = GameObject.Find("GotiWin");
                CoinValidationPanel = transform.Find("CoinNotEnough").gameObject;
                winnerPanel = transform.Find("WinnerPanel").gameObject;
                timerPanel = transform.Find("Time").gameObject;
                lossPanel = transform.Find("LossPanel").gameObject;
                winAnimation.SetActive(false);
                winnerPanel.SetActive(false);
                timerPanel.SetActive(false);
                CoinValidationPanel.SetActive(false);
                lossPanel.SetActive(false);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }


        }

        void onStartGame()
        {
            Debug.Log("Game is start");
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
            {
                Debug.Log("Offline Game is start");

                StartCoroutine(startOffline());
            }
        }

        int getNumberofActivePlayer()
        {
            int count = 0;
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].isActive)
                {
                    count++;
                }
            }
            return count;
        }
        bool checkGameValidation()
        {
            if (getNumberofActivePlayer() > 1)
            {
                return true;
            }
            return false;
        }

        void winnerLastPlayer()
        {
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].isActive)
                {
                    this.winner(playerList[i]);
                }
            }
        }

        void winner(BasePlayer player)
        {
            winnerPanel.SetActive(true);
            winnerPanel.GetComponent<WinnerClass>().showWinner(player.getName(), true);
            winAnimation.SetActive(true);
            LudoDelegate.playClapSound();

        }

        void onLeaveGame()
        {

            for (int i = 0; i < playerList.Length; i++)
            {
                try
                {
                    if (playerList[i] != null && playerList[i].getUser().Equals("1"))
                    {
                        Debug.Log(playerList[i].getName() + " false");
                        playerList[i].isActive = false;
                    }

                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }

            if (!checkGameValidation())
            {
                winnerLastPlayer();

            }
        }

        IEnumerator startOffline()
        {
            yield return new WaitForSeconds(1);
            playerList[0].setPlayer(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME), "1", false, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE));
            playerList[1].setPlayer("Computer", "2", true, "2");

            for (; currentPlayer < playerList.Length; currentPlayer++)
            {



                if (playerList[currentPlayer].isActive)
                {
                    playerList[currentPlayer].resetTurnNumber();
                    sendTurn(playerList[currentPlayer]);
                throwDice:
                    responded = false;
                    this.reChance = false;
                    int number = Random.Range(1, 8);
                    number = number > 6 ? 6 : number;

                    if (playerList[currentPlayer].isComputer)
                    {
                        playerList[currentPlayer].playerTurn(number, false);
                        yield return new WaitForSeconds(0.2f);
                        playerList[currentPlayer].startDiceAnimation(number);
                        yield return new WaitForSeconds(1);

                        playerList[currentPlayer].TurnValue.Add("" + number);
                        playerList[currentPlayer].addNumberInTurn(number);
                        if (number == 6)
                        {
                            if (playerList[currentPlayer].totalTurnNumber() < 18)
                            {
                                goto throwDice;
                            }

                        }
                        if (playerList[currentPlayer].totalTurnNumber() < 18)
                        {
                            if (playerList[currentPlayer].isGotiReadyForMove())
                            {
                              
                                Debug.Log("Computer is thinking " + playerList[currentPlayer].TurnValue.ToString());
                                while (playerList[currentPlayer].TurnValue.Count > 0)
                                {
                                    yield return new WaitForSeconds(0.5f);
                                    int num = int.Parse(playerList[currentPlayer].TurnValue[0]);
                                    gotiAnimationComplete = false;
                                   
                                    Debug.Log(playerList[currentPlayer].getName() + " move Goti for " + num);
                                    if (playerList[currentPlayer].MoveGotiByAI(num))
                                    {
                                        yield return new WaitUntil(() => gotiAnimationComplete);
                                        if (gotiAnimationComplete)
                                        {
                                            Debug.Log("Goti Animation is done");
                                        }
                                    }
                                    if (playerList[currentPlayer].removeNumber(num))
                                    {
                                        Debug.Log("number is remove " + playerList[currentPlayer].TurnValue.ToString());
                                    }
                                    if(checkForGotiCut()){
                                        this.reChance = true;
                                    }
                                    LudoDelegate.stopAnimation();
                                }

                            }
                        }
                    }
                    else
                    {
                        playerList[currentPlayer].playerTurn(number, true);
                        yield return new WaitUntil(() => responded);
                        if (responded)
                        {

                            Debug.Log("Responded For Dice " + playerList[currentPlayer].getName());

                            if (number == 6)
                            {
                                if (playerList[currentPlayer].totalTurnNumber() < 18)
                                {
                                    goto throwDice;
                                }
                                else
                                {
                                    if (currentPlayer == playerList.Length - 1)
                                    {
                                        currentPlayer = -1;
                                    }
                                    continue;
                                }
                            }
                        askForGoti:
                            if (playerList[currentPlayer].isGotiReadyForMove())
                            {
                                responded = false;


                                yield return new WaitUntil(() => responded);
                                if (responded)
                                {
                                    responded = false;
                                    Debug.Log("Responded For Goti " + playerList[currentPlayer].getName());
                                    gotiAnimationComplete = false;
                                    yield return new WaitUntil(() => gotiAnimationComplete);
                                    if (gotiAnimationComplete)
                                    {
                                        Debug.Log("Goti Animation is done");
                                    }

                                    if (checkForGotiCut())
                                    {
                                        this.reChance = true;
                                    }
                                    if (playerList[currentPlayer].TurnValue.Count > 0)
                                    {
                                        goto askForGoti;
                                    }
                                }

                            }
                            else
                            {
                                Debug.Log("Goti is not ready");
                            }
                        }

                    }
                    if (this.reChance)
                    {
                        Debug.Log("Get Rechance " + playerList[currentPlayer].getName());
                        goto throwDice;
                    }

                }

                if (playerList[currentPlayer].checkWinner())
                {
                    this.winner(playerList[currentPlayer]);
                    break;
                }

                if (!checkGameValidation())
                {
                    winnerLastPlayer();
                    break;
                }


                if (currentPlayer == playerList.Length - 1)
                {
                    currentPlayer = -1;
                }
                yield return new WaitForSeconds(1);

            }
        }

        bool checkSafePlace(int cell)
        {

            foreach (int j in LudoManager.safePlace)
            {
                if (j == cell)
                {

                    return true;
                }
            }
            return false;
        }


        bool checkForGotiCut()
        {
            foreach (GameObject myGoti in playerList[currentPlayer].getPlayeGoti())
            {
                int myPosition = myGoti.GetComponent<GotiScript>().CellNumber;
                if (!checkSafePlace(myPosition))
                {
                    if (myPosition >= 0)
                    {
                        for (int i = 0; i < playerList.Length; i++)
                        {
                            if (i != this.currentPlayer)
                            {
                                int numberoOfGoti = 0;
                                int gotiIndex = 0;
                                foreach (GameObject opponent in playerList[i].getPlayeGoti())
                                {
                                    int opponentPosition = opponent.GetComponent<GotiScript>().CellNumber;
                                    if (myPosition == opponentPosition)
                                    {
                                        numberoOfGoti++;
                                        gotiIndex = opponent.GetComponent<GotiScript>().index;
                                    }
                                }
                                if (numberoOfGoti == 1)
                                {
                                    LudoDelegate.closeGoti();
                                    Debug.Log(playerList[currentPlayer].getName() + " find Goti of " + playerList[i].getName() + " at " + myPosition);
                                    playerList[i].MoveGoti(gotiIndex, myPosition, -1);
                                    return true;
                                }

                            }
                        }
                    }
                }
            }
            return false;
        }





        void sendTurn(BasePlayer player)
        {
            for (int i = 0; i < 4; i++)
            {
                playerList[i].stopTurnAnimation();
            }
            if (player != null)
            {
                player.TurnAnimation();
            }

        }

        /* void sendDiceNumber(BasePlayer player, int number)
         {
             LudoDelegate.showDice(player.getUser(), number);

         }
         */

        void onNumberSelection(int number, string player, int goti1)
        {
            try
            {
                if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
                {
                    if (player.Equals(playerList[currentPlayer].getUser()))
                    {
                        playerList[currentPlayer].moveGotibySelection(number, goti1);
                        responded = true;
                    }
                }
                else
                {
                    if (player.Equals(playerList[currentPlayer].getUser()))
                    {
                        appwarp.userSelectGoti(goti1, number);
                    }


                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            LudoDelegate.stopAnimation();
        }


        void onGotiWin(int index, string playerID)
        {
            BasePlayer player = this.getPlayerByID(playerID);
            if (player != null)
            {
                Debug.Log("Working");
                winAnimation.SetActive(true);
                LudoDelegate.playClapSound();
                StartCoroutine(stopAnimation());

            }
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
            {
                
                this.reChance = true;
            }
        }
        void onGotiMovementComplete()
        {
            gotiAnimationComplete = true;
        }

        void onDiceRollDone(string player, int number)
        {
            try
            {
                if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
                {

                    if (player.Equals(playerList[currentPlayer].getUser()))
                    {
                        playerList[currentPlayer].TurnValue.Add("" + number);
                        playerList[currentPlayer].addNumberInTurn(number);
                        responded = true;
                    }
                }
                else
                {
                    appwarp.sendTapDone(number);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }



        BasePlayer getPlayerByColor(int color)
        {

            try
            {
                for (int i = 0; i < 4; i++)
                {

                    if (playerList[i].color == color)
                    {
                        Debug.Log("Find Color " + color + " PLayer " + playerList[i].transform.tag);
                        return playerList[i];
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return null;
        }



        public string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
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
                            BasePlayer playerObject = this.getPlayerByColor(int.Parse(data[ServerTags.COLOR]));
                            if (playerObject != null)
                            {
                                Debug.Log(data[ServerTags.DISPLAY_NAME] + "assign " + data[ServerTags.PIC]);

                                string imageName = "1";
                                string playerName = "";
                                string color = "1";
                                try{
                                    
                                    imageName = data[ServerTags.PIC];
                                    if(imageName.Equals("")){
                                        imageName = "1";
                                    }
                                   
                                }catch(System.Exception ex){
                                    imageName = "1";
                                }
                                try
                                {
                                    color = data[ServerTags.COLOR];
                                }
                                catch (System.Exception ex)
                                {
                                    color = "1";
                                }
                                try
                                {
                                    playerName = UppercaseFirst(data[ServerTags.DISPLAY_NAME]);
                                }
                                catch (System.Exception ex)
                                {
                                    playerName = "";
                                }
                                LudoDelegate.usedColor(UppercaseFirst(playerName), int.Parse(color));
                                try
                                {
                                    
                                    playerObject.setPlayer(UppercaseFirst(playerName), data[ServerTags.PLAYER_ID], false, imageName);
                                }catch(System.Exception ex){
                                    Debug.Log(ex.Message+" "+UppercaseFirst(playerName)+"  "+data[ServerTags.PLAYER_ID]+"  "+imageName);

                                }
                               
                              
                            }
                            else
                            {
                                Debug.Log("No player Found");
                            }
                        }


                    }

                    break;

                case ServerTags.START_DEAL:
                    {

                        LudoDelegate.gamePlay();
                        LudoController.isGameStart = true;
                    }
                    break;


                case ServerTags.TURN:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            playerList[i].stopTurnAnimation();
                        }
                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            player.TurnAnimation();
                        }

                        LudoDelegate.changeTurn(s[Tags.PLAYER]);

                    }
                    break;

                case ServerTags.TIMER:
                    {

                        timerPanel.SetActive(true);
                        timerPanel.GetComponent<TimerScript>().startTimer(int.Parse(s[Tags.VALUES]));

                    }
                    break;

                case ServerTags.STOP_TIMER:
                    {

                        timerPanel.SetActive(false);

                    }
                    break;

                case ServerTags.SMILEY:
                    {
                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            player.showSmiley(s[Tags.VALUES]);
                        }


                    }

                    break;



                case ServerTags.MESSAGE:
                    {
                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            player.showMessage(s[Tags.VALUES]);
                        }


                    }

                    break;


                case ServerTags.DICE_ROLL:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            Debug.Log("DICE_ROLL " + player.getUser());
                            if (player.getUser().Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
                            {
                                player.playerTurn(int.Parse(s[Tags.VALUES]), true);
                            }
                            else
                            {
                                player.playerTurn(int.Parse(s[Tags.VALUES]), false);
                            }
                        }

                    }
                    break;


                case ServerTags.TOTAL_DICE:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            JSONArray sd = new JSONArray();
                            JSONNode pl = s[Tags.VALUES];
                            for (int i = 0; i < pl.Count; i++)
                            {
                                sd.Add("" + pl[i]);
                            }
                            player.setNewTurnValue(sd);
                        }

                    }
                    break;

                case ServerTags.MOVE_GOTI:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            player.MoveGoti(int.Parse(s[Tags.INDEX]), int.Parse(s[Tags.LAST_POSITION]), int.Parse(s[Tags.TO_POSITION]));
                        }

                    }
                    break;

                case ServerTags.GOTI_WIN:
                    {


                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {
                            Debug.Log("Working");
                            winAnimation.SetActive(true);
                            StartCoroutine(stopAnimation());
                            player.WinGoti(int.Parse(s[Tags.INDEX]));
                            LudoDelegate.playClapSound();
                        }

                    }
                    break;


                case ServerTags.READY_GOTI:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null)
                        {

                            player.readyGoti(int.Parse(s[Tags.INDEX]), s["STEPS"]);
                        }

                    }
                    break;

                case ServerTags.REMOVE_PLAYER:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null && player.getUser().Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
                        {
                            if (!LudoController.isGameStart)
                            {
                                SceneManager.LoadSceneAsync("LobiScene");
                            }
                        }

                    }
                    break;

                case ServerTags.LEAVE_TABLE:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null && player.getUser().Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
                        {

                            SceneManager.LoadSceneAsync("LobiScene");
                        }

                    }
                    break;



                case Tags.DICE_ROLL_DONE:
                    {

                        BasePlayer player = this.getPlayerByID(s[Tags.PLAYER]);
                        if (player != null && !player.getUser().Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
                        {
                            Debug.Log("startDiceAnimation " + player.getUser());
                            player.startDiceAnimation(int.Parse(s[Tags.VALUES]));
                        }
                        if (player != null)
                        {
                            player.addNumberInTurn(int.Parse(s[Tags.VALUES]));
                        }

                    }
                    break;

                case Tags.WINNER_PLAYER:
                    {

                        string player_name = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
                        string node = s["PLAYER"];
                        string player_id = s["PLAYER_ID"];
                        string Wincoin = s["VALUE"];
                        if (player_id.Equals(player_name))
                        {
                            winnerPanel.SetActive(true);
                            winnerPanel.GetComponent<WinnerClass>().showWinner(node, true);
                            winAnimation.SetActive(true);
                            LudoDelegate.playClapSound();
                        }
                        else
                        {
                            lossPanel.SetActive(true);
                        }

                    }

                    break;

                case Tags.LOSS_GAME:
                    {

                        string player_name = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
                        string displayName = s["PLAYER"];
                        string player = s["VALUE"];
                        if (player_name.Equals(player))
                        {
                            lossPanel.SetActive(true);
                        }


                    }
                    break;

                case ServerTags.ROOM_PRICE:
                    {
                        try
                        {
                            string player = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
                            if (player.Equals(s[ServerTags.PLAYER_ID]))
                            {
                                int fee = int.Parse(s[ServerTags.VALUES]);
                                if (fee > int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN)))
                                {
                                    CoinValidationPanel.SetActive(true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }


                    break;

                default:
                    break;
            }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        IEnumerator stopAnimation()
        {
            yield return new WaitForSeconds(2.0f);
            if (winAnimation.activeSelf)
                winAnimation.SetActive(false);
        }





        BasePlayer getPlayerByID(string ID)
        {
            for (int i = 0; i < 4; i++)
            {
                if (playerList[i].getUser() == ID)
                {
                    return playerList[i];
                }
            }
            return null;
        }


        private void OnEnable()
        {
            LudoDelegate.onDiceRollDone += onDiceRollDone;
            LudoDelegate.onNumberSelection += onNumberSelection;
            LudoDelegate.onReceivedServerMessage += onReceivedServerMessage;
            LudoDelegate.onRefreshCell += onRefreshCell;
            LudoDelegate.onStartGame += onStartGame;
            LudoDelegate.onGotiWin += onGotiWin;
            LudoDelegate.onGotiMovementComplete += onGotiMovementComplete;
            LudoDelegate.onLeaveGame += onLeaveGame;
        }
        private void OnDisable()
        {
            LudoDelegate.onDiceRollDone -= onDiceRollDone;
            LudoDelegate.onNumberSelection -= onNumberSelection;
            LudoDelegate.onReceivedServerMessage -= onReceivedServerMessage;
            LudoDelegate.onRefreshCell -= onRefreshCell;
            LudoDelegate.onStartGame -= onStartGame;
            LudoDelegate.onGotiWin -= onGotiWin;
            LudoDelegate.onGotiMovementComplete -= onGotiMovementComplete;
            LudoDelegate.onLeaveGame -= onLeaveGame;
        }


        void onRefreshCell()
        {
            for (int i = 1; i <= 76; i++)
            {

                int count = 0;
                GameObject[] GotiArray = GameObject.FindGameObjectsWithTag("Goti");
                foreach (GameObject btn in GotiArray)
                {
                    if (btn.GetComponent<GotiScript>().CellNumber == i)
                    {
                        count++;
                    }
                }


                if (count > 1)
                {

                    int index = 1;
                    foreach (GameObject btn in GotiArray)
                    {

                        if (btn.GetComponent<GotiScript>().CellNumber == i)
                        {
                            btn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            GameObject pos = GameObject.Find("" + i);
                            btn.transform.position = pos.transform.position;
                            switch (index)
                            {
                                case 1:
                                    {
                                        btn.transform.position = new Vector3(btn.transform.position.x - 0.1f, btn.transform.position.y - 0.1f, btn.transform.position.z);
                                    }
                                    break;
                                case 2:
                                    {
                                        btn.transform.position = new Vector3(btn.transform.position.x + 0.1f, btn.transform.position.y + 0.1f, btn.transform.position.z);
                                    }
                                    break;
                                case 3:
                                    {
                                        btn.transform.position = new Vector3(btn.transform.position.x - 0.1f, btn.transform.position.y + 0.1f, btn.transform.position.z);
                                    }
                                    break;
                                case 4:
                                    {
                                        btn.transform.position = new Vector3(btn.transform.position.x + 0.1f, btn.transform.position.y - 0.1f, btn.transform.position.z);
                                    }
                                    break;
                                case 5:
                                    {
                                        btn.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);
                                    }
                                    break;

                            }

                            index++;


                        }

                    }
                }
                else if (count == 1)
                {

                    foreach (GameObject btn in GotiArray)
                    {

                        if (btn.GetComponent<GotiScript>().CellNumber == i)
                        {
                            btn.transform.localScale = new Vector3(1f, 1f, 1f);
                            GameObject pos = GameObject.Find("" + i);
                            btn.transform.position = pos.transform.position;
                        }

                    }
                }
            }
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}