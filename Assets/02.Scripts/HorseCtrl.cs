using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCtrl : MonoBehaviour
{
    public Transform Path;
    public Transform[] pathTransform;
    public List<Transform> Nodes;
    public int CurrentNode = 0;
    private Transform tr;
    public float rotDamping = 10f;
    public float moveDamping = 10f;
    public bool IsPatrol = true;
    void Start()
    {
        tr = this.transform;
        Path = GameObject.Find("PatrolPoints").transform;
        pathTransform = Path.GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != Path.transform)
                Nodes.Add(pathTransform[i]);
        }
    }
    void Update()
    {
        if (IsPatrol)
        {
            PatrolMove();
            CheckWayPointDistance();
        }
    }
    void PatrolMove()
    {                                 
        Quaternion rot = Quaternion.LookRotation(Nodes[CurrentNode].position - tr.position);             
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * rotDamping);
        tr.Translate(Vector3.forward * Time.deltaTime * moveDamping);
    }
    void CheckWayPointDistance()
    {
        if (Vector3.Distance(tr.position, Nodes[CurrentNode].position) <= 2.5f)
        {
            if (CurrentNode == Nodes.Count - 1)
            {
                CurrentNode = 0;
            }
            else
            {
                CurrentNode++;
            }
        }
    }
}
