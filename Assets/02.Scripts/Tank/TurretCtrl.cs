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
            //������ ����ī�޶󿡼� ���콺 Ŀ�� ��ġ�� �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //��ũ����ǥ-> ���󼼰� ��ǥ��
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
            {                       //ray�� ���� ��ġ�� ������ǥ�� ��ȯ
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //��ź��Ʈ �Լ�(������ �������� ��ǥ x, ������ ���� ���� ��ǥ z) * ����� �Լ��� ���Ѵ�.
                tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, CurRot, Time.deltaTime * 3.0f);
        }
    }
}
