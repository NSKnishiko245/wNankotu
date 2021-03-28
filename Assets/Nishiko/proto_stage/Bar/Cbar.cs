using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cbar : MonoBehaviour
{
    // �ڕW�̊p�x
    private float Angle_Destination = 0.0f;
    // �p���x��ڕW�p�̉����ɂ��邩
    private float Angle_Speed = 1.0f;

    //�������Ă��邩�H
    public bool IsHit{ get; private set; }

    // �X�e�[�W�����]�������ǂ���
    public bool ReverseRotateFlg { private get; set; }

    // ��]���
    public enum ROTATESTATE
    {
        NEUTRAL,        // �ʏ���
        ROTATED,        // ��]�ς�
        ROTATE_LEFT,    // ����]���n�߂�
        ROTATE_RIGHT,   // �E��]���n�߂�
        REROTATE,       // ���ɖ߂���]��
    }
    public ROTATESTATE RotateState { get; private set; }

    // ��]���߁i������O������ĂԂ��Ƃŉ�]������j
    public void Rotation(ROTATESTATE state)
    {
        RotateState = state;
        RotationInfo_Update();
    }

    void Start()
    {
        IsHit = false;
        ReverseRotateFlg = false;
        RotateState = ROTATESTATE.NEUTRAL;
        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        if (!(RotateState == ROTATESTATE.NEUTRAL || RotateState == ROTATESTATE.ROTATED))
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
            case ROTATESTATE.ROTATE_LEFT:
                Angle_Destination = -180.0f;
                if (ReverseRotateFlg) Angle_Destination *= -1.0f;
                break;
            case ROTATESTATE.ROTATE_RIGHT:
                Angle_Destination = 180.0f;
                if (ReverseRotateFlg) Angle_Destination *= -1.0f;
                break;
            case ROTATESTATE.REROTATE:
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
                Debug.Log(transform.localRotation.y);
                // ��]�I�����ɉ�]��Ԃ��X�V
                if (RotateState != ROTATESTATE.REROTATE)
                {
                    RotateState = ROTATESTATE.ROTATED;
                }
                else
                {
                    RotateState = ROTATESTATE.NEUTRAL;
                }
            }
        }
    }

    // �v���C���[�ɑ΂���Փˌ��m
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") IsHit = true;
        GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") IsHit = false;
        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }
}
