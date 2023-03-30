using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>�̍�UI�֘A�̏������s���N���X</summary>
public class AchievementManager : MonoBehaviour
{
    /// <summary>���у��X�g�̏����ʒu </summary>
    readonly Vector2 ACHIEVEMENT_LIST_FIRST_POSITION = new Vector2(-670, 30);

    [SerializeField, Header("�X�N���[���̑���")]
    float _scrollSpeed = 0f;
    [SerializeField, Header("�e���т̃X�N���[������"), Tooltip("0.���j�� 1.�~�o�� 2.�K��")]
    float[] _scrollMoveAmounts = default;
    [SerializeField, Header("�e���т̃X�N���[���o�[�̈ړ�����"), Tooltip("0.���j�� 1.�~�o�� 2.�K��")]
    float[] _scrollbarMoveAmounts = default;
    [SerializeField, Header("�e���у��X�g�̖��O")]
    string[] _listNames = default;
    [SerializeField, Tooltip("�\�����Ă���̍�")]
    Image _showImage = default;
    [SerializeField, Tooltip("���і��������̔w�i")]
    Image _hideImage = default;
    [SerializeField, Tooltip("���і��������̃t���[��")]
    Image _hideFarme = default;
    [SerializeField, Tooltip("���і��������̃N�G�X�`�����}�[�N")]
    Text _hideText = default;
    [SerializeField, Tooltip("�̍�����\������e�L�X�g")]
    Text _showAchievementText = default;
    [SerializeField, Tooltip("�ڍׂ�\������e�L�X�g1")]
    Text _showDetail1Text = default;
    [SerializeField, Tooltip("�ڍׂ�\������e�L�X�g2")]
    Text _showDetail2Text = default;
    [SerializeField, Tooltip("���у��X�g�̃��x��")]
    Text _achievementListLabel = default;
    [SerializeField, Tooltip("���у��X�g��ύX����{�^���@0=�� 1=�E")]
    Button[] _changeListButtons = default;
    [SerializeField, Tooltip("�e���тł̃{�^���ړ���i���j")]
    Button[] _leftButtonDestinations = default;
    [SerializeField, Tooltip("�e���тł̃{�^���ړ���i�E�j")]
    Button[] _rightButtonDestinations = default;
    [SerializeField, Tooltip("�e���у��X�g")]
    RectTransform[] _achievementLists = default;
    [SerializeField, Tooltip("���݂̑I������Ă���̍�")]
    AchievementController _selectedAchievement = default;
    [SerializeField, Tooltip("���т��J�������ɑI�������{�^��")]
    AchievementController _firstButton = default;
    [SerializeField, Tooltip("�X�N���[���o�[")]
    Scrollbar _scrollbar = default;
    [SerializeField, Tooltip("�̍��f�[�^ 0.���j�� 1.�~�o�� 2.�K��")]
    AchievementData[] _achievementData = default;
    [SerializeField, Tooltip("���т�\������L�����o�X")]
    Canvas _uiCanvas = default;

    /// <summary>���у��X�g��ύX����{�^���̃e�L�X�g�@0=�O 1= �� </summary>
    Text[] _changeListButtonTexts = new Text[2];
    /// <summary>���ݕ\�����Ă�����у��X�g�̃I�u�W�F�N�g </summary>
    RectTransform _currnetShowAchievementList = default;
    /// <summary>���ݎQ�Ƃ��Ă�����уf�[�^ </summary>
    AchievementData _currentAchievementData = default;

    /// <summary>���ݕ\�����Ă�����у��X�g�̎�� </summary>
    AchievementType _currentType = AchievementType.Kill;
    /// <summary>�e���у��X�g�ɍ��킹���X�N���[�������ɂ��邽�߂̕ϐ� </summary>
    float _currentMoveAmount = 0f;
    /// <summary>���݂̃X�N���[���o�[�ړ����� </summary>
    float _currentScrollberMoveAmount = 0f;
    /// <summary>�X�N���[���\���ǂ��� </summary>
    bool _isScroll = false;

    /// <summary>���т�\������L�����o�X </summary>
    public Canvas UICanvas { get => _uiCanvas; }

