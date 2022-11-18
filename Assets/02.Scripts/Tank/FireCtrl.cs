using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    private float fireRate = 0.2f;
    private float nextFire = 0.0f;
    //public AudioSource source;
    public AudioClip cannonSfx;
    TankMove tankmove;
    private PhotonView pv = null;
    private Quaternion CurRot = Quaternion.identity;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        tankmove = GetComponent<TankMove>();
        //source = GetComponent<AudioSource>();
        cannonSfx = Resources.Load<AudioClip>("Sounds/CannonFire");
    }
    void Update()
    {   //�̺�Ʈ ��ŷ
        if (MouseHover.mouseHover.isHover == true) return;

            if (Input.GetMouseButtonDown(0) && Time.time > nextFire && pv.isMine)
            {
                nextFire = Time.time + fireRate; //0.2�ʸ��� �߻�
            //�ڱ� �ڽ��� ��ũ�� ��� �����Լ� ȣ���� ��ź �߻�
                Fire();
            //���� ��Ʈ��ũ �÷��̾��� ��ũ�� RPC�� �������� Fire �Լ��� ȣ��
                pv.RPC("Fire", PhotonTargets.Others, null);
            }
    }
    [PunRPC] //�ٸ� ��Ʈ��ũ ������ �� �Լ��� ���������� ������ �� �ִ�.
    private void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        //source.PlayOneShot(cannonSfx, 1.0f);
        SoundManager.soundmanager.PlaySfx(transform.position, cannonSfx);
    }
}