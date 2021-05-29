using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndRoll : MonoBehaviour
{
    //�@�e�L�X�g�̃X�N���[���X�s�[�h
    [SerializeField]
    private float textScrollSpeed = 2;
    //�@�e�L�X�g�̐����ʒu
    [SerializeField]
    private float limitPosition = 60f;
    //�@�G���h���[�����I���������ǂ���
    private bool isStopEndRoll;
    //�@�V�[���ړ��p�R���[�`��
    private Coroutine endRollCoroutine;

    private bool speedUpFlg = false;

    // Update is called once per frame
    void Update()
    {
        //�@�G���h���[�����I��������
        if (isStopEndRoll)
        {
            endRollCoroutine = StartCoroutine(GoToNextScene());
        }
        else
        {
            //�@�G���h���[���p�e�L�X�g�����~�b�g���z����܂œ�����
            if (transform.position.y <= limitPosition)
            {
                transform.position = new Vector3(0, transform.position.y + textScrollSpeed * Time.deltaTime, -4);
            }
            else
            {
                isStopEndRoll = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            speedUpFlg = true;
        }

        if (speedUpFlg) textScrollSpeed += 0.3f;
    }

    IEnumerator GoToNextScene()
    {
        //�@5�b�ԑ҂�
        //yield return new WaitForSeconds(5f);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
        {
            StopCoroutine(endRollCoroutine);
            SceneManager.LoadScene("BonusScene");
        }

        yield return null;
    }
}
