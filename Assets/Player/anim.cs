using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim : MonoBehaviour
{
    //�A�j���[�V����
    private Animator m_anim;
    public GameObject m_playobj;

    //�A�j���[�V�����Ŏg��SE
    public AudioClip m_moveAudio;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //������ς���
        if (m_playobj.GetComponent<Player>().GetInputValue() > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }else if(m_playobj.GetComponent<Player>().GetInputValue() < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }




        if (m_playobj.GetComponent<Player>().IsMove)
        {
            m_anim.SetBool("move_flg", true);
        }
        else
        {
            m_anim.SetBool("move_flg", false);
        }
        if (m_playobj.GetComponent<Player>().IsHitGoalBlock)
        {
            m_anim.SetBool("goal_flg", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void PlayMoveSound()
    {
        AudioSource.PlayClipAtPoint(m_moveAudio, this.transform.position);//SE�Đ�
    }
}
