using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.SimpleJSON;
namespace Ludo { 

    public class appwarp : MonoBehaviour, ConnectionRequestListener, RoomRequestListener, ChatRequestListener, UpdateRequestListener, NotifyListener, ZoneRequestListener, LobbyRequestListener
    {
        //private string appKey = "e2383b63-fdc1-4ec5-a";
        //private string ipAddress = "192.168.1.4";
        //private int port = 12346;

        private string appKey = "b476cbe1-d633-4be1-a";
        private string ipAddress = "132.148.25.71";
        private int port = 12349;

        public static bool socketConnected = false;

        int roomID = 0;
        bool roomCreation = false;
        bool roomSub = false;
        GameObject privateGame;
        GameObject roomInfoPanel;
        InputField newRoomID;
        GameObject loadingGameScreen;
        public static string ServerMessage;

        private Image ConnectionImage;
        void Start()
        {
            LudoController.isGameStart = false;
            try
            {
                privateGame = transform.Find("PrivateTablePage").gameObject;
                newRoomID = privateGame.transform.Find("InputField").GetComponent<InputField>();
                roomInfoPanel = transform.Find("PrivateRoomInfo").gameObject;
                loadingGameScreen = transform.Find("LoadingGamePanel").gameObject;
                ConnectionImage = transform.Find("ConnectionImage").GetComponent<Image>();
                ConnectionImage.color = Color.yellow;
                loadingGameScreen.SetActive(true);
                appwarp.ServerMessage = "Connecting with server";
                if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PRIVATE)
                {
                    privateGame.SetActive(true);
                }
                else
                {
                    privateGame.SetActive(false);

                }
                roomInfoPanel.SetActive(false);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            Debug.Log("Game type " + PlayerPrefs.GetInt(LudoTags.GAME_TYPE));

            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
            {
                roomInfoPanel.SetActive(false);
                privateGame.SetActive(false);
                Color c = ConnectionImage.color;
                c.a = 0.0f;
                ConnectionImage.color = c;
                loadingGameScreen.SetActive(false);

            }
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                Debug.Log("Connect with server ");
                Connection();
            }
        }


        public void JoinGameAction()
        {
            if (newRoomID.text.Length != 0)
            {
                if (WarpClient.GetInstance() != null)
                {
                    WarpClient.GetInstance().JoinRoom(newRoomID.text);

                }
            }
            else
            {
                Debug.Log("Please enter a room ID");
            }
        }


