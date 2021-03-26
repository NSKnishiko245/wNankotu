using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cbar : MonoBehaviour
{
    // �ڕW�̊p�x
    private float Angle_Destination = 0.0f;
    // �p���x��ڕW�p�̉����ɂ��邩
    private float Angle_Speed = 0.1f;

    //�������Ă��邩�H
    public bool IsHit{ get; private set; }

    // ��]���
    public enum ROTATESTATE
    {
        NEUTRAL,        // �ʏ���
        ROTATED,        // ��]�ς�
        ROTATE_LEFT,    // ����]���n�߂�
        ROTATE_RIGHT,   // �E��]���n�߂�
        REROTATE,       // ���ɖ߂���]��
    }
    public ROTATESTATE RotateState{ get; private set; }

    // ��]���߁i������O������ĂԂ��Ƃŉ�]������j
    public void Rotation_Left() { RotateState = ROTATESTATE.ROTATE_LEFT; }
    public void Rotation_Right() { RotateState = ROTATESTATE.ROTATE_RIGHT; }
    public void Rotation_Reset() { RotateState = ROTATESTATE.REROTATE; }


    void Start()
    {
        IsHit = false;
        RotateState = ROTATESTATE.NEUTRAL;
        GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        RotationInfo_Update();
        Rotation_Update();
    }

    // ��]��Ԃ̍X�V
    private void RotationInfo_Update()
    {
        // ��]�O�ɖڕW�̊p�x������
        switch (RotateState)
        {
            case ROTATESTATE.ROTATE_LEFT:
                Angle_Destination = 180.0f;
                AngleSpeedJudge();
                break;
            case ROTATESTATE.ROTATE_RIGHT:
                Angle_Destination = -180.0f;
                AngleSpeedJudge();
                break;
            case ROTATESTATE.REROTATE:
                Angle_Destination = 0.0f;
                AngleSpeedJudge();
                break;
        }
    }
    // �p���x�̕�������
    private void AngleSpeedJudge()
    {
        if (transform.localEulerAngles.y > Angle_Destination)
        {
            Angle_Speed *= -1.0f;
        }
    }

    // ��]����
    private void Rotation_Update()
    {
        // ���݂̊p�x���ڕW�̊p�x�ɓ��B���Ă��Ȃ���Ή�]�𑱂���
        if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(Angle_Destination)) > 0.02f)
        {
            // ��]
            transform.Rotate(0.0f, Angle_Speed, 0.0f);

            // �ڕW�̒l�𒴂����ꍇ�͕␳
            if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(Angle_Destination)) < 0.02f)
            {
                transform.eulerAngles = new Vector3(0.0f, Angle_Destination, 0.0f);
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
