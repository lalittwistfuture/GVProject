using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ludo
{
    public class LudoDelegate : MonoBehaviour
    {

        public delegate void SelectColorDone(Color color, string playerTag, int colorIndex);

        public static event SelectColorDone onSelectColorDone;

        public static void selectColor(Color color, string playerTag, int colorIndex)
        {
            if (onSelectColorDone != null)
            {
                onSelectColorDone(color, playerTag,colorIndex);
            }
        }


        public delegate void GotiMovementComplete();

        public static event GotiMovementComplete onGotiMovementComplete;

        public static void movementComplete()
        {
            if (onGotiMovementComplete != null)
            {
                onGotiMovementComplete();
            }
        }

        public delegate void DiceTap(string playerID);

        public static event DiceTap onDiceTap;

        public static void diceClick(string playerID)
        {
            if (onDiceTap != null)
            {
                onDiceTap(playerID);
            }
        }

        public delegate void UserJoinBoard(string playerID);

        public static event UserJoinBoard onUserJoinBoard;

        public static void joinUser(string playerID)
        {
            if (onUserJoinBoard != null)
            {
                onUserJoinBoard(playerID);
            }
        }

        public delegate void ChangeSoundSetting(bool value);

        public static event ChangeSoundSetting onChangeSoundSetting;

        public static void changeSoundSetting(bool value)
        {
            if (onChangeSoundSetting != null)
            {
                onChangeSoundSetting(value);
            }
        }


        public delegate void ColorUsed(string playerName, int color);

        public static event ColorUsed onColorUsed;

        public static void usedColor(string playerName, int color)
        {
            if (onColorUsed != null)
            {
                onColorUsed( playerName,  color);
            }
        }


        public delegate void ReceivedServerMessage(string sender, string message);

        public static event ReceivedServerMessage onReceivedServerMessage;

        public static void sendMessage(string sender, string message)
        {
            if (onReceivedServerMessage != null)
            {
                onReceivedServerMessage(sender, message);
            }
        }

        public delegate void StopSelection();

        public static event StopSelection onStopSelection;

        public static void stopAnimation()
        {
            if (onStopSelection != null)
            {
                onStopSelection();
            }
        }

        public delegate void DiceSound();

        public static event DiceSound onDiceSound;

        public static void playDiceSound()
        {
            if (onDiceSound != null)
            {
                onDiceSound();
            }
        }

        public delegate void LeaveGame();

        public static event LeaveGame onLeaveGame;

        public static void leaveGame()
        {
            if (onLeaveGame != null)
            {
                onLeaveGame();
            }
        }


        public delegate void ClapSound();

        public static event ClapSound onClapSound;

        public static void playClapSound()
        {
            if (onClapSound != null)
            {
                onClapSound();
            }
        }


        public delegate void CutGoti();

        public static event CutGoti onCutGoti;

        public static void closeGoti()
        {
            if (onCutGoti != null)
            {
                onCutGoti();
            }
        }


        public delegate void SafePlaceSound();

        public static event SafePlaceSound onSafePlaceSound;

        public static void safePlace()
        {
            if (onSafePlaceSound != null)
            {
                onSafePlaceSound();
            }
        }

        public delegate void GotiSound();

        public static event GotiSound onGotiSound;

        public static void playGotiSound()
        {
            if (onGotiSound != null)
            {
                onGotiSound();
            }
        }

        public delegate void GamePlayStart();

        public static event GamePlayStart onGamePlayStart;

        public static void gamePlay()
        {
            if (onGamePlayStart != null)
            {
                onGamePlayStart();
            }
        }

        public delegate void ShowSelection(string PlayerID, SimpleJSON.JSONNode TurnValue, Color color, Vector3 pos,int goti);

        public static event ShowSelection onShowSelection;

        public static void showPanel(string PlayerID, SimpleJSON.JSONNode TurnValue, Color color, Vector3 pos, int goti)
        {
            if (onShowSelection != null)
            {
                onShowSelection(PlayerID,TurnValue,color,pos,goti);
            }
        }


        public delegate void NumberSelection(int number,string player,int goti);

        public static event NumberSelection onNumberSelection;

        public static void selectNumber(int number, string player, int goti)
        {
            if (onNumberSelection != null)
            {
                onNumberSelection(number, player,goti);
            }
        }


        public delegate void RefreshCell();

        public static event RefreshCell onRefreshCell;

        public static void refreshCell()
        {
            if (onRefreshCell != null)
            {
                onRefreshCell();
            }
        }

        public delegate void TurnChange(string playerID);

        public static event TurnChange onTurnChange;

        public static void changeTurn(string playerID)
        {
            if (onTurnChange != null)
            {
                onTurnChange(playerID);
            }
        }


        public delegate void UserGotiSelection(int index, int number);

        public static event UserGotiSelection onUserGotiSelection;

        public static void selectGoti(int index, int number)
        {
            if (onUserGotiSelection != null)
            {
                onUserGotiSelection(index, number);
            }
        }

        public delegate void GotiWin(int index, string player);

        public static event GotiWin onGotiWin;

        public static void gotiWin(int index, string player)
        {
            if (onGotiWin != null)
            {
                onGotiWin(index,player);
            }
        }

        public delegate void ShowDic(string player,int number,bool enable);

        public static event ShowDic onShowDic;

        public static void showDice(string player,int number, bool enable)
        {
            if (onShowDic != null)
            {
                onShowDic(player,number,enable);
            }
        }

        public delegate void HideDic(string player);

        public static event HideDic onHideDic;

        public static void hideDice(string player)
        {
            if (onHideDic != null)
            {
                onHideDic(player);
            }
        }

        public delegate void DiceRollDone(string player, int number);

        public static event DiceRollDone onDiceRollDone;

        public static void diceRollDone(string player, int number)
        {
            if (onDiceRollDone != null)
            {
                onDiceRollDone(player,number);
            }
        }



        public delegate void DiceRollStart(string player, int number);

        public static event DiceRollStart onDiceRollStart;

        public static void startDice(string player, int number)
        {
            if (onDiceRollStart != null)
            {
                onDiceRollStart(player, number);
            }
        }

       

        public delegate void SocketConnectionRecover(bool connection);

        public static event SocketConnectionRecover onSocketConnectionRecover;

        public static void connectionRecover(bool connection)
        {
            if (onSocketConnectionRecover != null)
            {
                onSocketConnectionRecover(connection);
            }
        }

        public delegate void StartGame();

        public static event StartGame onStartGame;

        public static void startGame()
        {
            if (onStartGame != null)
            {
                onStartGame();
            }
        }

    }
}