    /// <summary>����UI���J�������̏��� </summary>
    void SetupAchievement(GetAchievementList getAchievement)
    {
        var loadData = new List<bool[]>() { getAchievement._killAchievements, getAchievement._helpAchievements, getAchievement._rankAchievements };

        for (var i = 0; i < _achievementLists.Length; i++)
        {
            var target = _achievementLists[i];
            var loaddata = loadData[i];
            var achievementData = _achievementData[i];

            for (var k = 0; k < target.childCount; k++)
            {

                var achievement = target.GetChild(k).GetComponent<AchievementController>();
                var trigger = target.GetChild(k).GetComponent<EventTrigger>();

                EventTriggerEntry(trigger, EventTriggerType.Select, SelectAchievement, achievement);        //�̍������Z�b�g����
                EventTriggerEntry(trigger, EventTriggerType.Select, ScrollAchievementList, achievement);    //�X�N���[���@�\��ǉ�
                EventTriggerEntry(trigger, EventTriggerType.Deselect, ScrollAchievementList, achievement);

                achievement.OpenAchievementUI(loaddata[k], achievementData.AchievementInfos[k]._conditions);
            }
        }

        var firstAchievement = _achievementLists[0].GetChild(0).GetComponent<AchievementController>();  //���т��J�������ɑI������Ă�����уt���[���̐F��ύX����
        firstAchievement.SelectAchievement();

        SelectAchievement(firstAchievement);    //�ŏ��̎��т𒲂ׂ�

        _leftButtonDestinations[0].Select();
        _currentType = AchievementType.Kill;
        ChangeDestination();
        _isScroll = true;
    }


    /// <summary>���і��������ɖ������C���X�g��\������ </summary>
    /// <param name="flag">�������Ă��邩�ǂ���</param>
    void AchievementHide(bool flag)
    {
        var hideImageColor = _hideImage.color;
        var hideFrameColor = _hideFarme.color;
        var hideTextColor = _hideText.color;

        if (flag)
        {
            hideImageColor.a = 0;
            hideFrameColor.a = 0;
            hideTextColor.a = 0;

            _hideImage.color = hideImageColor;
            _hideFarme.color = hideFrameColor;
            _hideText.color = hideTextColor;

            return;
        }

        hideImageColor.a = 1;
        hideFrameColor.a = 1;
        hideTextColor.a = 1;

        _hideImage.color = hideImageColor;
        _hideFarme.color = hideFrameColor;
        _hideText.color = hideTextColor;
    }


