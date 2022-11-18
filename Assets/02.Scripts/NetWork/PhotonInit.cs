using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //씬관련 기능
public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";
    public InputField userId;
    public InputField roomName;

    public GameObject scrollContents;
    public GameObject roomItem;
    void Awake()
    {   //포톤 클라우드 접속  버전별로
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(version);
        roomName.text = "Room " + Random.Range(0, 999).ToString("000");
    }
    void OnJoinedLobby()
    {
        Debug.Log("로비에 입장!");
        //PhotonNetwork.JoinRandomRoom(); //아무방이나 무작위 접속
        userId.text = GetUserId();
    }
    string GetUserId()
    {                       //키값을 예약
        string userId = PlayerPrefs.GetString("User_ID");
        if(string.IsNullOrEmpty(userId))
        {                                           //세자리로 표시 숫자
            userId = "User_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }
    void OnPhotonRandomJoinFailed() //룸접속에 실패할 경우 호출되는 콜백함수
    {
        print("No Room! ");
        PhotonNetwork.CreateRoom("MyRoom"); //룸생성
        //PhotonNetwork.CreateRoom("YourRoom", RoomOptions, TypedLobby.Default);
    }
    void OnJoinedRoom() //룸이 있고 룸에 입장하면 호출되는 콜백 함수
    {
        Debug.Log("룸에 입장!!");
        //CreateTank();
        StartCoroutine(LoadBattleField());
    }
    IEnumerator LoadBattleField()
    {   //씬이 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;
        //백그라운드 씬 로딩
        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");
        //비동기 방식으로 씬접속
        yield return ao;
    }
    public void OnClickJoinRandomRoom()
    {   //로컬 플레이어의 이름 설정
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        PhotonNetwork.JoinRandomRoom(); //무작위로 추출된 랜덤 방 입장
    }
    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if(string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "Room" + Random.Range(0, 999).ToString("000");
        }
        //로컬플레이어 이름 설정
        PhotonNetwork.player.NickName = userId.text;
        //플레이어 이름을 저장 C# List Dictionary //쌍 키 밸류
        PlayerPrefs.SetString("User_ID",userId.text);
        //생성할 룸의 조건 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true; //공개 방
        roomOptions.IsVisible = true; //방목록 리스트에 방제목이 보이게 안보이게
        roomOptions.MaxPlayers = 20; //최대 접속자수
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }
    //룸생성 실패할때
    void OnPhotonCreateRoomFailed(object[] error)//룸 입장이 안된 경우 출력되는 콜백 함수
    {
        Debug.Log(error[0].ToString()); //오류코드
        Debug.Log(error[1].ToString()); //오류난 원인 메세지 출력
    }
    //방이 생성되었거나 룸목록이 변경되었을때 호출되는 콜백 함수
    void OnReceivedRoomListUpdate()
    {           
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Room_Item"))
        {
            Destroy(obj);
        }
        //방정보 클래스               //방이 만들어지면 배열에 담은 것을 보여준다.
        foreach(RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            print(_room.Name);
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);
            //로컬좌표를 기준으로 하위에 생성된다

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;
            roomData.DisplayRoomData();
            //RoomItem의 Button 컴포넌트에 클릭 이벤트를 동적으로 연결
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.
                AddListener(delegate { OnClickRoomItem(roomData.roomName); });
        }
    }
    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        //인자로 전달된 이름에 해당하는 룸으로 입장
        PhotonNetwork.JoinRoom(roomName);
    }
    private void OnGUI() //글자의 형식을 스크린에 보여주는 함수
    {                    //콜백 함수
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        //화면에 포톤네트워크 접속 정보를 보여준다.
    }
}