using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �}�e���A���Ƀe�N�X�`�����Z�b�g����ɂ̓V�F�[�_�[��ύX����K�v������
            string shader = "Legacy Shaders/Diffuse";
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find(shader);

            // �X�N�V�����Ă݂��I
            StartCoroutine(SelectAreaScreenShot(new Vector2Int(0, 0), 100, 100));
        }
    }

    
    // �w�肵���͈͂̃X�N�V�������A���̃X�N���v�g���A�^�b�`���Ă���I�u�W�F�N�g�ɓ\��
    // �����ŃX�N�V���������͈͂̍����̓_�Ɣ͈͂̕���n���i���͈͂̎w��̓X�N���[�����W�Łj
    private IEnumerator SelectAreaScreenShot(Vector2Int leftBottom, int width, int height)
    {
        // �X�N�V���͕`���ɂ����s���Ȃ��̂ŁA�`���Ɏ��s
        yield return new WaitForEndOfFrame();

        // �X�N���[���S�̂��X�N�V�����A���̃f�[�^��ێ�
        Vector2Int size = new Vector2Int(Screen.width, Screen.height);
        Texture2D screeTex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
        screeTex.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
        screeTex.Apply();

        // ��ŎB�����X�N�V���f�[�^����w�肵���͈͂̃s�N�Z�����擾
        Color[] colors = screeTex.GetPixels(leftBottom.x, leftBottom.y, width, height);

        // �擾�����s�N�Z���f�[�^�Ńe�N�X�`�����쐬
        Texture2D tex2 = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex2.SetPixels(colors);
        tex2.Apply();

        // �I�u�W�F�N�g�̃}�e���A���Ƀe�N�X�`�����Z�b�g
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", tex2);
    }
}