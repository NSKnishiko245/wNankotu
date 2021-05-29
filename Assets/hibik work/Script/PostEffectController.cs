using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PostEffectController : MonoBehaviour
{
    [SerializeField] private GameObject postProcess;    // �|�X�g�v���Z�X
    [SerializeField] private GameObject fire;   // ���E�\�N�̉�

    private Animator fireAnim;  // ���E�\�N�̉΂̃A�j���[�V����
    private Volume volume;      // �u���[��
    private Vignette vignette;  // �r�l�b�g

    public static float intensity;  // �r�l�b�g�̋��x
    [SerializeField] private float intensityMax;    // �r�l�b�g�̋��x�̍ő�l
    [SerializeField] private float intensityVal;    // �r�l�b�g�̋��x�̑�����
    private bool vigFlg = false;    // true:�r�l�b�gON

    void Awake()
    {
        volume = postProcess.GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
        fireAnim = fire.GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            intensity = vignette.intensity.value;
            vigFlg = true;
        }

        if (SceneManager.GetActiveScene().name == "Stage1Scene")
        {
            intensity = intensityMax;
            vigFlg = true;

            fireAnim.SetBool("isAnim", true);
        }
    }

    void Update()
    {
        // �r�l�b�gON
        if (vigFlg)
        {
            if (intensity < intensityMax) intensity += intensityVal;
        }
        // �r�l�b�gOFF
        else
        {
            if (intensity > 0.0f) intensity -= intensityVal;
        }

        vignette.intensity.value = intensity;
    }

    // �r�l�b�g���Z�b�g
    public void SetVigFlg(bool sts)
    {
        vigFlg = sts;
    }

    // ���E�\�N�̉΂��Z�b�g
    public void SetFireFlg(bool sts)
    {
        fireAnim.SetBool("isAnim", sts);
    }
}
