using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmountSelectionTeenpatti : MonoBehaviour {

	private bool amountSelected = false;
	// Use this for initialization
	void Start () {
		amountSelected = false;
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PressBackBututon ();
		}
	}
	public void PressBackBututon () {
		SceneManager.LoadSceneAsync ("MainLobby");
	}
	void clickBtn (GameObject btn) {

	}
	/*public void AddPointAction ()
	{
		if (rollerCount <= 5) {

			if (rollerCount == 5) {

			} else {
				rollerCount++;
			}

			GameObject g = GameObject.Find ("pos_" + rollerCount);
			int i = rollerCount - 1;
			GameObject pointValue = GameObject.Find ("Text_" + i);
			string ptValue = pointValue.GetComponent<Text> ().text;
			Debug.Log ("coin value " + ptValue);
			MoveRoller (g.transform, ptValue);

		}
		Debug.Log ("count " + rollerCount);

	}*/

	/*public void SubstractPointAction ()
	{
		if (rollerCount >= 1) {

			if (rollerCount == 1) {

			} else {
				rollerCount--;
			}

			GameObject g = GameObject.Find ("pos_" + rollerCount);
			int i = rollerCount - 1;
			GameObject pointValue = GameObject.Find ("Text_" + i);
			string ptValue = pointValue.GetComponent<Text> ().text;
			Debug.Log ("coin value " + ptValue);
			MoveRoller (g.transform, ptValue);

		}
		Debug.Log ("count " + rollerCount);

	}*/

	public void PlayNow () {
		//GameControllerTeenPatti.BootAmount
		if (int.Parse (PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_COIN)) >= GameControllerTeenPatti.MaxBetAmt) {
			//SceneManager.LoadSceneAsync ("AmountInHand");
			PlayerPrefs.SetString ("InHand", PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_COIN));
			SceneManager.LoadScene ("GameScene_Teenpatti");
		} else {
			Debug.Log ("Play NOW");
			GameControllerTeenPatti.showToast ("Account balence very low?");
		}
	}

	/*	public void Point1Action ()
	    {
	        Debug.Log ("Point1Action working");
	        rollerCount = 1;
	        MoveRoller (pos1, pos1value.GetComponent<Text> ().text);
	    }

	    public void Point2Action ()
	    {
	        Debug.Log ("Point2Action working");
	        rollerCount = 2;
	        MoveRoller (pos2, pos2value.GetComponent<Text> ().text);
	    }

	    public void Point3Action ()
	    {
	        Debug.Log ("Point3Action working");
	        rollerCount = 3;
	        MoveRoller (pos3, pos3value.GetComponent<Text> ().text);
	    }

	    public void Point4Action ()
	    {
	        Debug.Log ("Point4Action working");
	        rollerCount = 4;
	        MoveRoller (pos4, pos4value.GetComponent<Text> ().text);

	    }

	    public void Point5Action ()
	    {
	        Debug.Log ("Point5Action working");
	        rollerCount = 5;
	        MoveRoller (pos5, pos5value.GetComponent<Text> ().text);
	    }*/

	public void MoveRoller (Transform pos, string value) {
		//iTween.MoveTo (roller, pos.position, 1.0f);
		int count = int.Parse (value);
		//	ShowPointValue.GetComponent<Text> ().text = "" + count * 1024;
		//MaxBet.GetComponent<Text> ().text = "" + count * 128;
		float selectedPrice = float.Parse (value);

		//Debug.Log ("count " + count);

		GameControllerTeenPatti.MaxBetAmt = count * 128;
		GameControllerTeenPatti.BootAmount = count;
		GameControllerTeenPatti.PortLimit = count * 1024;
	}
}