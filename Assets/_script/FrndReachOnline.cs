using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class FrndReachOnline : MonoBehaviour
    {

        public Text FrndName;
        public GameObject player_Image;
        public GameObject ClosePanel;
        public GameObject ChallengeBtn;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ChallengeFrnd()
        {

            PlayerPrefs.SetString(GameTags.PRIVATE_TABLE_TYPE, GameTags.FB_FRIEND_ONLINE);
            SceneManager.LoadSceneAsync("GameScene");
            ClosePanel.SetActive(false);
            transform.gameObject.SetActive(false);

        }

        public void DismisAction()
        {
            ClosePanel.SetActive(false);
            transform.gameObject.SetActive(false);
        }



        //	public void ShowPanel (string FBId, string player_name, string roomID)
        //	{
        //		
        //		FrndName.text = "Your Friend " + player_name + " is Online";
        //		string fbImageUrl = "https://graph.facebook.com/" + FBId + "/picture?type=large";
        //		if (fbImageUrl.Length != 0) {
        //			StartCoroutine (loadImage (fbImageUrl));
        //		} else {
        //			
        //			try {
        //				player_Image.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("images/user-default");
        //			} catch (System.Exception ex) {
        //				Debug.Log ("ShowPanel Exception " + ex.Message);
        //			}
        //		}
        //			
        //
        //	}

        IEnumerator loadImage(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                player_Image.GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            }
            else
            {
                //Debug.Log ("Error occur while downloading");
                player_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/user-default");
            }
        }

    }
}