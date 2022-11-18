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
            //ù��° �θ� ������Ʈ�� ����
        }                
        //�κ������ �Ѿ���� �ٽ� ����Ŭ���� �������� �޼����� ���Ź޾ƾ� �Ѵ�.
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
    {   //���� ������ �� ������ �޾ƿ�
        Room curRoom = PhotonNetwork.room;
        //���� ���� ������ ���� �ִ� ���� ������ ���� ���ڿ��� �������� Text UI �׸����� ���
        txtConnect.text = "<color=#ff0000>" + curRoom.PlayerCount.ToString() + "</color>" + " / " + curRoom.MaxPlayers.ToString();
    }
    //��Ʈ��ũ �÷��̾ ���������� ȣ��
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GetConnectPlayerCount();
    }
    //��Ʈ��ũ �÷��̾ �����ߴٰ� ������ �� ȣ��Ǵ� �Լ�
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }
    public void OnClickExit()
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.NickName + "]DisConnected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        //���� ���� ���� ������ ������ ��� ��Ʈ��ũ ��ü�� ����
        PhotonNetwork.LeaveRoom();
    }
    //�뿡�� ���� ����Ǿ��� �� ȣ��Ǵ� �ݹ� �Լ�
    void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
