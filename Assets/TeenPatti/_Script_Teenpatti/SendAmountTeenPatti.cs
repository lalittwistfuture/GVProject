using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class SendAmountTeenPatti : MonoBehaviour
{

	public GameObject Coin;
	int coinAmt = 0;
	// Use this for initialization
	void Start ()
	{
		Coin = transform.GetChild (0).gameObject;
        Coin.GetComponent<Text> ().text = PlayerPrefs.GetString(PlayerDetails.Coin);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void UpdateCoin (int myCoin, string playerID)
	{
		if (playerID.Equals (GameControllerTeenPatti.Player_ID)) {
			
			Debug.Log ("Current Player Amt " + myCoin);
		} else {

			try {
				coinAmt = coinAmt + myCoin;
				Coin.GetComponent<Text> ().text = "" + coinAmt;
			} catch (System.Exception ex) {
				Debug.Log ("UpdateCoin Exception " + ex.Message);
			}
		}

	}

	public void Resetcoin ()
	{
		try {
			Coin.GetComponent<Text> ().text = "";
			coinAmt = 0;
		} catch (System.Exception ex) {
			Debug.Log ("UpdateCoin Exception " + ex.Message);
		}
	}





}