    /// <summary>EventTrigger�Ɋ֐���o�^���鏈�� </summary>
    void EventTriggerEntry(EventTrigger trigger, EventTriggerType type, Action<AchievementController> action, AchievementController achievement)
    {
        var entry = new EventTrigger.Entry();       //EventTrigger�Ɋ֐���o�^����
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => action.Invoke(achievement));
        trigger.triggers.Add(entry);
    }

    /// <summary>����UI���J�������̏����@���������� </summary>
    public async UniTask OpenAchievementUI()
    {
        var achievementData = await JsonProcessClass.TestLoadData<GetAchievementList>(DataType.Achievement); //�f�[�^��ǂݍ���

        _changeListButtonTexts[0] = _changeListButtons[0].transform.GetChild(0).GetComponent<Text>();
        _changeListButtonTexts[1] = _changeListButtons[1].transform.GetChild(0).GetComponent<Text>();
        _currnetShowAchievementList = _achievementLists[0];
        _currentMoveAmount = _scrollMoveAmounts[0];
        _currentScrollberMoveAmount = _scrollbarMoveAmounts[0];
        _currentAchievementData = _achievementData[0];

        UpdateAchievementListName(_achievementLists.Length);
        ChangeDestination();
        SetupAchievement(achievementData);

        _uiCanvas.enabled = true;
    }

    /// <summary>�\�����Ă�����у��X�g�ɍ��킹�ă{�^���̈ړ����ύX���� </summary>
    void ChangeDestination()
    {
        var liftNavi = _changeListButtons[0].navigation;
        var rightNavi = _changeListButtons[1].navigation;
        var nextIndex = (int)_currentType % _achievementLists.Length;

        liftNavi.selectOnDown = _leftButtonDestinations[nextIndex];
        rightNavi.selectOnDown = _rightButtonDestinations[nextIndex];

        _changeListButtons[0].navigation = liftNavi;
        _changeListButtons[1].navigation = rightNavi;


        _leftButtonDestinations[nextIndex].Select();    //�\�����Ă�����у��X�g���ύX���ꂽ�獶��ɂ���{�^����I������
    }

    /// <summary>���X�g����\������e�L�X�g���X�V���鏈�� </summary>
    void UpdateAchievementListName(int index)
    {
        if (index <= 0)
        {
            index = _achievementLists.Length;
        }

        _achievementListLabel.text = $"{_listNames[index % _achievementLists.Length]} ����";

        //���X�g��ύX����{�^���̃e�L�X�g���X�V
        _changeListButtonTexts[0].text = _listNames[(index - 1) % _achievementLists.Length];
        _changeListButtonTexts[1].text = _listNames[(index + 1) % _achievementLists.Length];
    }

    /// <summary>�̍����X�g���X�N���[�������� </summary>
    public void ScrollAchievementList(AchievementController controller)
    {
        if (!_isScroll) { return; }

        if (_selectedAchievement.transform.position.y < controller.transform.position.y)     //���X�g����Ɉړ�������
        {
            _currnetShowAchievementList.DOComplete();
            _currnetShowAchievementList.DOAnchorPosY(_currnetShowAchievementList.localPosition.y - _currentMoveAmount, _scrollSpeed);
            _scrollbar.value -= _currentScrollberMoveAmount;
        }
        else if (controller.transform.position.y < _selectedAchievement.transform.position.y)  //���X�g�����Ɉړ�������
        {
            _currnetShowAchievementList.DOComplete();
            _currnetShowAchievementList.DOAnchorPosY(_currnetShowAchievementList.localPosition.y + _currentMoveAmount, _scrollSpeed);
            _scrollbar.value += _currentScrollberMoveAmount;
        }

        _selectedAchievement = controller;
    }

    /// <summary>�̍������Z�b�g���� </summary>
    public void SelectAchievement(AchievementController controller)
    {
        if (controller.IsOpen)
        {
            _showAchievementText.text = $"�̍��� : {_currentAchievementData.AchievementInfos[controller.ID]._label}";
            _showDetail1Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._detail1}";
            _showDetail2Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._detail2}";
            _showImage.sprite = controller.AchievementImage.sprite;
        }
        else
        {
            _showAchievementText.text = $"�������";
            _showDetail1Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._conditions}";
            _showDetail2Text.text = $" ";
        }

        AchievementHide(controller.IsOpen);
    }

    /// <summary>
    /// �\��������у��X�g��ύX����
    /// �{�^������Ăяo��
    /// </summary>
    public void ChangeShowAchievementList(int index)
    {
        _currnetShowAchievementList.DOComplete();

        var nextIndex = (int)_currentType + index;

        if (nextIndex < 0)
        {
            nextIndex = _achievementLists.Length - 1;
        }

        _currnetShowAchievementList.gameObject.SetActive(false);
        _currnetShowAchievementList = _achievementLists[nextIndex % _achievementLists.Length];
        _currnetShowAchievementList.gameObject.SetActive(true);
        _currentMoveAmount = _scrollMoveAmounts[nextIndex % _achievementLists.Length];  //�X�N���[���̋�����ύX����
        _currentScrollberMoveAmount = _scrollbarMoveAmounts[nextIndex % _achievementLists.Length]; //�X�N���[���o�[�̈ړ�������ύX����
        _currentAchievementData = _achievementData[nextIndex % _achievementLists.Length];

        UpdateAchievementListName(nextIndex);

        _currentType = (AchievementType)nextIndex;
        ChangeDestination();
    }
     
    /// <summary>����UI����� </summary>
    public void CloseUI()
    {
        for (var i = 0; i < _achievementLists.Length; i++)
        {
            if (i <= 0)
            {
                _achievementLists[i].gameObject.SetActive(true);
                _achievementLists[i].localPosition = ACHIEVEMENT_LIST_FIRST_POSITION;
                continue;
            }

            _achievementLists[i].gameObject.SetActive(false);
            _achievementLists[i].localPosition = ACHIEVEMENT_LIST_FIRST_POSITION;
        }

        SelectAchievement(_firstButton);

        _isScroll = false;
        _selectedAchievement = _firstButton;
        _scrollbar.value = 0;
    }
}

/// <summary>���т̎�� </summary>
public enum AchievementType
{
    /// <summary>�|������ </summary>
    Kill = 0,
    /// <summary>�~�o��</summary>
    Help = 1,
    /// <summary>�K��</summary>
    Rank = 2,
}
