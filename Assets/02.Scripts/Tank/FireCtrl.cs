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
    {   //이벤트 훅킹
        if (MouseHover.mouseHover.isHover == true) return;

            if (Input.GetMouseButtonDown(0) && Time.time > nextFire && pv.isMine)
            {
                nextFire = Time.time + fireRate; //0.2초마다 발사
            //자기 자신의 탱크일 경우 로컬함수 호출해 포탄 발사
                Fire();
            //원격 네트워크 플레이어의 탱크에 RPC로 원격으로 Fire 함수를 호출
                pv.RPC("Fire", PhotonTargets.Others, null);
            }
    }
    [PunRPC] //다른 네트워크 유저가 이 함수를 원격지에서 공유할 수 있다.
    private void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        //source.PlayOneShot(cannonSfx, 1.0f);
        SoundManager.soundmanager.PlaySfx(transform.position, cannonSfx);
    }
}