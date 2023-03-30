using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>�A�N�V�����}�b�v </summary>
public enum ActionMapNames
{
    /// <summary>�C���Q�[���̓���</summary>
    InGame,
    /// <summary>UI�̓��� </summary>
    UI,
}

public enum InputType
{
    /// <summary> ������� </summary>
    Submit,
    /// <summary> �L�����Z������ </summary>
    Cancel,
    /// <summary> �ړ����� </summary>
    Move,
    /// <summary> �U�����͂P </summary>
    Fire1,
    /// <summary> �U�����͂Q </summary>
    Fire2,
    /// <summary> �U�����͂R </summary>
    Fire3,
    /// <summary> L�{�^�� </summary>
    LeftShoulder,
    /// <summary> R�{�^��</summary>
    RightShoulder,
    /// <summary> X,Y�{�^�� </summary>
    Menu,
}
public class PlayerInput : MonoBehaviour
{
    private static PlayerInput _instance = default;
    private static bool _initialized = true;

    /// <summary>����؂�ւ��{�^���̖��O�i���j </summary>
    const string WEAPON_CHANGE_LEFT_NAME = "leftShoulder";
    /// <summary>����؂�ւ��{�^���̖��O�i�E�j </summary>
    const string WEAPON_CHANE_RIGHT_NAME = "rightShoulder";

    /// <summary> ���͕��� </summary>
    private Vector2 _inputVector = default;
    /// <summary>���݂̓��� </summary>
    private ActionMapNames _currentMap = ActionMapNames.InGame;
    /// <summary>InputSystem�̐ݒ�N���X</summary>
    private GameInputs _gameInputs;

    /// <summary>���͒����ǂ��� </summary>
    private bool _isInputting = false;

