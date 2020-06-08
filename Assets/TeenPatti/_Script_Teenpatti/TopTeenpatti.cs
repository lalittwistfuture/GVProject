using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopTeenpatti : MonoBehaviour
{

	public GameObject SoundBtn;
	public GameObject GameMenu;
	public GameObject ClosePanel;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void InvitePeopleAction ()
	{
		
	}

	public void SoundAction ()
	{
		if (GameControllerTeenPatti.isSoundOn) {

			GameControllerTeenPatti.isSoundOn = false;
			SoundBtn.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images_Teenpatti/sound_off");
		} else {
			GameControllerTeenPatti.isSoundOn = true;
			SoundBtn.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Images_Teenpatti/sound_on");
		}
	}

	public void CurrencyAction ()
	{

	}

	public void QuestionMarkAction ()
	{

	}

	public void MenuBar ()
	{
		ClosePanel.SetActive (true);
		GameMenu.SetActive (true);
	}

}