        void searchTable()
        {
            if (PlayerPrefs.GetInt(LudoTags.ENTRY_FEE) <= int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN)))
            {
                Debug.Log("Search Game Room");
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add(LudoTags.GAME_TYPE, PlayerPrefs.GetInt(LudoTags.GAME_TYPE));
                dic.Add(LudoTags.ENTRY_FEE, PlayerPrefs.GetInt(LudoTags.ENTRY_FEE));
                dic.Add(LudoTags.ROOM_TYPE, PlayerPrefs.GetInt(LudoTags.ROOM_TYPE));
                dic.Add(LudoTags.USER_LIMIT, PlayerPrefs.GetInt(LudoTags.USER_LIMIT));
                dic.Add(LudoTags.GOTI_LIMIT, PlayerPrefs.GetInt(LudoTags.GOTI_LIMIT));
                dic.Add(Tags.DOMAIN, Tags.URL);
                WarpClient.GetInstance().JoinRoomWithProperties(dic);
            }
            else
            {
                GameConstantData.showToast("You do not have sufficient coin.");
            }
        }
        public void createTable()
        {
            if (PlayerPrefs.GetInt(LudoTags.ENTRY_FEE) <= int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN)))
            {
                Debug.Log("Create Game Room");
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add(LudoTags.GAME_TYPE, PlayerPrefs.GetInt(LudoTags.GAME_TYPE));
                dic.Add(LudoTags.ENTRY_FEE, PlayerPrefs.GetInt(LudoTags.ENTRY_FEE));
                dic.Add(LudoTags.ROOM_TYPE, PlayerPrefs.GetInt(LudoTags.ROOM_TYPE));
                dic.Add(LudoTags.USER_LIMIT, PlayerPrefs.GetInt(LudoTags.USER_LIMIT));
                dic.Add(LudoTags.GOTI_LIMIT, PlayerPrefs.GetInt(LudoTags.GOTI_LIMIT));
                dic.Add(Tags.DOMAIN, Tags.URL);
                WarpClient.GetInstance().CreateRoom("New Room", "ludo", PlayerPrefs.GetInt(LudoTags.USER_LIMIT), dic);
            }
            else
            {
                GameConstantData.showToast("You do not have sufficient coin.");
            }

        }

        void HideLoading()
        {
            loadingGameScreen.SetActive(false);
        }



        public static void sendAnimationDone()
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, "DONE");
            sendChat(data.ToString());

        }

        public static void joinGameHello()
        {

            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, ServerTags.GAME_REQUEST);
            data.Add(Tags.COIN, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
            data.Add(Tags.DISPLAY_NAME, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME));
            data.Add(Tags.PIC, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE));
            data.Add(ServerTags.PLAYER_ID, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            data.Add(Tags.TOTAL_MATCH, "12");
            data.Add(Tags.WON_MATCH, "3");
            sendChat(data.ToString());

        }

        public static void leaveGame()
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, "LEAVE_GAME");
            sendChat(data.ToString());
        }

        public static void sendSmiley(string message)
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, ServerTags.SMILEY);
            data.Add(ServerTags.PLAYER, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            data.Add(ServerTags.VALUES, message);
            sendChat(data.ToString());
        }


        public static void sendMessageChat(string message)
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, ServerTags.MESSAGE);
            data.Add(ServerTags.PLAYER, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            data.Add(ServerTags.VALUES, message);
            sendChat(data.ToString());
        }

        public static void joinGame(int color)
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, "COLOR_SELECTION");
            data.Add(Tags.COLOR, "" + color);
            sendChat(data.ToString());
        }

        public static void sendTapDone(int number)
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, Tags.DICE_ROLL_DONE);
            data.Add(ServerTags.PLAYER, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            data.Add("VALUE", "" + number);
            sendChat(data.ToString());
        }

        public static void userSelectGoti(int pos, int number)
        {
            JSONClass data = new JSONClass();
            data.Add(ServerTags.TAG, Tags.SELECT_GOTI);
            data.Add(ServerTags.POSITION, "" + pos);
            data.Add("STEPS", "" + number);
            data.Add(ServerTags.PLAYER, PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            sendChat(data.ToString());
        }

        static void sendChat(string msg)
        {
            try
            {
                if (WarpClient.GetInstance() != null)
                {
                    if (!msg.Equals(""))
                    {

                        Debug.Log(msg);
                        WarpClient.GetInstance().SendChat(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }



        public void Connection()
        {

            loadingGameScreen.SetActive(true);
            appwarp.ServerMessage = "Connecting with server";
            Debug.Log("Player Name " + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            string player_name = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
            WarpClient.initialize(appKey, ipAddress, port);
            WarpClient.setRecoveryAllowance(120);
            //listen = new Listener();
            WarpClient.GetInstance().AddConnectionRequestListener(this);
            WarpClient.GetInstance().AddChatRequestListener(this);
            WarpClient.GetInstance().AddLobbyRequestListener(this);
            WarpClient.GetInstance().AddNotificationListener(this);
            WarpClient.GetInstance().AddRoomRequestListener(this);
            WarpClient.GetInstance().AddUpdateRequestListener(this);
            WarpClient.GetInstance().AddZoneRequestListener(this);
            WarpClient.GetInstance().Connect(player_name, "");

        }

        public static void Disconnect()
        {
            if (WarpClient.GetInstance() != null)
            {
                WarpClient.GetInstance().Disconnect();

            }
        }
        void onStartGame()
        {

        }




        IEnumerator hideLoadingMessage()
        {
            yield return new WaitForSeconds(6.0f);
            loadingGameScreen.SetActive(false);
        }

        private void onUserJoinBoard(string playerID)
        {
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                //loadingGameScreen.SetActive(true);
                //appwarp.ServerMessage = "Waiting for Opponent";
            }
        }
        void onGamePlayStart()
        {
            loadingGameScreen.SetActive(false);

        }

        private void OnEnable()
        {

            LudoDelegate.onStartGame += onStartGame;
            LudoDelegate.onUserJoinBoard += onUserJoinBoard;
            LudoDelegate.onGamePlayStart += onGamePlayStart;
        }

        private void OnDisable()
        {

            LudoDelegate.onStartGame -= onStartGame;
            LudoDelegate.onUserJoinBoard -= onUserJoinBoard;
            LudoDelegate.onGamePlayStart -= onGamePlayStart;
        }


        public void onConnectDone(ConnectEvent eventObj)
        {
            try
            {
                switch (eventObj.getResult())
                {
                    case WarpResponseResultCode.SUCCESS:
                        {

                            ConnectionImage.color = Color.green;
                            appwarp.socketConnected = true;
                            if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PUBLIC)
                            {
                                searchTable();
                            }
                            else
                            {
                                HideLoading();
                            }

                        }
                        break;
                    case WarpResponseResultCode.CONNECTION_ERROR_RECOVERABLE:
                        appwarp.socketConnected = false;
                        ConnectionImage.color = Color.yellow;
                        WarpClient.GetInstance().RecoverConnection();
                        break;
                    case WarpResponseResultCode.SUCCESS_RECOVERED:
                        appwarp.socketConnected = true;
                        ConnectionImage.color = Color.green;
                        break;
                    case WarpResponseResultCode.AUTH_ERROR:
                        ConnectionImage.color = Color.red;
                        appwarp.ServerMessage = "Please try to connect after some time.";
                        appwarp.socketConnected = false;
                        break;
                    case WarpResponseResultCode.CONNECTION_ERR:
                        ConnectionImage.color = Color.red;
                        appwarp.socketConnected = false;
                        appwarp.ServerMessage = "Unable to conncet with server.";
                        break;
                    case WarpResponseResultCode.BAD_REQUEST:
                        ConnectionImage.color = Color.red;
                        appwarp.socketConnected = false;
                        appwarp.ServerMessage = "Please check your internet connection";
                        break;
                    default:
                        ConnectionImage.color = Color.red;
                        appwarp.ServerMessage = "Unable to conncet with server.";
                        appwarp.socketConnected = false;
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public void onInitUDPDone(byte res)
        {

        }



        public void onDisconnectDone(ConnectEvent eventObj)
        {
            appwarp.socketConnected = false;
            Debug.Log("disconnect " + eventObj.getResult());
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
               

            }
        }


        public void onCreateRoomDone(RoomEvent eventObj)
        {
            Debug.Log("Room Creation " + eventObj.getResult());
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                this.roomCreation = true;
                this.roomID = int.Parse(eventObj.getData().getId());
                WarpClient.GetInstance().JoinRoom(eventObj.getData().getId());
            }
            else
            {
                createTable();
            }

        }

        public void onJoinRoomDone(RoomEvent eventObj)
        {
            Debug.Log("Join Room " + eventObj.getResult());
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {

                WarpClient.GetInstance().SubscribeRoom(eventObj.getData().getId());
            }
            else
            {
                createTable();
            }
        }

        public void onSubscribeRoomDone(RoomEvent eventObj)
        {
            if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
            {
                Debug.Log("Subscribe " + PlayerPrefs.GetInt(LudoTags.ROOM_TYPE));
                this.roomSub = true;
                this.roomID = int.Parse(eventObj.getData().getId());
                appwarp.joinGameHello();
                if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PRIVATE)
                {
                    loadingGameScreen.SetActive(false);
                    privateGame.SetActive(false);
                    if (PlayerPrefs.GetString(GameTags.PRIVATE_TABLE_TYPE).Equals(GameTags.CREATE_TABLE))
                    {
                        roomInfoPanel.SetActive(true);
                        roomInfoPanel.GetComponent<PrivateTable>().updateRoomID("" + this.roomID);
                    }
                    else
                    {
                        roomInfoPanel.SetActive(false);
                    }
                }
                else
                {
                    loadingGameScreen.SetActive(false);

                }
            }
            else
            {
                createTable();
            }
        }
        public void onChatReceived(ChatEvent eventObj)
        {

            Debug.Log(eventObj.getSender() + "  :  " + eventObj.getMessage());
            if (WarpClient.GetInstance() != null)
            {
                LudoDelegate.sendMessage(eventObj.getSender(), eventObj.getMessage());
            }

        }


        /******************   Delegate Functions Start **********************/

        public void onJoinLobbyDone(LobbyEvent eventObj)
        {

        }

        public void onLeaveLobbyDone(LobbyEvent eventObj)
        {

        }

        public void onSubscribeLobbyDone(LobbyEvent eventObj)
        {
        }

        public void onUnSubscribeLobbyDone(LobbyEvent eventObj)
        {

        }

        public void onGetLiveLobbyInfoDone(LiveRoomInfoEvent eventObj)
        {

        }


        public void onDeleteRoomDone(RoomEvent eventObj)
        {

        }

        public void onGetAllRoomsDone(AllRoomsEvent eventObj)
        {
        }

        public void onGetOnlineUsersDone(AllUsersEvent eventObj)
        {

        }

        public void onGetLiveUserInfoDone(LiveUserInfoEvent eventObj)
        {

        }

        public void onSetCustomUserDataDone(LiveUserInfoEvent eventObj)
        {
        }

        public void onGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
        {
        }


        public void onUnSubscribeRoomDone(RoomEvent eventObj)
        {

        }



        public void onLockPropertiesDone(byte result)
        {

        }

        public void onUnlockPropertiesDone(byte result)
        {

        }

        public void onLeaveRoomDone(RoomEvent eventObj)
        {

        }

        public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
        {

        }

        public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
        {

        }

        public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
        {


        }




        public void onSendChatDone(byte result)
        {


        }

        public void onSendPrivateChatDone(byte result)
        {

        }


        public void onSendUpdateDone(byte result)
        {

        }


        public void onRoomCreated(RoomData eventObj)
        {

        }

        public void onRoomDestroyed(RoomData eventObj)
        {

        }

        public void onUserLeftRoom(RoomData eventObj, string username)
        {
        }

        public void onUserJoinedRoom(RoomData eventObj, string username)
        {

        }

        public void onUserLeftLobby(LobbyData eventObj, string username)
        {

        }

        public void onUserJoinedLobby(LobbyData eventObj, string username)
        {

        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
        {

        }

        public void onPrivateChatReceived(string sender, string message)
        {


        }

        public void onMoveCompleted(MoveEvent move)
        {

        }



        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {

        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<String, System.Object> properties)
        {


        }

        public void onUserPaused(string a, bool b, string c)
        {

        }

        public void onUserResumed(string a, bool b, string c)
        {



        }

        public void onGameStarted(string a, string b, string c)
        {

        }

        public void onGameStopped(string a, string b)
        {

        }



        public void onInvokeZoneRPCDone(RPCEvent evnt)
        {

        }

        public void onInvokeRoomRPCDone(RPCEvent evnt)
        {

        }

        public void sendMsg(string msg)
        {

            WarpClient.GetInstance().SendChat(msg);

        }

        public void sendBytes(byte[] msg, bool useUDP)
        {

            if (useUDP == true)
                if (WarpClient.GetInstance() != null)
                {
                    WarpClient.GetInstance().SendUDPUpdatePeers(msg);
                }
                else if (WarpClient.GetInstance() != null)
                {
                    WarpClient.GetInstance().SendUpdatePeers(msg);
                }

        }

        /******************   Delegate Functions End   **********************/

    }
}
