using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    void Start()
    {
       // gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �}�e���A���Ƀe�N�X�`�����Z�b�g����ɂ̓V�F�[�_�[��ύX����K�v������
            string shader = "Legacy Shaders/Diffuse";
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find(shader);

            StartCoroutine(TurnOnScreenShotEx());
        }
    }

    // �X�N�V�����A���̉摜����ʂɓ\��
    private IEnumerator TurnOnScreenShotEx()
    {
        yield return new WaitForEndOfFrame();

        Vector2Int size = new Vector2Int(Screen.width, Screen.height);
        Texture2D tex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        // �����Ŏw�肵����`�����X�N�V�����A���̃f�[�^�� tex �Ɋi�[
        tex.ReadPixels(new Rect(0.0f, 0.0f, size.x, size.y), 0, 0);
        tex.Apply();

        // quad�I�u�W�F�N�g�̃}�e���A���ɃX�N�V�������摜���Z�b�g
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
    }
}