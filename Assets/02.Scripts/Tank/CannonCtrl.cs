using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    private Transform tr;
    public float rotSpeed = 300f;
    float CurRotate = 0f;//현재 회전값
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
        if(stream.isWriting) //송신
        {
            stream.SendNext(tr.localRotation); //자신의 회전값
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
            /* 마우스 휠마우스는 업다운시 양수와 음수로 변경된다.
               다른 Axis 예를 들어 Horizontal과 같이 -1에서
               +1까지 리턴하지 않기 때문에
               양수 음수를 판단하여 각도를 제한했습니다. */

            if (wheel <= -0.01f) //포신을 올릴때
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
            else //포신을 내릴때
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