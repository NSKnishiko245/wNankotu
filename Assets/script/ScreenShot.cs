using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    private Texture DefaultTexture;
    private Vector2 DefaultScale;

    void Start()
    {
        // ������Texture��ێ����Ă����i�摜���Z�b�g�p�j
        DefaultTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        DefaultTexture = GetComponent<Renderer>().material.GetTexture("_MainTex");

        // �e�q�֌W���`�������ƃX�P�[�����ς��̂ŁA���߂Ɏ擾���Ă���
        DefaultScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g�̃T�C�Y�ŃX�N�V�����B��
            RectTransform trans = gameObject.GetComponent<RectTransform>();
            Vector2 pos = new Vector2(trans.position.x, trans.position.y);

            // quad�̍����̓_�ƉE��̓_���擾
            Vector2 leftBottom = pos - DefaultScale / 2.0f;
            Vector2 rightTop = pos + DefaultScale / 2.0f;
            // ���������Ă��鎞�͕␳
            if (gameObject.transform.localRotation.y != 0.0f)
            {
                leftBottom.x -= DefaultScale.x;
                rightTop.x += DefaultScale.x;
            }
            leftBottom = RectTransformUtility.WorldToScreenPoint(Camera.main, leftBottom);
            rightTop = RectTransformUtility.WorldToScreenPoint(Camera.main, rightTop);

            // �X�N�V�����Ă݂��I
            StartCoroutine(SelectAreaScreenShot(new Vector2Int((int)leftBottom.x, (int)leftBottom.y), (int)(rightTop.x - leftBottom.x), (int)(rightTop.y - leftBottom.y)));
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

    // �����e�N�X�`���Ƀ��Z�b�g
    public void ResetTexture()
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", DefaultTexture);
    }
}