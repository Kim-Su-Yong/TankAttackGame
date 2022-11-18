using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1. ��Ʈ�� ����Ʈ ��ġ
public class ApacheEnemy : MonoBehaviour
{
    public Transform Path;
    public Transform[] pathTransform;
    public List<Transform> Nodes;
    public int CurrentNode = 0; //�迭 �ε��� ��
    private Transform tr;
    public float rotDamping = 10f;
    public float moveDamping = 10f;
    public bool IsPatrol = true;
    //public Transform TankTr;
    [Header("�߻� ���� ������")]
    public Transform[] firePos;
    public float CurrentDelay = 0f;
    public float MaxDelay = 80f;
    public GameObject bullet;
    public AudioClip RotorSound;
    [SerializeField]
    private PhotonView pv = null;
    [Header("����� �Ÿ��� �ִ� �÷��̾� ����")]
    public List<GameObject> FoundObjects;
    public string tagName = "Tank";
    public float shortDist;
    public GameObject[] playerTanks;
    public Transform target;
    void Awake()
    {
        pv = PhotonView.Get(this);
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        RotorSound = Resources.Load<AudioClip>("Sounds/RotorSound");
        tr = this.transform;
        //TankTr = GameObject.FindGameObjectWithTag("Tank").transform;
        Path = GameObject.Find("PatrolPoints").transform;
        pathTransform = Path.GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != Path.transform)
                Nodes.Add(pathTransform[i]);
        }
        //SoundManager.soundmanager.PlaySfx(tr.position, RotorSound, true);
        //TankShortDist();
    }
    #region �������� ��ũ ��
    //private void TankShortDist()
    //{
    //    FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName));
    //    //���̶�Ű�� ��ũ �±׸� �о�ͼ� ����Ʈ �迭�� ��´�.
    //    shortDist = Vector3.Distance(this.gameObject.transform.position, FoundObjects[0].transform.position);
    //    //�ڱ��ڽ��� ��ġ�� ��Ʈ�� ���� ù��° �迭�� ��ġ�� �񱳸� �Ѵ�.
    //    //TankTr.position = FoundObjects[0].transform.position;
    //    foreach(GameObject found in FoundObjects)
    //    {       
    //        float Distance = Vector3.Distance(this.gameObject.transform.position, found.transform.position);
    //        if(Distance < shortDist)
    //        {
    //            //TankTr.position = found.transform.position;
    //            shortDist = Distance;
    //        }
    //        //Debug.Log(TankTr.name);
    //    }
    //}
    #endregion
    void Update()
    {   //����信 �ƹ� ������ �������̸�
        if (PhotonNetwork.connected)
        {
            if (IsPatrol)
            {
                PatrolMove();
                CheckWayPointDistance();
            }
            else
            {
                Attack();
            }
        }
    }
    void PatrolMove()
    {                                   //���Ͱ��� �޾Ƽ� ���ʹϾ� ȸ�������� �����ϴ� �Լ�
        Quaternion rot = Quaternion.LookRotation(Nodes[CurrentNode].position - tr.position);
        //������ - �ڱ��ڽ���ġ�� ���� ȸ�� ����
                                    //from           to      time
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * rotDamping);
        tr.Translate(Vector3.forward * Time.deltaTime * moveDamping);
        Search();
    }
    void CheckWayPointDistance()
    {
        if(Vector3.Distance(tr.position, Nodes[CurrentNode].position) <= 2.5f)
        {
            if(CurrentNode == Nodes.Count - 1)
            {
                CurrentNode = 0;
            }
            else
            {
                CurrentNode++;
            }
        }
    }
    void Search()
    {
        playerTanks = GameObject.FindGameObjectsWithTag(tagName);
        target = playerTanks[0].transform;
        float dist = Vector3.Distance(tr.position, target.position);
        //float dist = (TankTr.position - tr.position).magnitude;
        //Ÿ����ġ - �ڱ��ڽ���ġ
        //Vector3 dist = (TankTr.position - tr.position);
        //�� 2���� ��������
        if(dist <= 75.0f)
        {
            IsPatrol = false;
        }
    }
    void Attack()
    {
        playerTanks = GameObject.FindGameObjectsWithTag(tagName);
        target = playerTanks[0].transform;
        float Dist = Vector3.Distance(tr.position, target.position);
        float dist2D;
        foreach (GameObject tank in playerTanks)
        {           //�ڱ� �ڽŰ� �迭�� ���� ��ũ�� ��ü �Ÿ��� ũ��
            dist2D = (tank.transform.position - tr.position).sqrMagnitude;
            if(dist2D < Dist)
            {
                target = tank.transform;
            }
        }
        Vector3 targetDist = target.position - tr.position;
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(targetDist), Time.deltaTime * rotDamping);
        
        if(targetDist.magnitude > 75f)
        {
            IsPatrol = true;
        }
        CurrentDelay -= 0.1f;
        if(CurrentDelay <= 0f)
        {
            Fire();
            CurrentDelay = MaxDelay;          
        }
    }
    void Fire()
    {
        Instantiate(bullet, firePos[0].position, firePos[0].rotation);
        Instantiate(bullet, firePos[1].position, firePos[1].rotation);
    }
}