using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayBlock : MonoBehaviour
{
    [Header("Center")]
    public GameObject m_centerobj;

    [Header("�������肷��Obj")]
    public GameObject m_Collobj;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.y != 0)
        {
            m_centerobj.GetComponent<OneWayCenter>().m_Direction = OneWayCenter.DIRECTION.LEFT;
        }
        else
        {
            m_centerobj.GetComponent<OneWayCenter>().m_Direction = OneWayCenter.DIRECTION.RIGHT;
        }

        if (m_centerobj.GetComponent<OneWayCenter>().GetIsPlayerHit())
        {
            m_Collobj.SetActive(false);
        }
        else
        {
            m_Collobj.SetActive(true);
        }
    }


}
