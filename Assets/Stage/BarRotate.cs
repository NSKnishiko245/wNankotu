using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarRotate : MonoBehaviour
{
    // �ڕW�̊p�x
    private float Angle_Destination = 0.0f;
    // �p���x��ڕW�p�̉����ɂ��邩
    private float Angle_Speed = 2.0f;

    //�������Ă��邩�H
    public bool IsHit{ get; private set; }

    // �X�e�[�W�����]�������ǂ���
    public bool ReverseRotateFlg { private get; set; }

    // ��]���
    public enum ROTSTATEINNERDATA
    {
        NEUTRAL,        // �ʏ���
        ROTATED,        // ��]�ς�
        ROTATE_LEFT,    // ����]���n�߂�
        ROTATE_RIGHT,   // �E��]���n�߂�
        REROTATE,       // ���ɖ߂���]��
    }
    public enum ROTSTATEOUTERDATA
    {
        ROTATE_LEFT,    // ����]���n�߂�
        ROTATE_RIGHT,   // �E��]���n�߂�
        REROTATE,       // ���ɖ߂���]��
    }
    public ROTSTATEINNERDATA RotateState { get; private set; }

    // ��]���߁i������O������ĂԂ��Ƃŉ�]������j
    public void Rotation(ROTSTATEOUTERDATA state)
    {
        switch (state)
        {
            case ROTSTATEOUTERDATA.ROTATE_LEFT:
                RotateState = ROTSTATEINNERDATA.ROTATE_LEFT;
                break;
            case ROTSTATEOUTERDATA.ROTATE_RIGHT:
                RotateState = ROTSTATEINNERDATA.ROTATE_RIGHT;
                break;
            case ROTSTATEOUTERDATA.REROTATE:
                RotateState = ROTSTATEINNERDATA.REROTATE;
                break;
        }
        RotationInfo_Update();
    }

    void Start()
    {
        IsHit = false;
        ReverseRotateFlg = false;
        RotateState = ROTSTATEINNERDATA.NEUTRAL;
        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        if (!(RotateState == ROTSTATEINNERDATA.NEUTRAL || RotateState == ROTSTATEINNERDATA.ROTATED))
        {
            // ��]����
            Rotation_Update();
        }
    }

    // ��]��Ԃ̍X�V
    private void RotationInfo_Update()
    {
        // ��]�O�ɖڕW�̊p�x������
        switch (RotateState)
        {
            case ROTSTATEINNERDATA.ROTATE_LEFT:
                Angle_Destination = -180.0f;
                if (ReverseRotateFlg) Angle_Destination *= -1.0f;
                break;
            case ROTSTATEINNERDATA.ROTATE_RIGHT:
                Angle_Destination = 180.0f;
                if (ReverseRotateFlg) Angle_Destination *= -1.0f;
                break;
            case ROTSTATEINNERDATA.REROTATE:
                Angle_Destination = 0.0f;
                break;
        }
        AngleSpeedJudge();
    }
    // �p���x�̕�������
    private void AngleSpeedJudge()
    {
        Angle_Speed = Mathf.Abs(Angle_Speed);
        if (transform.localRotation.ToEuler().y > Angle_Destination)
        {
            Angle_Speed *= -1.0f;
        }
    }

    // ��]����
    private void Rotation_Update()
    {
        // ���݂̊p�x���ڕW�̊p�x�ɓ��B���Ă��Ȃ���Ή�]�𑱂���
        if (Mathf.Abs(Mathf.Abs(transform.localEulerAngles.y) - Mathf.Abs(Angle_Destination)) > 0.02f)
        {
            // ��]
            transform.Rotate(0.0f, Angle_Speed, 0.0f);

            // �ڕW�̒l�𒴂����ꍇ�͕␳
            if (Mathf.Abs(Mathf.Abs(transform.localEulerAngles.y) - Mathf.Abs(Angle_Destination)) < 0.02f)
            {
                transform.localRotation = Quaternion.Euler(0.0f, -Angle_Destination, 0.0f);
                // ��]�I�����ɉ�]��Ԃ��X�V
                if (RotateState != ROTSTATEINNERDATA.REROTATE)
                {
                    RotateState = ROTSTATEINNERDATA.ROTATED;
                }
                else
                {
                    RotateState = ROTSTATEINNERDATA.NEUTRAL;
                }
            }
        }
    }

    // �v���C���[�ɑ΂���Փˌ��m
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsHit = true;
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IsHit = false;
            GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
