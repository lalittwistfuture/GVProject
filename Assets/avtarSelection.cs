using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avtarSelection : MonoBehaviour
{
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickOnAvtar(int value)
    {

        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "" + value);
        Debug.Log(value);
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avtar/" + value);
        GameDelegateTeenPatti.updateImage("" + value);

    }
    public void Ok()
    {
        transform.gameObject.SetActive(false);
  
    }

}
