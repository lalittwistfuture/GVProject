using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitCurrentGameTeenpatti : MonoBehaviour
{

	public Text QuitGameText;
	// Use this for initialization
	void Start ()
	{

		QuitGameText.text = MessageScriptTeenPatti.QUIT_GAME_MESSAGE;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ClosePanel ()
	{
		transform.gameObject.SetActive (false);
	}

	public void YesAction ()
	{

		try {
			appwrapTeenpatti.LeftRoom ();
			appwrapTeenpatti.Disconnect ();
			transform.gameObject.SetActive (false);
            SceneManager.LoadScene ("MainLobby");
		} catch (System.Exception ex) {
			Debug.Log (ex.Message);
		}
	}
		

}
