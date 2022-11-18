using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    TankMove tankmove;
    public FollowCamera followcam;
    private readonly string playerTag = "Player";
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        tankmove = GetComponentInParent<TankMove>();
        followcam = Camera.main.transform.GetComponent<FollowCamera>();
    }

    void Update()
    {
        GetOutTank();
    }

    private void GetOutTank()
    {
        if (tankmove.isGetIn == false) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            followcam.target = player.transform; //ī�޶� Ÿ���� player��
            player.SetActive(true);
            //player.GetComponentInChildren<GameObject>().tag = "MainCamera";
            tankmove.isGetIn = false;
            //player.transform.position = new Vector3(tankmove.transform.position.x + -15f,
            //    tankmove.transform.position.y + 2f, tankmove.transform.position.z);
            player.transform.position = new Vector3(tankmove.transform.position.x + Random.Range(-10f,-15f),
    tankmove.transform.position.y + 2f, tankmove.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == playerTag)
        {
            tankmove.isGetIn = true;
            //ī�޶� Ÿ���� �ڱ� �ڽ�����      
            player.SetActive(false);
            player.GetComponentInChildren<AudioListener>().enabled = false;
            //player.transform.GetChild(0).tag = "Untagged";
            //followcam.tag = "MainCamera";
            followcam.GetComponent<AudioListener>().enabled = true;
            followcam.target = tankmove.transform;
            followcam.distance = 20f;
            followcam.height = 15f;
        }
    }
}