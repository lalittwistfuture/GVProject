using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lotto
{
    public class LottoGameController
    {

        public delegate void StartNewSession();
        public static event StartNewSession onStartNewSession;
        public static void startSession()
        {
            if (onStartNewSession != null)
            {
                onStartNewSession();
            }
        }

        public delegate void SessionClose();
        public static event SessionClose onSessionClose;
        public static void closeSession()
        {
            if (onSessionClose != null)
            {
                onSessionClose();
            }
        }

        public delegate void RefreshWallet(SimpleJSON.JSONNode data);
        public static event RefreshWallet onRefreshWallet;
        public static void refreshWallet(SimpleJSON.JSONNode data)
        {
            if (onRefreshWallet != null)
            {
                onRefreshWallet(data);
            }
        }

        public delegate void UpdateWallet();
        public static event UpdateWallet onUpdateWallet;
        public static void updateWallet()
        {
            if (onUpdateWallet != null)
            {
                onUpdateWallet();
            }
        }

        public delegate void BettingStop();
        public static event BettingStop onBettingStop;
        public static void stopBetting()
        {
            if (onBettingStop != null)
            {
                onBettingStop();
            }
        }

        public delegate void TicketClick();
        public static event TicketClick onTicketClick;
        public static void clickTicket()
        {
            if (onTicketClick != null)
            {
                onTicketClick();
            }
        }
    }
}