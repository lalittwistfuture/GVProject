using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitApplicationTeenpatti : MonoBehaviour
{

	public Text QuitTitle;
	// Use this for initialization
	void Start ()
	{
		QuitTitle.text = MessageScriptTeenPatti.QUIT_APPLICATION;

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
		Application.Quit ();
	}

}
