using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���^�C�v
/// </summary>
internal enum VibrateType
{
    VERTICAL,
    HORIZONTAL
}

/// <summary>
/// �ΏۃI�u�W�F�N�g�̐U�����Ǘ�����N���X
/// </summary>
public class VibrateController : MonoBehaviour
{

    [SerializeField] private VibrateType vibrateType;          //�U���^�C�v
    [Range(0, 100)] [SerializeField] private float vibrateRange; //�U����
    [SerializeField] private float vibrateSpeed;               //�U�����x

    private float initPosition;   //�����|�W�V����
    private float testPosition;   //�e�X�g�p
    private float newPosition;    //�V�K�|�W�V����
    private float minPosition;    //�|�W�V�����̉���
    private float maxPosition;    //�|�W�V�����̏��
    private bool directionToggle; //�U�������̐؂�ւ��p�g�O��(�I�t�F�l���������Ȃ������ �I���F�l���傫���Ȃ������)

    public float vibrateTime { get; private set; }

    public static int startFlg = 0;        //Start()�����̂݌Ăяo������
                                           // Use this for initialization
                                           /*
                                            * �X�e�[�W���]���ɃS�[���𐶐���������Start()���Ă΂�o�O��
                                            */
    void Start()
    {


        ResetPosition();

        //�����|�W�V�����̐ݒ��U���^�C�v���ɕ�����
        switch (this.vibrateType)
        {
            case VibrateType.VERTICAL:
                this.initPosition = transform.localPosition.y;
                testPosition = transform.localPosition.y;
                break;
            case VibrateType.HORIZONTAL:
                this.initPosition = transform.localPosition.x;
                testPosition = transform.localPosition.x;
                break;
        }

        this.newPosition = this.initPosition;
        this.minPosition = this.initPosition - this.vibrateRange;
        this.maxPosition = this.initPosition + this.vibrateRange;
        this.directionToggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        vibrateTime--;
        if (vibrateTime > 0)
        {
            //���t���[���U�����s��
            Vibrate();
        }
        else
        {
            switch (this.vibrateType)
            {
                case VibrateType.VERTICAL:
                    this.transform.localPosition = new Vector3(0, testPosition, 0);
                    break;
                case VibrateType.HORIZONTAL:
                    this.transform.localPosition = new Vector3(testPosition, 0, 0);
                    break;
            }
        }
        // Debug.Log("vibrate");
    }

    private void Vibrate()
    {

        //�|�W�V�������U�����͈̔͂𒴂����ꍇ�A�U��������؂�ւ���
        if (this.newPosition <= this.minPosition ||
            this.maxPosition <= this.newPosition)
        {
            this.directionToggle = !this.directionToggle;
        }

        //�V�K�|�W�V������ݒ�
        this.newPosition = this.directionToggle ?
            this.newPosition + (vibrateSpeed * Time.deltaTime) :
            this.newPosition - (vibrateSpeed * Time.deltaTime);
        this.newPosition = Mathf.Clamp(this.newPosition, this.minPosition, this.maxPosition);

        //�V�K�|�W�V��������
        switch (this.vibrateType)
        {
            case VibrateType.VERTICAL:
                this.transform.localPosition = new Vector3(0, this.newPosition, 0);
                break;
            case VibrateType.HORIZONTAL:
                this.transform.localPosition = new Vector3(this.newPosition, 0, 0);
                break;
        }
    }

    public void SetVibrateTime(float _time)
    {
        vibrateTime = _time;
    }
    public void SetVibrateRange(float _range)
    {
        vibrateRange = _range;
    }
    public void SetSpeed(float _speed)
    {
        vibrateSpeed = _speed;
    }
    /*
     * �܂邽�тɏ����ʒu�����Z�b�g 
     */
    public void ResetPosition()
    {
        if (startFlg < 2)
        {
            return;
        }
        Debug.Log("VibrateController::Start()");
        startFlg++;
        //�����|�W�V�����̐ݒ��U���^�C�v���ɕ�����
        switch (this.vibrateType)
        {
            case VibrateType.VERTICAL:
                this.initPosition = transform.localPosition.y;
                break;
            case VibrateType.HORIZONTAL:
                this.initPosition = transform.localPosition.x;
                break;
        }

        this.newPosition = this.initPosition;
        this.minPosition = this.initPosition - this.vibrateRange;
        this.maxPosition = this.initPosition + this.vibrateRange;
        this.directionToggle = false;
    }
}