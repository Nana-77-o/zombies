using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �_���[�W�\���N���X
/// </summary>
public class DamageView : MonoBehaviour
{
    /// <summary> �����v�[���� </summary>
    private static readonly int DEFAULT_COUNT = 20;
    /// <summary> �\���ʒu�Ԃꕝ </summary>
    private static readonly float VIEW_DIFFUS = 1f;
    /// <summary> �ʒu������ </summary>
    private static readonly Vector3 VIEW_DIR = new Vector3(1, 1, 0);
    /// <summary> �v�[����ێ�����I�u�W�F�N�g </summary>
    private static GameObject viewBase = null;
    /// <summary> �_���[�W�\��Prefab </summary>
    private static DamageView damageViewPrefab = null;
    /// <summary> �v�[�����X�g </summary>
    private static List<DamageView> damageViewList = new List<DamageView>();
    [SerializeField,Tooltip("�\���p�e�L�X�g")]
    private Text _viewText = default;
    [SerializeField,Header("�\������")]
    private float _lifeTime = 1f;
    /// <summary> �^�C�}�[ </summary>
    private float _timer = 0;
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0) //�\�����Ԍo�߂Ŕ�\����
        {
            _timer = 0;
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// ���W���w�肵�A�_���[�W�\�����s��
    /// </summary>
    /// <param name="pos">/param>
    /// <param name="damage"></param>
    public static void Show(Vector3 pos, float damage)
    {
        if (damageViewPrefab == null) //�����Prefab�����[�h���v�[���𐶐�����
        {
            damageViewPrefab = Resources.Load<DamageView>("Prefab/DamageView");
            if (damageViewPrefab == null)
            {
                Debug.Log("null! Resources/Prefab/DamageView");
                return;
            }
            viewBase = new GameObject("DamageViewBase");
            for (int i = 0; i < DEFAULT_COUNT; i++) //�����v�[������
            {
                var newView = Instantiate(damageViewPrefab, viewBase.transform);
                newView.gameObject.SetActive(false);
                damageViewList.Add(newView);
            }
            DontDestroyOnLoad(viewBase);
        }
        //Vector3.Diffusivity�͊g�����\�b�h�A�����_���ɍ��W�����炷�B�_���[�W�\���̏d�Ȃ��}���邽��
        GetView(pos.Diffusivity(VIEW_DIR, VIEW_DIFFUS) + Vector3.up + Vector3.back).View(damage);
    }
    /// <summary>
    /// �v�[������DamageView�����o��
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static DamageView GetView(Vector3 pos)
    {
        foreach (DamageView view in damageViewList)
        {
            if (view.gameObject.activeInHierarchy == true)
            {
                continue;
            }
            view.transform.position = pos;
            return view;
        }
        //�s�����͒ǉ�����
        var newView = Instantiate(damageViewPrefab, viewBase.transform);
        newView.transform.position = pos;
        damageViewList.Add(newView);
        return newView;
    }
    /// <summary>
    /// �_���[�W�\��
    /// </summary>
    /// <param name="damage"></param>
    public void View(float damage)
    {
        _viewText.text = damage.ToString("F0");
        _timer = _lifeTime;
        gameObject.SetActive(true);
    }
}
