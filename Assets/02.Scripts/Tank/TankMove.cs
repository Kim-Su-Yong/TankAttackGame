using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//앞뒤 조정 : W,S
//회전 : A,D
public class TankMove : MonoBehaviour
{
    private float h = 0f, v = 0f;
    [SerializeField]
    private float moveSpeed = 12f;
    [SerializeField]
    private float rotSpeed = 80f;
    private Transform tr;
    private Rigidbody rbody;
    public bool isGetIn = false;
    public bool isGetOut = true;
    private PhotonView pv = null;
    private Vector3 CurPos = Vector3.zero;
    private Quaternion CurRot = Quaternion.identity;
    public TankDamage damage;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        tr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
        damage = GetComponent<TankDamage>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        //통신유형은 UDP였다가 중요한 데이터인 경우에는 TCP/IP 방식으로 바뀔수 있다.
        pv.ObservedComponents[0] = this;
        //자기자신 클래스 즉 TankMove의 움직임
        if (pv.isMine) //포톤뷰가 네트워크상에 나의 것이라면
        {
            Camera.main.GetComponent<FollowCamera>().target = tr;
        }    
        //탱크나 자동차는 무게 중심을 낮게 설정 이렇게 해야 넘어지지 않는다.
        rbody.centerOfMass = new Vector3(0f, -0.5f, 0f);
        CurPos = tr.position;
        CurRot = tr.rotation;
    }
    //움직임을 송수신하는 함수
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) //송신
        {
            stream.SendNext(tr.position); //자신의 위치값
            stream.SendNext(tr.rotation); //자신의 회전값
        }
        if(stream.isReading) //다른 네트워크 유저 움직임을 수신
        {
            CurPos = (Vector3)stream.ReceiveNext();
            CurRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Update()
    {
        if (pv.isMine && !damage.IsDestroy)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
            tr.Rotate(Vector3.up * h * rotSpeed * Time.deltaTime);
        }
        else //다른 네트워크 유저의 탱크라면
        {
            //원격 플레이어의 탱크를 수신받은 위치까지 부드럽게 이동시킴
            tr.position = Vector3.Lerp(tr.position, CurPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, CurRot, Time.deltaTime * 3.0f);
        }
    }
}