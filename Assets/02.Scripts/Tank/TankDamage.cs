using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TankDamage : MonoBehaviour
{           //�ǰݴ����� hp�� �� ���̸� �޽��������� ��Ȱ��ȭ�� ����
    private MeshRenderer[] meshRenderer;
    //���� ����Ʈ
    [SerializeField]
    private GameObject expEffect = null;
    private int initHp = 100;
    private int CurHp = 0;
    private readonly string bulletTag = "T_Bullet";
    private BoxCollider boxcollider;
    private TankMove tankmove;
    public bool IsDestroy = false;
    public Canvas hudCanvas;
    public Image hpBar;
    void Start()
    {
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        CurHp = initHp;
        expEffect = Resources.Load<GameObject>("Effects/SmallExplosionEffect");
        boxcollider = GetComponentInChildren<BoxCollider>();
        tankmove = GetComponent<TankMove>();
        hpBar.color = Color.green;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(CurHp > 0 && other.tag == bulletTag)
        {
            CurHp -= 20;
            CurHp = Mathf.Clamp(CurHp, 0, 100);
            hpBar.fillAmount = (float)CurHp / (float)initHp;
            if (hpBar.fillAmount <= 0.3f)
                hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.5f)
                hpBar.color = Color.yellow;

            if(CurHp <= 0)
            {       //��ŸƮ �ڷ�ƾ : ������ �ڽŸ��� �������� ���� ���鶧 ������
                StartCoroutine(ExplosionTank());
            }
        }
    }
    IEnumerator ExplosionTank()
    {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3.0f);
        boxcollider.enabled = false;

        SetTankVisible(false);
        IsDestroy = true;
        //HUD�� ��Ȱ��ȭ
        hudCanvas.enabled = false;

        yield return new WaitForSeconds(5.0f);

        CurHp = initHp;
        hpBar.fillAmount = 1.0f;
        hpBar.color = Color.green;
        hudCanvas.enabled = true;
        SetTankVisible(true);
        boxcollider.enabled = true;
        IsDestroy = false;
    }
    void SetTankVisible(bool isVisible)
    {
        foreach (MeshRenderer _renderer in meshRenderer)
        {
            _renderer.enabled = isVisible;
        }
    }
}
