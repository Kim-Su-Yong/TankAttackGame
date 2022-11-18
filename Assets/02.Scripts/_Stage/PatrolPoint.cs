using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Color linecolor;
    public List<Transform> Nodes;
    public float _radius = 3f;
    private void OnDrawGizmos() //좌표에 색상과 선을 그려주는 함수 콜백 메서드
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();
        foreach(Transform tr in transforms)
        {
            if(tr != this.transform)
                Nodes.Add(tr);
        }
        for(int i = 0; i < Nodes.Count; i++)
        {
            Vector3 CurrentNode = Nodes[i].position;
            Vector3 PrevNode = Vector3.zero;
            if (i > 0)
                PrevNode = Nodes[i - 1].position;
            else if (i == 0 && Nodes.Count > 1)
                PrevNode = Nodes[Nodes.Count - 1].position;
            Gizmos.DrawSphere(transform.position, _radius);
            Gizmos.DrawLine(PrevNode, CurrentNode);
        }
    }
}
