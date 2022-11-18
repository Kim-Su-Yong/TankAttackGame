using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public UnityEngine.UI.Text txtConnect;
    public UnityEngine.UI.Text txtLogMsg;
    [SerializeField]
    private List<Transform> PointsList;
    [SerializeField]
    private PhotonView pv = null;
    void Awake()
    {
        pv = PhotonView.Get(this);
        var point = GameObject.Find("ApacheSpawnPoints");
        if(point != null)
        {
            point.GetComponentsInChildren<Transform>(PointsList);
            PointsList.RemoveAt(0);
            //첫번째 부모 오브젝트는 제거
        }                
        //로비씬에서 넘어오면 다시 포톤클라우드 서버에서 메세지를 수신받아야 한다.
        PhotonNetwork.isMessageQueueRunning = true;
        CreateTank();
        InvokeRepeating("CreateApache", 0.2f, 3.0f);
        GetConnectPlayerCount();
    }
    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20f, pos), Quaternion.identity, 0);
    }
    private void Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.NickName + "]Connected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
    }
    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text = txtLogMsg.text + msg;
    }
    void CreateApache()
    {
        int count = (int)GameObject.FindGameObjectsWithTag("Apache").Length;
        if (count < 10)
        {
            int idx = Random.Range(0, PointsList.Count);
            PhotonNetwork.InstantiateSceneObject("Apache", PointsList[idx].position, PointsList[idx].rotation, 0, null);
        }
    }
    void GetConnectPlayerCount()
    {   //현재 입장한 룸 정보를 받아옴
        Room curRoom = PhotonNetwork.room;
        //현재 룸의 접속자 수와 최대 접속 가능한 수를 문자열로 구성한후 Text UI 항목으로 출력
        txtConnect.text = "<color=#ff0000>" + curRoom.PlayerCount.ToString() + "</color>" + " / " + curRoom.MaxPlayers.ToString();
    }
    //네트워크 플레이어가 접속했을때 호출
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GetConnectPlayerCount();
    }
    //네트워크 플레이어가 접속했다가 나갓을 때 호출되는 함수
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }
    public void OnClickExit()
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.NickName + "]DisConnected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        //현재 룸을 빠져 나가며 생성한 모든 네트워크 객체를 삭제
        PhotonNetwork.LeaveRoom();
    }
    //룸에서 접속 종료되었을 때 호출되는 콜백 함수
    void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
