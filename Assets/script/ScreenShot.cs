using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    private Vector3 axispoint;
    private bool rotFlg = false;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(1.0f, 0.5f);
        transform.position = new Vector3(Screen.width, Screen.height * 0.5f, 0.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TurnOnScreenShotEx());
        }

        // �X�N�V���B�e��A��]���n�߂�
        if (rotFlg)
        {
            transform.Rotate(new Vector3(0.0f, -0.05f));
        }
    }

    // �X�N�V�����A���̉摜����ʂɓ\��
    private IEnumerator TurnOnScreenShotEx()
    {
        yield return new WaitForEndOfFrame();

        Vector2Int size = new Vector2Int(Screen.width, Screen.height);
        Texture2D tex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        // �����Ŏw�肵����`�����X�N�V�����A���̃f�[�^�����g(tex)�Ɋi�[
        tex.ReadPixels(new Rect(0.0f, 0.0f, size.x, size.y), 0, 0);
        tex.Apply();

        gameObject.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        gameObject.GetComponent<Image>().color = Color.white;
        rotFlg = true;
    }
}