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

            //W, S키를 누를때 반응하는 Input.GetAxis("Vertical")보다 더 빠르게 반응하는 명령어
            var offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
            meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            //                                    텍스처 종류가 diffuse인 경우 y오프셋 값 변경
            meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
            //                                    노멀맵 y오프셋 값 변경
        
    }
}