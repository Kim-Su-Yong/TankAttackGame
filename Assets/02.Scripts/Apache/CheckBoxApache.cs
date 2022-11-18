using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoxApache : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    ApacheCtrl apache;
    public FollowCamera followcam;
    private readonly string playerTag = "Player";
    private readonly string groundtag = "ground";
    [SerializeField]
    public bool isGround;
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        followcam = Camera.main.transform.GetComponent<FollowCamera>();
        apache = GetComponentInParent<ApacheCtrl>();
    }

    void Update()
    {
        GetOutApache();
    }
    private void GetOutApache()
    {
        if (apache.isGetInapa == false) return;
        if (Input.GetKeyDown(KeyCode.E) && isGround)
        {
            followcam.target = player.transform; //카메라 타겟을 player로
            player.SetActive(true);
            //player.GetComponentInChildren<GameObject>().tag = "MainCamera";
            apache.isGetInapa = false;
            //player.transform.position = new Vector3(tankmove.transform.position.x + -15f,
            //    tankmove.transform.position.y + 2f, tankmove.transform.position.z);
            player.transform.position = new Vector3(apache.transform.position.x + Random.Range(-10f, -15f),
    apache.transform.position.y + 2f, apache.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            apache.isGetInapa = true;
            //카메라 타겟을 자기 자신으로      
            player.SetActive(false);
            player.GetComponentInChildren<AudioListener>().enabled = false;
            //player.transform.GetChild(0).tag = "Untagged";
            //followcam.tag = "MainCamera";
            followcam.GetComponent<AudioListener>().enabled = true;
            followcam.target = apache.transform;
            followcam.distance = 20f;
            followcam.height = 15f;
        }
        if (other.tag == groundtag)
        {
            isGround = true;
        }      
    }
    private void OnTriggerExit(Collider other)
    {
        isGround = false;
    }
}