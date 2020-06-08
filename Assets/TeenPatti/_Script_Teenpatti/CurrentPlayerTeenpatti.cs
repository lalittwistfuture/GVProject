
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;
using SimpleJSON;
using UnityEngine.UI;


public class CurrentPlayerTeenpatti : BasePlayerTeenpatti
{
	private int CurrentNetwork = -1;
	public GameObject statusMsg;
	//public GameObject PlayerTotalCoin;
	public GameObject SeeBtn;
   // public GameObject TipBtn;
    public GameObject JoinBtn;
    public GameObject connectingScreen;
	void Start ()
	{
		base.Start ();
        if (connectingScreen.activeSelf)
            connectingScreen.SetActive(false); 
		try {
			base.player_Image.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Avtar/" + PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_IMAGE));
		} catch (System.Exception ex) {
			Debug.Log ("Exception " + ex.Message);
			base.player_Image.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("user");
		}

		try {

			//JoinBtn = transform.Find ("joinBtn").gameObject;
			JoinBtn.SetActive(false);
		} catch (System.Exception ex) {
			Debug.Log ("Exception " + ex.Message);
			base.player_Image.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("user");
		}

		try {
			//base.coin = int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin));
			//PlayerTotalCoin = GameObject.Find ("CoinText");
            base.totalCoin.GetComponent<Text> ().text = "" + base.coin;
		} catch (System.Exception ex) {
			Debug.Log (ex.Message);
		}

		try {
			//statusMsg = transform.Find ("statusMsg").gameObject;
			//SeeBtn = transform.Find ("SeeBtn").gameObject;
			if (SeeBtn) {
				SeeBtn.SetActive (false);
			}
			//TipBtn = GameObject.Find ("Tipbutton");

			base.player_id = PlayerPrefs.GetString (PrefebTagsTeenpatti.PLAYER_ID);
			//base.name = GameControllerTeenPatti.UppercaseFirst (PlayerPrefs.GetString (PlayerDetails.RealName));
			base.player_name.GetComponent<Text> ().text = GameControllerTeenPatti.UppercaseFirst (PlayerPrefs.GetString (PlayerDetails.RealName));
			player_coin.GetComponent<Text> ().text = "0";
           // StartCoroutine(syncronized());
		} catch (System.Exception ex) {
			Debug.Log ("Exception " + ex.Message);
		}
		this.SeeBtn.SetActive (false);
	}

	//private void OnEnable()
	//{
 //       GameDelegateTeenPatti.onPlayerConnection += onPlayerConnection1;

	//}
	//private void OnDisable()
	//{
 //       GameDelegateTeenPatti.onPlayerConnection -= onPlayerConnection1;
	//}

    //public void onPlayerConnection1(string playerID, bool connection)
    //{
    //    if (player_id.Equals(PlayerPrefs.GetString(PrefebTagsTeenpatti.PLAYER_ID)))
    //    {
    //        if (connection)
    //        {
    //            if (connectingScreen.activeSelf)
    //        connectingScreen.SetActive(false); 
    //        }
    //        else
    //        {
    //            if (!connectingScreen.activeSelf)
    //        connectingScreen.SetActive(true); 
    //        }
    //    }
    //}

