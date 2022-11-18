using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    private Transform tr;
    private RaycastHit hit;
    public float rotSpeed = 5.0f;
    [SerializeField]
    TankMove tankmove;
    private PhotonView pv = null;
    private Quaternion CurRot = Quaternion.identity;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
        tankmove = GetComponentInParent<TankMove>();
        tr = GetComponent<Transform>();
        CurRot = tr.localRotation;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else 
        {
            CurRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Update()
    {
        if (pv.isMine)
        {
            //광선을 메인카메라에서 마우스 커서 위치로 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //스크린좌표-> 가상세계 좌표로
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
            {                       //ray가 맞은 위치를 로컬좌표로 변환
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //역탄제트 함수(실제로 맞은로컬 좌표 x, 실제로 맞은 로컬 좌표 z) * 래디언 함수를 곱한다.
                tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, CurRot, Time.deltaTime * 3.0f);
        }
    }
}