    /// <summary> ���͒��� </summary>
    private Dictionary<InputType, Action> _onEnterInputDic = new Dictionary<InputType, Action>();
    /// <summary> ���͒� </summary>
    private Dictionary<InputType, Action> _onStayInputDic = new Dictionary<InputType, Action>();
    /// <summary> ���͉��� </summary>
    private Dictionary<InputType, Action> _onExitInputDic = new Dictionary<InputType, Action>();
    public static PlayerInput Instance
    {
        get
        {
            if (_instance == null)//null�Ȃ�C���X�^���X������
            {
                var obj = new GameObject("PlayerInput");
                var input = obj.AddComponent<PlayerInput>();
                input.Initialization();
                _instance = input;
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    /// <summary> ���͕��� </summary>
    public static Vector2 InputVector
    {
        get
        {
            if (_instance == null || !_initialized)
            {
                return Vector2.zero;
            }
            return _instance._inputVector;
        }
    }

    /// <summary>���͒����ǂ���</summary>
    public bool IsInputting { get => _isInputting; set => _isInputting = value; }

    private void Awake()
    {
        // Input Action�C���X�^���X����
        _gameInputs = new GameInputs();

        InputInitialzie();
        ChangeActionMap(ActionMapNames.InGame);
    }

    private void Update()
    {
        if (Mouse.current != null)  //�}�E�X���ڑ�����Ă��Ȃ���Γ��͂��󂯕t���Ȃ�
        {
            if (Mouse.current.leftButton.IsPressed())   //�}�E�X�Ή�
            {
                _onStayInputDic[InputType.Fire1]?.Invoke();
            }
        }

        if (Gamepad.current == null) return;    //�Q�[���p�b�h���ڑ�����Ă��Ȃ���Γ��͂��󂯕t���Ȃ�

        if (Gamepad.current.buttonEast.IsPressed())
        {
            _onStayInputDic[InputType.Fire1]?.Invoke();
        }
    }
    private void OnDestroy()
    {
        ResetInput();
    }

    /// <summary>
    /// �������������s��
    /// </summary>
    private void Initialization()
    {
        for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
        {
            _onEnterInputDic.Add((InputType)i, null);
            _onStayInputDic.Add((InputType)i, null);
            _onExitInputDic.Add((InputType)i, null);
        }
    }
    private void ResetInput()
    {
        if (_instance == null || !_initialized) { return; }
        for (int i = 0; i < Enum.GetValues(typeof(InputType)).Length; i++)
        {
            _instance._onEnterInputDic[(InputType)i] = null;
            _instance._onStayInputDic[(InputType)i] = null;
            _instance._onExitInputDic[(InputType)i] = null;
        }
        _initialized = false;
    }

    //�ȉ�InGame��Action
    //---------------------------------------------------------------------

    /// <summary>�ړ�����</summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
        if (_inputVector == Vector2.zero) { _isInputting = false; }
        else { _isInputting = true; }
    }

    /// <summary>�U������ </summary>
    private void OnAttack(InputAction.CallbackContext context)
    {
        _onEnterInputDic[InputType.Fire1]?.Invoke();
    }

    /// <summary>�|�[�Y���� </summary>
    private void OnPause(InputAction.CallbackContext context)
    {
        _onEnterInputDic[InputType.Menu]?.Invoke();
    }

    /// <summary>�Q�[���ĊJ����</summary>
    private void OnRestart(InputAction.CallbackContext context)
    {
        _onEnterInputDic[InputType.Menu]?.Invoke();
    }

    /// <summary>����؂�ւ����� </summary>
    private void OnWeaponChange(InputAction.CallbackContext context)
    {

        if (context.control.name == WEAPON_CHANGE_LEFT_NAME)    //���������ꂽ
        {
            _onEnterInputDic[InputType.LeftShoulder]?.Invoke();
        }
        else �@�@//�E�������ꂽ
        {
            _onEnterInputDic[InputType.RightShoulder]?.Invoke();
        }
    }

    /// <summary>�ʐ^���B�鏈��</summary>
    private void OnPicture(InputAction.CallbackContext context)
    {

    }

    /// <summary>���͂�����������</summary>
    private void InputInitialzie()
    {
        InGameInputInitialzie();
    }

    /// <summary>�C���Q�[���Ŏg�p������͂�o�^���� </summary>
    private void InGameInputInitialzie()
    {
        //�ړ�
        _gameInputs.InGame.Move.started += OnMove;
        _gameInputs.InGame.Move.performed += OnMove;
        _gameInputs.InGame.Move.canceled += OnMove;

        //�U��
        _gameInputs.InGame.Attack.performed += OnAttack;

        _gameInputs.InGame.Attack.canceled += _ => _instance._onExitInputDic[InputType.Fire1]?.Invoke();

        //�|�[�Y
        _gameInputs.InGame.Pause.started += OnPause;

        //�ĊJ
        _gameInputs.InGame.Restart.started += OnRestart;

        //����؂�ւ�
        _gameInputs.InGame.WeaponChange.performed += OnWeaponChange;

        //�ʐ^���B��
        _gameInputs.InGame.Picture.performed += OnPicture;
    }

    /// <summary>���͂�؂�ւ���</summary>
    /// <param name="nextMap">���̓���</param>
    private void ChangeActionMap(ActionMapNames nextMap)
    {
        switch (nextMap)
        {
            case ActionMapNames.InGame:     //�C���Q�[���̓��͂��󂯎��
                _gameInputs.InGame.Enable();
                _gameInputs.UI.Disable();
                break;
            case ActionMapNames.UI:         //UI�̓��͂��󂯎��
                _gameInputs.UI.Enable();
                _gameInputs.InGame.Disable();
                break;
        }

        _currentMap = nextMap;
    }

    public static void SetEnterInput(InputType type, Action action)
    {
        if (!_initialized) { return; }
        Instance._onEnterInputDic[type] += action;
    }
    public static void LiftEnterInput(InputType type, Action action)
    {
        if (!_initialized) { return; }
        Instance._onEnterInputDic[type] -= action;
    }
    public static void SetStayInput(InputType type, Action action)
    {
        if (!_initialized) { return; }
        Instance._onStayInputDic[type] += action;
    }
    public static void LiftStayInput(InputType type, Action action)
    {
        if (_instance == null || !_initialized) { return; }
        _instance._onStayInputDic[type] -= action;
    }
    public static void SetExitInput(InputType type, Action action)
    {
        if (_instance == null || !_initialized) { return; }
        _instance._onExitInputDic[type] += action;
    }
    public static void LiftExitInput(InputType type, Action action)
    {
        if (_instance == null || !_initialized) { return; }
        _instance._onExitInputDic[type] -= action;
    }
}
