using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�յ� ���� : W,S
//ȸ�� : A,D
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
        //��������� UDP���ٰ� �߿��� �������� ��쿡�� TCP/IP ������� �ٲ�� �ִ�.
        pv.ObservedComponents[0] = this;
        //�ڱ��ڽ� Ŭ���� �� TankMove�� ������
        if (pv.isMine) //����䰡 ��Ʈ��ũ�� ���� ���̶��
        {
            Camera.main.GetComponent<FollowCamera>().target = tr;
        }    
        //��ũ�� �ڵ����� ���� �߽��� ���� ���� �̷��� �ؾ� �Ѿ����� �ʴ´�.
        rbody.centerOfMass = new Vector3(0f, -0.5f, 0f);
        CurPos = tr.position;
        CurRot = tr.rotation;
    }
    //�������� �ۼ����ϴ� �Լ�
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) //�۽�
        {
            stream.SendNext(tr.position); //�ڽ��� ��ġ��
            stream.SendNext(tr.rotation); //�ڽ��� ȸ����
        }
        if(stream.isReading) //�ٸ� ��Ʈ��ũ ���� �������� ����
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
        else //�ٸ� ��Ʈ��ũ ������ ��ũ���
        {
            //���� �÷��̾��� ��ũ�� ���Ź��� ��ġ���� �ε巴�� �̵���Ŵ
            tr.position = Vector3.Lerp(tr.position, CurPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, CurRot, Time.deltaTime * 3.0f);
        }
    }
}