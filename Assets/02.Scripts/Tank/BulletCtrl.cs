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
        StartCoroutine(ExplosionCannon(3.0f)); //3���� ������ �����
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ExplosionCannon(0.0f));
    }
    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        _collider.enabled = false; //�ݶ��̴� ��Ȱ��ȭ
        rbody.isKinematic = true; //���������� ������ ���� �ʿ䰡 ����.

        GameObject obj = Instantiate(ExplosionEffect, tr.position, Quaternion.identity);
        Destroy(obj, 1.0f);
        source.PlayOneShot(expSfx, 1.0f);
        GetComponent<TrailRenderer>().Clear(); //Ʈ���Ϸ����� �����ϰ�
        Destroy(this.gameObject, 1.0f);
        //Trail�������� �Ҹ�Ǳ���� ��� ����� ����
    }
}