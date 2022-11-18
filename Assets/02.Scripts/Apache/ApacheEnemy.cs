using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1. 패트롤 포인트 위치
public class ApacheEnemy : MonoBehaviour
{
    public Transform Path;
    public Transform[] pathTransform;
    public List<Transform> Nodes;
    public int CurrentNode = 0; //배열 인덱스 값
    private Transform tr;
    public float rotDamping = 10f;
    public float moveDamping = 10f;
    public bool IsPatrol = true;
    //public Transform TankTr;
    [Header("발사 관련 변수들")]
    public Transform[] firePos;
    public float CurrentDelay = 0f;
    public float MaxDelay = 80f;
    public GameObject bullet;
    public AudioClip RotorSound;
    [SerializeField]
    private PhotonView pv = null;
    [Header("가까운 거리에 있는 플레이어 공격")]
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
    #region 여러대의 탱크 중
    //private void TankShortDist()
    //{
    //    FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName));
    //    //하이라키에 탱크 태그를 읽어와서 리스트 배열에 담는다.
    //    shortDist = Vector3.Distance(this.gameObject.transform.position, FoundObjects[0].transform.position);
    //    //자기자신의 위치와 라스트에 담은 첫번째 배열의 위치를 비교를 한다.
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
    {   //포톤뷰에 아무 유저가 접속중이면
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
    {                                   //벡터값을 받아서 쿼터니언 회전값으로 변경하는 함수
        Quaternion rot = Quaternion.LookRotation(Nodes[CurrentNode].position - tr.position);
        //목적지 - 자기자신위치를 빼면 회전 방향
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
        //타겟위치 - 자기자신위치
        //Vector3 dist = (TankTr.position - tr.position);
        //위 2개랑 같은거임
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
        {           //자기 자신과 배열에 담은 탱크의 전체 거리의 크기
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