using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;
namespace Ludo
{
    public class GotiScript : MonoBehaviour
    {


        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int TOP = 3;
        public const int BOTTOM = 4;
        public const int CENTER = 5;
        //public const int LIMIT = 10;
        public const int LIMIT = 56;
        public const int INITIATE = -1;
        private GameObject Panel;
        private GameObject animator;
        [HideInInspector]
        public int openPoint = 0;
        [HideInInspector]
        public int winingPoint = 0;
        [HideInInspector]
        public int currentPosition = INITIATE;
        [HideInInspector]
        public int initialPosition;
        [HideInInspector]
        public int winingGate;
        [HideInInspector]
        private bool Enable = false;
        [HideInInspector]
        private int destination = 0;
        [HideInInspector]
        public bool isHome = false;
        [HideInInspector]
        public int homeEntryPoint = 0;
        private JSONNode TurnValue;
        [HideInInspector]
        public Vector3 InitialPositionVector;
        [HideInInspector]
        public int index;
        [HideInInspector]
        public int CellNumber = -1;
        string playerID = "";
        [HideInInspector]
        public bool isMoving = false;
        [HideInInspector]
        public bool isLock = true;
        [HideInInspector]
        public bool isWin = false;



        Color myColor;
        // Use this for initialization
        void Start()
        {

            if (Application.platform != RuntimePlatform.Android)
            {
                GetComponent<Button>().enabled = true;
            }else{
                GetComponent<Button>().enabled = false;  
            }


            InitialPositionVector = transform.position;
            animator = transform.Find("Animation").gameObject;
            animator.SetActive(false);

        }


        public bool moveValidation(JSONNode numbers)
        {

            if (isLock && userHasOpenNumber(numbers))
            {

                this.TurnValue = numbers;
                showAnimation();
                return true;
            }
            else if ((!isLock) && gotiLimit(numbers))
            {

                this.TurnValue = numbers;
                showAnimation();
                return true;
            }
            return false;
        }

        public bool gotiLimit(JSONNode numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                int num = int.Parse(numbers[i]) + currentPosition;


                if (num <= LIMIT)
                {

                    return true;
                }
            }
            return false;
        }




