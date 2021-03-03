using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaSelectControl : MonoBehaviour
{
    RectTransform rect;
    Image image;
    public Vector2 firstPos;
    public Vector2 endPos;
   

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    void Update()
    {
        // �h���b�O���̎n�_�ƏI�_�ŋ�`���쐬
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            // �\��
            image.enabled = true;
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rect.position = (firstPos + endPos) / 2;

            rect.sizeDelta = new Vector2(Mathf.Abs(firstPos.x - endPos.x), Mathf.Abs(firstPos.y - endPos.y));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // ��\��
            image.enabled = false;
        }
    }

    public Vector2 FirstPos
    {
        get { return this.firstPos; }
    }
    public Vector2 EndPos
    {
        get { return this.endPos; }
    }
}