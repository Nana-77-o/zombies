using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

/// <summary>タイトル・ホーム関連を管理するクラス </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>フェードにかかる時間</summary>
    const float HOME_FADE_TIME = 0.1f;
    /// <summary>フェードで変更する値</summary>
    const float FADE_VALUE = 0.1f;

    [SerializeField, Header("インゲームシーン名")]
    string _inGameSceneName = "";
    [SerializeField, Header("ホーム画面遷移時の色")]
    Color _fadeColor = default;
    [SerializeField, Tooltip("ホームUI")]
    Canvas _homeUI = default;
    [SerializeField, Tooltip("実績を表示するUI")]
    AchievementManager _achievementUI = default;
    [SerializeField, Tooltip("タイトルUIを管理するクラス")]
    TitleBaseMove _titleBase = default;
    [SerializeField, Tooltip("ホーム画面用カメラ")]
    Camera _homeCamera = null;
    [SerializeField, Tooltip("ホーム画面に遷移するフェードUI")]
    FadeImage _homeFadeImage = null;
    [SerializeField, Tooltip("入力を管理するクラス")]
    InputSystemUIInputModule _inputSystemUIInputModule = null;
    [SerializeField, Header("実績ボタン")]
    Button _achievementButton = default;
    [SerializeField, Header("設定ボタン")]
    Button _optionButton = default;
    [SerializeField, Header("ゲームスタートボタン")]
    Button _gameStartButton = default;
    [SerializeField, Header("武器が未選択時の色")]
    Color _notSelectColor = default;
    [SerializeField, Header("選択した武器のImage配列"), Tooltip("0=喜 1=怒 2=哀 3=楽")]
    Image[] _selectedWeaponImages = new Image[4];
    [SerializeField, Header("最初に選択されている武器"), Tooltip("0=喜 1=怒 2=哀 3=楽")]
    HomeWeaponData[] _currentWeaponImages = new HomeWeaponData[4];
    [SerializeField, Tooltip("設定UIを管理するクラス")]
    OptionManager _optionManager = default;

    [SerializeField]
    EventSystem eventSystem;

    /// <summary>タイトル入力を受け取る</summary>
    GameInputs _inputs = default;

    /// <summary>現在の選択されているアニメーター</summary>
    Animator _currentAnimator = default;

    private void Awake()
    {
        //タイトル・ホーム画面での入力処理を設定
        _inputs = new GameInputs();
        _inputs.Title.Transition.started += _ => HomeUITransition();
        _inputs.Achievement.BackHome.started += _ => CloseAchievement();
        _inputs.Option.BackUI.started += _ => CloseOption();
        _inputs.Title.Enable();
    }

    void Start()
    {
        for (var i = 0; i < _currentWeaponImages.Length; i++)   //最初の武器を選択状態にする
        {
            SelectAttribute(_currentWeaponImages[i]);
        }

        _gameStartButton.onClick.AddListener(GameStart);
    }


    private void Update()
    {
        if (Keyboard.current == null) { return; }    //キーボードが接続されていなければ何もしない

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log(eventSystem.currentSelectedGameObject.name);
        }
    }

    /// <summary>フェードを行う</summary>
    async UniTask Fade(FadeMode mode, Color fadeColor, Action endAction = null)
    {
        _homeFadeImage.color = fadeColor;  //フェード時の色を設定

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

    /// <summary>ゲームを開始する </summary>
    public void GameStart()
    {
        Debug.Log("ゲームスタート");
        SceneControl.ChangeTargetScene(_inGameSceneName);
    }

    /// <summary>武器欄を移動させて表示する</summary>
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

    /// <summary>武器欄を移動させて非表示する</summary>
    public void OnWeaponDeSelect()
    {
        if (_currentAnimator == null) { return; }

        _currentAnimator.Play("DeSelect");
    }

    /// <summary>
    /// 実績を表示する 
    /// ボタンから呼び出し
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

    /// <summary>実績を閉じる</summary>
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

    /// <summary>設定のUIを開く </summary>
    public void OpenOptionUI()
    {
        _inputs.Option.Enable();
        _optionManager.Open();
    }

    /// <summary>設定をUIを閉じる </summary>
    public void CloseOption()
    {
        _inputs.Disable();
        _inputs.UI.Enable();
        _optionManager.Close(_optionButton);
    }

    /// <summary>
    /// 武器の属性を変更する
    /// ボタンから呼び出す
    /// </summary>
    public void SelectAttribute(HomeWeaponData data)
    {
        UserData.Data._selectWeapos[(int)data.Attribute] = data.Weapon;      //インゲームシーンに選択した武器を引き継ぐ
        data.ChangeSprite(_selectedWeaponImages[(int)data.Attribute]);     //選択武器のイラストを変更

        _currentWeaponImages[(int)data.Attribute].ChangeWeaponImageColor(SelectState.notSelect, _notSelectColor);   //現在の武器を選択解除
        _currentWeaponImages[(int)data.Attribute] = data;
        _currentWeaponImages[(int)data.Attribute].ChangeWeaponImageColor(SelectState.select, _notSelectColor);

        data.CurrentAttributeAnimator.Play("DeSelect");
        data.AttributeBackButton.Select();  //属性選択のボタンを選択する
    }

    /// <summary>ホーム画面に遷移する </summary>
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

/// <summary>武器イラストの色を変更する為 （ホーム画面で使用）</summary>
public enum SelectState
{
    /// <summary>武器が選択された</summary>
    select = 1,
    /// <summary>選択されていない</summary>
    notSelect = 2,
}

/// <summary>フェード処理 </summary>
public enum FadeMode
{
    /// <summary>明るくなる</summary>
    FadeIn,
    /// <summary>暗くなる</summary>
    FadeOut,
}