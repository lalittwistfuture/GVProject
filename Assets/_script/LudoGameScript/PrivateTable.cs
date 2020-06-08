using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class PrivateTable : MonoBehaviour
    {
        public Text TableCode;

        void Start()
        {
        }

        public void ShareCode()
        {
            int entryFee = PlayerPrefs.GetInt(LudoTags.ENTRY_FEE);
            string tableCode = TableCode.text;
            string s = "Join me on BetLudo table '"+tableCode+"' of Rs." + entryFee + ". Download Link 'http://betludo.com/game/ludo.apk'.";
            GameConstantData.shareText(s);
        }


        public void updateRoomID(string code)
        {
            TableCode.text = code;
        }

    }
}