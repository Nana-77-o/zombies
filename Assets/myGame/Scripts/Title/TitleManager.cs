using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

/// <summary>�^�C�g���E�z�[���֘A���Ǘ�����N���X </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>�t�F�[�h�ɂ����鎞��</summary>
    const float HOME_FADE_TIME = 0.1f;
    /// <summary>�t�F�[�h�ŕύX����l</summary>
    const float FADE_VALUE = 0.1f;

    [SerializeField, Header("�C���Q�[���V�[����")]
    string _inGameSceneName = "";
    [SerializeField, Header("�z�[����ʑJ�ڎ��̐F")]
    Color _fadeColor = default;
    [SerializeField, Tooltip("�z�[��UI")]
    Canvas _homeUI = default;
    [SerializeField, Tooltip("���т�\������UI")]
    AchievementManager _achievementUI = default;
    [SerializeField, Tooltip("�^�C�g��UI���Ǘ�����N���X")]
    TitleBaseMove _titleBase = default;
    [SerializeField, Tooltip("�z�[����ʗp�J����")]
    Camera _homeCamera = null;
    [SerializeField, Tooltip("�z�[����ʂɑJ�ڂ���t�F�[�hUI")]
    FadeImage _homeFadeImage = null;
    [SerializeField, Tooltip("���͂��Ǘ�����N���X")]
    InputSystemUIInputModule _inputSystemUIInputModule = null;
    [SerializeField, Header("���у{�^��")]
    Button _achievementButton = default;
    [SerializeField, Header("�ݒ�{�^��")]
    Button _optionButton = default;
    [SerializeField, Header("�Q�[���X�^�[�g�{�^��")]
    Button _gameStartButton = default;
    [SerializeField, Header("���킪���I�����̐F")]
    Color _notSelectColor = default;
    [SerializeField, Header("�I�����������Image�z��"), Tooltip("0=�� 1=�{ 2=�� 3=�y")]
    Image[] _selectedWeaponImages = new Image[4];
    [SerializeField, Header("�ŏ��ɑI������Ă��镐��"), Tooltip("0=�� 1=�{ 2=�� 3=�y")]
    HomeWeaponData[] _currentWeaponImages = new HomeWeaponData[4];
    [SerializeField, Tooltip("�ݒ�UI���Ǘ�����N���X")]
    OptionManager _optionManager = default;

    [SerializeField]
    EventSystem eventSystem;

    /// <summary>�^�C�g�����͂��󂯎��</summary>
    GameInputs _inputs = default;

    /// <summary>���݂̑I������Ă���A�j���[�^�[</summary>
    Animator _currentAnimator = default;

    private void Awake()
    {
        //�^�C�g���E�z�[����ʂł̓��͏�����ݒ�
        _inputs = new GameInputs();
        _inputs.Title.Transition.started += _ => HomeUITransition();
        _inputs.Achievement.BackHome.started += _ => CloseAchievement();
        _inputs.Option.BackUI.started += _ => CloseOption();
        _inputs.Title.Enable();
    }

    void Start()
    {
        for (var i = 0; i < _currentWeaponImages.Length; i++)   //�ŏ��̕����I����Ԃɂ���
        {
            SelectAttribute(_currentWeaponImages[i]);
        }

        _gameStartButton.onClick.AddListener(GameStart);
    }


    private void Update()
    {
        if (Keyboard.current == null) { return; }    //�L�[�{�[�h���ڑ�����Ă��Ȃ���Ή������Ȃ�

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log(eventSystem.currentSelectedGameObject.name);
        }
    }

    /// <summary>�t�F�[�h���s��</summary>
    async UniTask Fade(FadeMode mode, Color fadeColor, Action endAction = null)
    {
        _homeFadeImage.color = fadeColor;  //�t�F�[�h���̐F��ݒ�

        if (mode == FadeMode.FadeOut)
        {
            _homeFadeImage.Range = 0f;

            while (_homeFadeImage.Range < 1f)
            {
                _homeFadeImage.Range += FADE_VALUE;
                await UniTask.Delay(TimeSpan.FromSeconds(HOME_FADE_TIME));
            }
        }
        else
        {
            _homeFadeImage.Range = 1f;

            while (0f < _homeFadeImage.Range)
            {
                _homeFadeImage.Range -= FADE_VALUE;
                await UniTask.Delay(TimeSpan.FromSeconds(HOME_FADE_TIME));
            }
        }

        endAction?.Invoke();
    }

    /// <summary>�Q�[�����J�n���� </summary>
    public void GameStart()
    {
        Debug.Log("�Q�[���X�^�[�g");
        SceneControl.ChangeTargetScene(_inGameSceneName);
    }

    /// <summary>���헓���ړ������ĕ\������</summary>
    public void OnWeaponSelect(Animator animator)
    {
        if (_currentAnimator != null)
        {
            if (_currentAnimator != animator)
            {
                _currentAnimator.Play("DeSelect");
            }
        }

        _currentAnimator = animator;
        animator.Play("Select");
    }

    /// <summary>���헓���ړ������Ĕ�\������</summary>
    public void OnWeaponDeSelect()
    {
        if (_currentAnimator == null) { return; }

        _currentAnimator.Play("DeSelect");
    }

    /// <summary>
    /// ���т�\������ 
    /// �{�^������Ăяo��
    /// </summary>
    public async void ActiveAchievement()
    {
        _inputs.Title.Disable();
        _inputs.Achievement.Enable();
        _inputSystemUIInputModule.enabled = false;

        await Fade(FadeMode.FadeOut, _fadeColor);
        await _achievementUI.OpenAchievementUI();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        await Fade(FadeMode.FadeIn, _fadeColor, () => 
        {
            _inputSystemUIInputModule.enabled = true;
            _homeUI.enabled = false;
        });
    }

    /// <summary>���т����</summary>
    public async void CloseAchievement()
    {
        _inputSystemUIInputModule.enabled = false;
        await Fade(FadeMode.FadeOut, _fadeColor, () => _homeUI.enabled = true);
        _achievementUI.UICanvas.enabled = false;
        _achievementButton.Select();
        _inputs.Achievement.Disable();
        _inputs.UI.Enable();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        _achievementUI.CloseUI();
        await Fade(FadeMode.FadeIn, _fadeColor, () => _inputSystemUIInputModule.enabled = true);
    }

    /// <summary>�ݒ��UI���J�� </summary>
    public void OpenOptionUI()
    {
        _inputs.Option.Enable();
        _optionManager.Open();
    }

    /// <summary>�ݒ��UI����� </summary>
    public void CloseOption()
    {
        _inputs.Disable();
        _inputs.UI.Enable();
        _optionManager.Close(_optionButton);
    }

    /// <summary>
    /// ����̑�����ύX����
    /// �{�^������Ăяo��
    /// </summary>
    public void SelectAttribute(HomeWeaponData data)
    {
        UserData.Data._selectWeapos[(int)data.Attribute] = data.Weapon;      //�C���Q�[���V�[���ɑI����������������p��
        data.ChangeSprite(_selectedWeaponImages[(int)data.Attribute]);     //�I�𕐊�̃C���X�g��ύX

        _currentWeaponImages[(int)data.Attribute].ChangeWeaponImageColor(SelectState.notSelect, _notSelectColor);   //���݂̕����I������
        _currentWeaponImages[(int)data.Attribute] = data;
        _currentWeaponImages[(int)data.Attribute].ChangeWeaponImageColor(SelectState.select, _notSelectColor);

        data.CurrentAttributeAnimator.Play("DeSelect");
        data.AttributeBackButton.Select();  //�����I���̃{�^����I������
    }

    /// <summary>�z�[����ʂɑJ�ڂ��� </summary>
    public void HomeUITransition()
    {
        _inputs.Title.Disable();
        _inputs.UI.Enable();

        _titleBase.HomeTransition(async () =>
        {
            _gameStartButton.Select();
            _homeCamera.enabled = true;
            _homeUI.enabled = true;
            await Fade(FadeMode.FadeIn, _fadeColor);
        });
    }
}

/// <summary>����C���X�g�̐F��ύX����� �i�z�[����ʂŎg�p�j</summary>
public enum SelectState
{
    /// <summary>���킪�I�����ꂽ</summary>
    select = 1,
    /// <summary>�I������Ă��Ȃ�</summary>
    notSelect = 2,
}

/// <summary>�t�F�[�h���� </summary>
public enum FadeMode
{
    /// <summary>���邭�Ȃ�</summary>
    FadeIn,
    /// <summary>�Â��Ȃ�</summary>
    FadeOut,
}