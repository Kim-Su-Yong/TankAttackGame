using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //������ ���
public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";
    public InputField userId;
    public InputField roomName;

    public GameObject scrollContents;
    public GameObject roomItem;
    void Awake()
    {   //���� Ŭ���� ����  ��������
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(version);
        roomName.text = "Room " + Random.Range(0, 999).ToString("000");
    }
    void OnJoinedLobby()
    {
        Debug.Log("�κ� ����!");
        //PhotonNetwork.JoinRandomRoom(); //�ƹ����̳� ������ ����
        userId.text = GetUserId();
    }
    string GetUserId()
    {                       //Ű���� ����
        string userId = PlayerPrefs.GetString("User_ID");
        if(string.IsNullOrEmpty(userId))
        {                                           //���ڸ��� ǥ�� ����
            userId = "User_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }
    void OnPhotonRandomJoinFailed() //�����ӿ� ������ ��� ȣ��Ǵ� �ݹ��Լ�
    {
        print("No Room! ");
        PhotonNetwork.CreateRoom("MyRoom"); //�����
        //PhotonNetwork.CreateRoom("YourRoom", RoomOptions, TypedLobby.Default);
    }
    void OnJoinedRoom() //���� �ְ� �뿡 �����ϸ� ȣ��Ǵ� �ݹ� �Լ�
    {
        Debug.Log("�뿡 ����!!");
        //CreateTank();
        StartCoroutine(LoadBattleField());
    }
    IEnumerator LoadBattleField()
    {   //���� �̵��ϴ� ���� ���� Ŭ���� �����κ��� ��Ʈ��ũ �޽��� ���� �ߴ�
        PhotonNetwork.isMessageQueueRunning = false;
        //��׶��� �� �ε�
        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");
        //�񵿱� ������� ������
        yield return ao;
    }
    public void OnClickJoinRandomRoom()
    {   //���� �÷��̾��� �̸� ����
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        PhotonNetwork.JoinRandomRoom(); //�������� ����� ���� �� ����
    }
    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if(string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "Room" + Random.Range(0, 999).ToString("000");
        }
        //�����÷��̾� �̸� ����
        PhotonNetwork.player.NickName = userId.text;
        //�÷��̾� �̸��� ���� C# List Dictionary //�� Ű ���
        PlayerPrefs.SetString("User_ID",userId.text);
        //������ ���� ���� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true; //���� ��
        roomOptions.IsVisible = true; //���� ����Ʈ�� �������� ���̰� �Ⱥ��̰�
        roomOptions.MaxPlayers = 20; //�ִ� �����ڼ�
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }
    //����� �����Ҷ�
    void OnPhotonCreateRoomFailed(object[] error)//�� ������ �ȵ� ��� ��µǴ� �ݹ� �Լ�
    {
        Debug.Log(error[0].ToString()); //�����ڵ�
        Debug.Log(error[1].ToString()); //������ ���� �޼��� ���
    }
    //���� �����Ǿ��ų� ������ ����Ǿ����� ȣ��Ǵ� �ݹ� �Լ�
    void OnReceivedRoomListUpdate()
    {           
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Room_Item"))
        {
            Destroy(obj);
        }
        //������ Ŭ����               //���� ��������� �迭�� ���� ���� �����ش�.
        foreach(RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            print(_room.Name);
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);
            //������ǥ�� �������� ������ �����ȴ�

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;
            roomData.DisplayRoomData();
            //RoomItem�� Button ������Ʈ�� Ŭ�� �̺�Ʈ�� �������� ����
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.
                AddListener(delegate { OnClickRoomItem(roomData.roomName); });
        }
    }
    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        //���ڷ� ���޵� �̸��� �ش��ϴ� ������ ����
        PhotonNetwork.JoinRoom(roomName);
    }
    private void OnGUI() //������ ������ ��ũ���� �����ִ� �Լ�
    {                    //�ݹ� �Լ�
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //ȭ�鿡 �����Ʈ��ũ ���� ������ �����ش�.
    }
}