using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster :MonoBehaviour
{
    public Ball ball;
    public Transform PosA;
    public Transform PosB;
    public GameObject PosAG;//A場地
    public GameObject PosBG;//B場地
    public static GameMaster _instance;
    
    public GameObject View;
    [SerializeField]
    Text GameTimeText , secondsText;
    [SerializeField]
    public int point = 0;
    public Text myScore;
    public Text otherScore;
    public int second = 60;   
    public void SetScore(int m , int o)
    {
        myScore.text = m.ToString();
        otherScore.text = o.ToString();
    }
	void Awake()
    {
        _instance = this;
    }
    
	void Start () {
        View.SetActive(false);
        PhotonNetwork.OnEventCall += OnReceiveMessage;
    }      
    public void StartTheGameCoroutine()
    {        
        ball.photonView.RPC("OnMove", PhotonTargets.All);//球的位移開啟
        StartCoroutine(StartGame());
    }
    public void BallOnGroundB()
    {
        //到場地B A場地得分
        GainPointPosB();
        StartCoroutine(ResetBall(PosB));
    }
   
    public void BallOnGroundA()
    {
        GainPointPosA();
        StartCoroutine(ResetBall(PosA));
    }

    public void GainPointPosA()
    {
        byte evCode = 2;//事件分組 可用0~200
        bool reliable = true;//傳輸事件是否確保正確傳輸給其他人
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent(evCode, PosAG.name, reliable, options);//所有人做傳輸

    }
    public void GainPointPosB()
    {
        byte evCode = 2;//事件分組 可用0~200
        bool reliable = true;//傳輸事件是否確保正確傳輸給其他人
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent(evCode, PosBG.name, reliable, options);//所有人做傳輸

    }

    void OnReceiveMessage(byte eventCode, object content, int senderId)
    {
        print(content.ToString());
        if (eventCode == 0)
        {
            switch (content.ToString())
            {
                case "close":
                    View.SetActive(false);
                    break;
                default:
                    View.SetActive(true);
                    secondsText.text = content.ToString() + "秒後落下";
                    break;
            }
        }
        else if(eventCode == 1)
        {
            if (int.Parse(content.ToString()) < 10)
            {
                GameTimeText.text = "<color=#F00>" + content.ToString() + "</color>";
            }
            else
            {
                GameTimeText.text = "<color=#00F>" + content.ToString() + "</color>";
            }
            
        }
        else if (eventCode == 2)
        {

            //如果我是這個場的GameMaster就對自己加分
            //SetScore(我的分數,對面分數);

            //GameObject [] players  = GameObject.FindGameObjectWithTag("Player");
            //foreach(GameObject player in players)
            //{                
            //    if (player.GetComponent<PlayMove>().Pos.name == (string)content)
            //    {
            //        print("執行更新分數");
            //GameObject.FindGameObjectWithTag("Player").GetPhotonView().RPC("Gain", PhotonTargets.All, (string)content);
            //        break;
            //    }
                
            //}
        }

    }
    IEnumerator StartGame()
    {
        byte evCode = 1;//事件分組 可用0~200
        bool reliable = true;//傳輸事件是否確保正確傳輸給其他人
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        for (int i = 60; i >=0; i--)
        {
            PhotonNetwork.RaiseEvent(evCode, i, reliable, options);//所有人做傳輸
            yield return new WaitForSeconds(1);
        }
    }
    
    IEnumerator ResetBall(Transform pos)
    {
        byte evCode = 0;//事件分組 可用0~200
        bool reliable = true;//傳輸事件是否確保正確傳輸給其他人
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };        
        ball.transform.position = pos.position;       
        ball.photonView.RPC("OnGravityClose", PhotonTargets.All);
        int resetSecond = 3;
        for (int  i = 0;  i < 3;  i++)
        {           
            Send(resetSecond);
            resetSecond--;
            yield return new WaitForSeconds(1);
        }
        ball.photonView.RPC("OnGravityOpen", PhotonTargets.All);
        PhotonNetwork.RaiseEvent(evCode, "close", reliable, options);//所有人做傳輸
        yield break;
    }

    void Send(int second)
    {
        byte evCode = 0;//事件分組 可用0~200
        bool reliable = true;//傳輸事件是否確保正確傳輸給其他人
                             //  RaiseEventOptions options = null; //事件一些選項 例如傳輸的對象，是否快取等。
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent(evCode, second, reliable, options);//所有人做傳輸
    }
}
