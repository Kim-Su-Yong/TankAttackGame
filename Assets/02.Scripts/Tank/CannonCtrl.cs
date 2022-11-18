using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private Transform tr;
    public float rotSpeed = 300f;
    float CurRotate = 0f;//���� ȸ����
    public float upperAngle = -30f;
    public float downAngle = 30f;
    [SerializeField]
    TankMove tankmove;
    private PhotonView pv = null;
    private Quaternion CurRot = Quaternion.identity;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
        tankmove = gameObject.transform.parent.GetComponentInParent<TankMove>();
        tr = GetComponent<Transform>();
        CurRot = tr.localRotation;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting) //�۽�
        {
            stream.SendNext(tr.localRotation); //�ڽ��� ȸ����
        }
        else if(stream.isReading)
        {
            CurRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Update()
    {
        if (pv.isMine)
        {
            //float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
            //tr.Rotate(angle, 0f, 0f);

            float wheel = -Input.GetAxis("Mouse ScrollWheel");
            float angle = Time.deltaTime * wheel * rotSpeed;
            /* ���콺 �ٸ��콺�� ���ٿ�� ����� ������ ����ȴ�.
               �ٸ� Axis ���� ��� Horizontal�� ���� -1����
               +1���� �������� �ʱ� ������
               ��� ������ �Ǵ��Ͽ� ������ �����߽��ϴ�. */

            if (wheel <= -0.01f) //������ �ø���
            {
                CurRotate += angle;
                if (CurRotate > upperAngle)
                {
                    tr.Rotate(angle, 0f, 0f);
                }
                else
                {
                    CurRotate = upperAngle;
                }
            }
            else //������ ������
            {
                CurRotate += angle;
                if (CurRotate < downAngle)
                {
                    tr.Rotate(angle, 0f, 0f);
                }
                else
                {
                    CurRotate = downAngle;
                }
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, CurRot, Time.deltaTime * 3.0f);
        }
    }
}