	public IEnumerator syncronized()
    {
       
        while (true)
        {
            try
            {
                int[] data_f = new int[2];
                data_f[0] = int.Parse(base.player_id);
                data_f[1] = 0;
                int data_len = sizeof(int) * 2;
                byte[] data1 = new byte[data_len];
                System.Buffer.BlockCopy(data_f, 0, data1, 0, sizeof(int) * 2);
                listner.sendBytes(data1, true);
              
            }
            catch (System.Exception ex)
            {
              
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
	// Update is called once per frame
	void Update ()
	{

	}

	void FixedUpdate ()
	{
		base.FixedUpdate ();
        //if(base.avaivableUser<0){
        //    if (!connectingScreen.activeSelf)
        //        connectingScreen.SetActive(true);
        //}else{
        //    if (connectingScreen.activeSelf)
        //        connectingScreen.SetActive(false);
        //}
		//base.player_coin.GetComponent<Text> ().text = ""+GameControllerTeenPatti.Total_coin;
		//if (CurrentNetwork != GameControllerTeenPatti.NetworkStatus) {
		//	CurrentNetwork = GameControllerTeenPatti.NetworkStatus;
		//	updateNewworkSignal ();
		//}

	}

	public void joinGame ()
	{

		appwrapTeenpatti.gameRequest ();

	}

	public override void ShowjoinBtn ()
	{
		JoinBtn.SetActive (true);
	}

	public override void HidejoinBtn ()
	{
		JoinBtn.SetActive (false);
	}

	public void SeeBtnAction ()
	{
		appwrapTeenpatti.CardSeeRequest ();
		//SeeBtn.SetActive (false);
		//GameDelegateTeenPatti.ShowCollectBootSound ();

	}

	public void TipAction ()
	{
		//Debug.Log ("TipAction working ");
		if (base.coin >= GameControllerTeenPatti.TipAmt) {
			//base.SendTip ();
			//GameControllerTeenPatti.Total_coin -= GameControllerTeenPatti.TipAmt;
			base.coin -= 10;
            base.totalCoin.GetComponent<Text> ().text = "" + base.coin;
			Debug.Log ("TipAction " + base.coin);

			appwrapTeenpatti.SendTip ();
		} else {
			GameControllerTeenPatti.showToast (MessageScriptTeenPatti.TIP_ERROR_MSG);
		}

	}

	public  override void SeeCard ()
	{
		this.SeeBtn.SetActive (false);
		GameDelegateTeenPatti.ShowEnableSideShow ();
		base.ShowUserCard ();


	}

    public void sendIn_Hand()
    {
        appwrapTeenpatti.sendCardInHand(base.CardValues);
    }

	void updateNewworkSignal ()
	{
		
	}

	public override void myTrun (int amount)
	{
		//GameControllerTeenPatti.Total_coin = base.coin;
        //PlayerPrefs.SetString(PlayerDetails.Coin,""+base.coin);
		GameDelegateTeenPatti.showPanel (amount);

		//Handheld.Vibrate ();

	}


	public override void ShowSeeBtn ()
	{
		bool flag = true;
		try {
			for (int i = 0; i < base.CardValues.Count; i++) {

				if (int.Parse (base.CardValues [i]) == -1) {
					flag = false;

				}
			}
		} catch (System.Exception ex) {
			Debug.Log ("2 " + ex.Message);
		}
		if (flag) {
			if (SeeBtn)
				SeeBtn.SetActive (true);
		}
	}

	public override void UpdateCoin (int TotalBetAmt)
	{
		//Debug.Log ("TotalBetAmt " + TotalBetAmt + " total Amt " + base.coin);
		try {
            //base.totalCoin.GetComponent<Text> ().text = "" + base.coin;
            //PlayerPrefs.SetString(PlayerDetails.Coin, "" + base.coin);
           
			player_coin.GetComponent<Text> ().text = "" + TotalBetAmt;
		} catch (System.Exception ex) {
			Debug.Log ("UpdateCoin " + ex.Message);
		}
	}

	public override void ResetGame ()
	{
		//Debug.Log ("ResetGame current ");
		try {
			player_coin.GetComponent<Text> ().text = "0";
		} catch (System.Exception ex) {
			Debug.Log ("UpdateCoin " + ex.Message);
		}

		PlayerStatus.SetActive (false);

		try {

			foreach (GameObject cc in cards) {
				cc.SetActive (false);
			}
			this.Betcoin = 0;


			base.SoundPlay = false;
			Color c = new Color (255.0f, 255.0f, 255.0f, 1.0f);
			player_Image.GetComponent<Image> ().color = c;
			 
//			for (int i = 0; i < cards.Length; i++) {
//				GameObject card = cards [i];
//				card.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images_Teenpatti/cards/backcard");
//			}
		} catch (System.Exception ex) {
			//Debug.Log ("ResetGame Exception " + ex.Message);
		}
	}

	public override void StatusMsg (string msg)
	{

		statusMsg.GetComponent<Text> ().text = "" + msg;
	}

	public override void UserLeft ()
	{
        Debug.Log("User Remove and move to lobby");
        try
        {
            appwrapTeenpatti.Disconnect();
            if (GameMenuTeenPattti.MoveToMainLobby)
            {
                SceneManager.LoadSceneAsync("MainLobby");
            }
            else
            {
                SceneManager.LoadSceneAsync("MainLobby");
            }
        }catch(System.Exception ed){
            Debug.Log(ed.Message);
        }
		//SceneManager.LoadSceneAsync ("Lobby_Teenpatti");
	}

	public override void hideSeeButton ()
	{
		if (SeeBtn.activeSelf) {
			this.SeeBtn.SetActive (false);
		}
	}

	public override void UserActive (string msg)
	{
		PlayerStatus.SetActive (true);
		try {
			Color c = player_Image.GetComponent<Image> ().color;
			c.a = 1.0f;
			player_Image.GetComponent<Image> ().color = c;
			Color c1 = player_Image.GetComponent<Image> ().color;
			c1.a = 1.0f;
			player_coinHolder.GetComponent<Image> ().color = c1;
			//PlayerStatus.GetComponent<Text> ().text = msg;
			player_coin.GetComponent<Text> ().text = "";
		} catch (System.Exception) {
		}
	}

	public override void UserInactive (string msg)
	{
		try {
			PlayerStatus.SetActive (true);
			Color c = player_Image.GetComponent<Image> ().color;
			//c.a = 0.5f;
			player_Image.GetComponent<Image> ().color = c;
			Color c1 = player_Image.GetComponent<Image> ().color;
			//c1.a = 0.5f;
			if (SeeBtn.activeSelf) {
				this.SeeBtn.SetActive (false);
			}
			//base.clearCards ();
			//PlayerStatus.GetComponent<Text> ().text = msg;
			player_coin.GetComponent<Text> ().text = "";
		} catch (System.Exception ex) {
			Debug.Log (ex.Message);
		}
	}

	public override void AddWinnerAmount ()
	{
		//PlayerTotalCoin.GetComponent<Text> ().text = "" + ;
	}

	public override void GetLiveRoomInfo ()
	{
		Debug.Log ("GetLiveRoomInfo working " + PlayerPrefs.GetString (PrefebTagsTeenpatti.ROOM_ID));
		StartCoroutine (getRoomDetails ());
	}


	IEnumerator getRoomDetails ()
	{
		yield return new WaitForSeconds (5.0f);
		if (WarpClient.GetInstance () != null) {
			WarpClient.GetInstance ().GetLiveRoomInfo (PlayerPrefs.GetString (PrefebTagsTeenpatti.ROOM_ID));
		}
	}

	public override void onTotalGameUser (int user)
	{
		Debug.Log ("Number of User " + user);
		if (user <= 1) {
			SceneManager.LoadSceneAsync ("MainLobby");
		}
	}
}

