using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAni : MonoBehaviour
{
    private float scrollSpeed = 1.0f;
    private MeshRenderer meshRenderer;
    [SerializeField]
    TankMove tankmove;
    void Start()
    {
        tankmove = gameObject.transform.parent.GetComponent<TankMove>();
        meshRenderer = GetComponent<MeshRenderer>();
    }


    void Update()
    {

            //W, SŰ�� ������ �����ϴ� Input.GetAxis("Vertical")���� �� ������ �����ϴ� ��ɾ�
            var offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
            meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            //                                    �ؽ�ó ������ diffuse�� ��� y������ �� ����
            meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
            //                                    ��ָ� y������ �� ����
        
    }
}