        public bool userHasOpenNumber(JSONNode numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                int num = int.Parse(numbers[i]);
                if (num == 1 || num == 6)
                {
                    return true;
                }
            }
            return false;
        }


        public void setPlayer(string id, string type, Color color)
        {
            this.myColor = color;
            this.playerID = id;
            transform.Find("Icon").GetComponent<Image>().color = color;

            switch (type)
            {
                case "Player1":
                    {
                        initialPosition = 28;
                        homeEntryPoint = 65;
                        winingPoint = 70;

                    }
                    break;
                case "Player2":
                    {
                        initialPosition = 2;
                        homeEntryPoint = 53;
                        winingPoint = 58;

                    }
                    break;
                case "Player4":
                    {
                        initialPosition = 15;
                        homeEntryPoint = 59;
                        winingPoint = 64;

                    }
                    break;
                case "Player3":
                    {
                        initialPosition = 41;
                        homeEntryPoint = 71;
                        winingPoint = 76;

                    }
                    break;
                default:
                    break;
            }
        }


        void onGotiWin(int value, string player)
        {
            if (player.Equals(this.playerID))
            {
                if (value == this.index)
                {
                    gameObject.SetActive(false);
                }
            }
        }


        void OnEnable()
        {

            LudoDelegate.onStopSelection += onStopSelection;

        }

        void OnDisable()
        {
            // LudoDelegate.onNumberSelection -= onNumberSelection;
            LudoDelegate.onStopSelection -= onStopSelection;
        }




        void onStopSelection()
        {
            Enable = false;
            animator.GetComponent<Animation>().Stop();
            animator.SetActive(false);
        }



        public void moveGoti(int current, int next)
        {
            if (next >= 0)
            {
                try
                {

                    if (!this.isMoving)
                        StartCoroutine(moveGotiSteps(next));
                }
                catch (System.Exception ex)
                {
                    Debug.Log("moveGoti Exception " + ex.Message);
                }

            }
            if (next == -1)
            {
                transform.position = InitialPositionVector;
                transform.localScale = new Vector3(1f, 1f, 1f);
                currentPosition = -1;
                isLock = true;
                LudoDelegate.closeGoti();
                this.CellNumber = -1;

            }
            LudoDelegate.refreshCell();
        }

        IEnumerator moveGotiSteps(int next)
        {

            this.isMoving = true;
            int i = currentPosition + 1;

            while (i <= next)
            {
                int nextPos = 0;
                int Object = 0;
                if (i >= 51)
                {
                    nextPos = homeEntryPoint + i - 51;
                    Object = nextPos;
                }
                else
                {
                    nextPos = initialPosition + i;
                    Object = nextPos > 52 ? nextPos - 52 : nextPos;
                }


                GameObject pos = GameObject.Find("" + Object);
                transform.position = pos.transform.position;
                currentPosition = i;
                this.CellNumber = Object;
                LudoDelegate.refreshCell();
                LudoDelegate.playGotiSound();
                i++;
                yield return new WaitForSeconds(0.2f);
            }
            if (next == GotiScript.LIMIT)
            {
                gotiWin();
            }
            Debug.Log(this.playerID + " ----" + this.index + " : " + next);
            checkSafePlace();
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
            {
                LudoDelegate.movementComplete();
            }
            this.isMoving = false;

        }




        void checkSafePlace()
        {

            foreach (int j in LudoManager.safePlace)
            {
                if (j == this.CellNumber)
                {
                    LudoDelegate.safePlace();
                }
            }
        }


        void MovePanel()
        {
            int Position = GotiScript.getGotiPosition(transform.gameObject);
            switch (Position)
            {
                case GotiScript.LEFT:
                    if (TurnValue.Count == 2)
                    {
                        Panel.transform.localPosition = new Vector3(16.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    else
                    {
                        Panel.transform.localPosition = new Vector3(32.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    break;
                case GotiScript.RIGHT:
                    if (TurnValue.Count == 2)
                    {
                        Panel.transform.localPosition = new Vector3(-16.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    else
                    {
                        Panel.transform.localPosition = new Vector3(-32.0f, Panel.transform.localPosition.y, 0.0f);
                    }
                    break;
                case GotiScript.TOP:
                    Panel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                case GotiScript.BOTTOM:
                    Panel.transform.localPosition = new Vector3(0.0f, 27.9f, 0.0f);
                    break;
                default:
                    Panel.transform.localPosition = new Vector3(0.0f, 27.9f, 0.0f);
                    break;
            }


        }


        public void readyGoti(JSONNode steps)
        {
            this.TurnValue = steps;


            showAnimation();

        }
        public void gotiWin()
        {
            Debug.Log("Goti Win " + this.index);
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) == LudoTags.OFFLINE)
            {
                LudoDelegate.gotiWin(this.index, this.playerID);
            }
            isWin = true;
            transform.gameObject.SetActive(true);
        }

        public void showAnimation()
        {

            this.Enable = true;
            animator.SetActive(true);
            animator.GetComponent<Animation>().Play();
        }


        public void gotiAction()
        {

            if (TurnValue.Count > 1)
            {
                if (currentPosition == -1)
                {

                    if (userHasOne() && userHasSix())
                    {
                        LudoDelegate.showPanel(playerID, TurnValue, this.myColor, transform.position, this.index);
                    }
                    else
                    {
                        if (userHasSix())
                        {
                            LudoDelegate.selectNumber(6, this.playerID, this.index);
                        }
                        else if (userHasOne())
                        {
                            LudoDelegate.selectNumber(1, this.playerID, this.index);
                        }
                    }
                }
                else
                {
                    LudoDelegate.showPanel(playerID, TurnValue, this.myColor, transform.position, this.index);
                }

            }
            else if (TurnValue.Count == 1)
            {

                int number = int.Parse(TurnValue[0]);

                if (currentPosition == -1)
                {
                    if (number == 6 || number == 1)
                        LudoDelegate.selectNumber(number, this.playerID, this.index);


                }
                if (currentPosition != -1)
                {
                    LudoDelegate.selectNumber(number, this.playerID, this.index);

                }
            }

        }


		private void Update()
		{
            if (Application.platform == RuntimePlatform.Android)
            {

                PointerEventData pointer = new PointerEventData(EventSystem.current);
                List<RaycastResult> raycastResult = new List<RaycastResult>();

                foreach (Touch touch in Input.touches)
                {
                    pointer.position = touch.position;
                    EventSystem.current.RaycastAll(pointer, raycastResult);
                    foreach (RaycastResult result in raycastResult)
                    {
                        if (result.gameObject == this.gameObject)
                        {
                            if (touch.phase == TouchPhase.Began)
                            {
                                if (result.gameObject.GetComponent<GotiScript>().index == this.index)
                                {
                                    TapGotiAction();
                                    break;
                                }

                            }


                        }

                    }

                    raycastResult.Clear();

                }
            }
		}


		public void TapGotiAction()
        {


            if (this.Enable)
            {
                if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
                {

                    if (this.playerID.Equals(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID)))
                    {
                        gotiAction();
                    }

                }
                else
                {
                    gotiAction();
                }

            }else{
                Debug.Log("is not enable");
            }

            LudoDelegate.refreshCell();

        }


        public bool userHasSix()
        {
            for (int i = 0; i < TurnValue.Count; i++)
            {
                if (int.Parse(TurnValue[i]) == 6)
                {
                    return true;
                }
            }
            return false;
        }

        public bool userHasOne()
        {
            for (int i = 0; i < TurnValue.Count; i++)
            {
                if (int.Parse(TurnValue[i]) == 1)
                {
                    return true;
                }
            }
            return false;
        }


        public static int getGotiPosition(GameObject goti)
        {
            GameObject left = GameObject.Find("72");
            GameObject right = GameObject.Find("60");
            GameObject upper = GameObject.Find("53");
            GameObject bottom = GameObject.Find("65");
            if (goti.transform.position.x < left.transform.position.x)
            {
                return GotiScript.LEFT;
            }
            if (goti.transform.position.x > right.transform.position.x)
            {
                return GotiScript.RIGHT;
            }
            if (goti.transform.position.y > upper.transform.position.y)
            {
                return GotiScript.TOP;
            }
            if (goti.transform.position.y < bottom.transform.position.y)
            {
                return GotiScript.BOTTOM;
            }

            return GotiScript.CENTER;

        }

    }
}