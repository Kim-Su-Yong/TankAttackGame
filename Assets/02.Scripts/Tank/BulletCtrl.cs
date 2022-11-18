using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public Rigidbody rbody;
    public CapsuleCollider _collider;
    public float Speed = 150f;
    private Transform tr;
    public GameObject ExplosionEffect;
    public AudioSource source;
    public AudioClip expSfx;
    void Start()
    {
        source = GetComponent<AudioSource>();
        _collider = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
        ExplosionEffect = Resources.Load<GameObject>("Effects/SmallExplosionEffect");
        expSfx = Resources.Load<AudioClip>("Sounds/missile_explosion");
        rbody.AddForce(tr.forward * Speed, ForceMode.Impulse);
        StartCoroutine(ExplosionCannon(3.0f)); //3초후 폭파후 사라짐
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ExplosionCannon(0.0f));
    }
    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        _collider.enabled = false; //콜라이더 비활성화
        rbody.isKinematic = true; //물리엔진의 영향을 받을 필요가 없다.

        GameObject obj = Instantiate(ExplosionEffect, tr.position, Quaternion.identity);
        Destroy(obj, 1.0f);
        source.PlayOneShot(expSfx, 1.0f);
        GetComponent<TrailRenderer>().Clear(); //트레일랜더러 투명하게
        Destroy(this.gameObject, 1.0f);
        //Trail렌더러가 소멸되기까지 잠시 대기후 삭제
    }
}