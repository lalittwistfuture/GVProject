using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class LobbyTeenPatti : MonoBehaviour
{


	public GameObject PrivateBtn;

	void Start ()
	{
		PlayerPrefs.SetInt (Game.Type, Game.TeenPatti);
	/*	if ((PlayerPrefs.GetInt (PlayerDetails.RealMoney) == 1)) {

			PrivateBtn.SetActive (true);
		} else {
			PrivateBtn.SetActive (false);
		}*/
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PressBackBututon ();
		}
	}

	void FixedUpdate ()
	{
		

	}

	public void PressBackBututon ()
	{
		SceneManager.LoadSceneAsync ("MainLobby");
	}

	//	IEnumerator loadImage (string url)
	//	{
	//		//Debug.Log ("load " + url);
	//		WWW www = new WWW (url);
	//		yield return www;
	//		try{
	//		if (www.error == null) {
	//			PlayerPic.sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
	//		} else {
	//			Debug.Log ("Error occur while downloading");
	//			PlayerPic.sprite = Resources.Load<Sprite> ("Images_Teenpatti/user_Teenpatti");
	//		}
	//		}catch(System.Exception ex){
	//			Debug.Log (ex.Message);
	//		}
	//	}

	public void PlayNowAction ()
	{
		GameControllerTeenPatti.isChallenge = false;
		GameControllerTeenPatti.GameType = TagsTeenpatti.PUBLIC;
		GameControllerTeenPatti.PrivateGameType = "";
      //  MyGameController.rummyGame.GameType = Game.PRACTICE;
		SceneManager.LoadSceneAsync ("AmountSelectionTeenpatti");
		
	}

	public void PlayPrivateAction ()
	{
		GameControllerTeenPatti.isChallenge = true;
		GameControllerTeenPatti.GameType = TagsTeenpatti.PRIVATE;
        SceneManager.LoadSceneAsync ("TeenPattiPrivate");

	}

	//
	//	private IEnumerator ServerRequest (WWW www)
	//	{
	//		yield return www;
	//
	//		if (www.error == null) {
	//			string response = www.text;
	//			JSONNode node = JSON.Parse (response);
	//			if (node != null) {
	//				string result = node ["status"];
	//				string msg = node ["message"];
	//				Debug.Log ("result is " + result);
	//				if (result.Equals ("OK")) {
	//					try{
	//						JSONNode data1 = node ["data"];
	//						JSONNode data = data1 [0];
	//						int coin = int.Parse(data ["balance"]);
	//						PlayerPrefs.SetInt (PlayerDetails.Coin, coin);
	//						GameControllerTeenPatti.Total_coin = coin;
	//						PlayerCoin.text = "" + GameControllerTeenPatti.Total_coin;
	//						PlayerCoin.GetComponent<Text> ().text = ""+PlayerPrefs.GetInt (PlayerDetails.Coin);
	//						GameController.coin = coin;
	//						GameController.practic_coin = coin;
	//					}catch(System.Exception ex){
	//						Debug.Log (ex.Message);
	//					}
	//				} else {
	//
	//				}
	//
	//			}
	//
	//		}
	//
	//	}



		

}
