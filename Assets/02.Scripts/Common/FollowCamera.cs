using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float height = 5f;
    public float distance = 7f;
    public float targetOffset = 1.0f;
    public float moveDamping = 15f;
    public float rotDamping = 20f;
    private Transform tr;
    [SerializeField]
    private AudioClip BGM;
    void Start()
    {
        BGM = Resources.Load<AudioClip>("Sounds/BGM");
        tr = GetComponent<Transform>();
        SoundManager.soundmanager.PlaySfx(tr.position, BGM, true);
    }
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);
        tr.position = Vector3.Lerp(tr.position, camPos, Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Lerp(tr.rotation, target.rotation, Time.deltaTime * rotDamping);
        tr.LookAt(target.position + (target.up * targetOffset));
    }
    //private void OnDrawGizmos() //좌표에 선이나 색상을 그려주는 함수
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.3f);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    //    Gizmos.DrawWireSphere(transform.position, 1f);
    //}
}
