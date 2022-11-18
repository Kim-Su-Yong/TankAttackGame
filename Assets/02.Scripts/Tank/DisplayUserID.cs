using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUserID : MonoBehaviour
{
    public Text userId;
    private PhotonView pv = null;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        userId.text = pv.owner.NickName;
    }
}
