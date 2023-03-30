using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private enum Angle
    {
        Flont = 0,
        Back = 1,
        Right = 2,
        Left = 3,
    }
    private enum State
    {
        Idle,
        Run,
        Attack,
    }
    [SerializeField]
    private string[] _idleNames = { "PlayerIdleFront", "PlayerIdleBack", "PlayerIdleRight", "PlayerIdleLeft" };
    [SerializeField]
    private string[] _runNames = { "PlayerRunPrevious", "PlayerRunBack", "PlayerRunRight", "PlayerRunLeft" };
    [SerializeField]
    private string[] _attackNames = { "AttackFront", "AttackBack", "AttackRight", "AttackLeft" };
    private float _moveMagnitude = 0.2f;
    private Animator _playerAnim;
    private State _state = State.Idle;
    private Angle _angle = Angle.Flont;
    private Angle _animeAngle = Angle.Flont;
    private bool _isAttacking = false;
    private void Start()
    {
        _playerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        var dir = PlayerInput.InputVector;
        _angle = GetAngle(dir);
        if (_isAttacking == true) { return; }
        if (dir.magnitude < _moveMagnitude)
        {
            SetAnimation(_angle, State.Idle);
        }
        else
        {
            SetAnimation(_angle, State.Run);
        }
    }
    /// <summary>
    /// �������������Ԃ�
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Angle GetAngle(Vector2 dir)
    {
        if (dir.y != 0)
        {
            if (dir.y > 0)
            {
                return Angle.Back;
            }
            return Angle.Flont;
        }
        else if (dir.x != 0)
        {
            if (dir.x > 0)
            {
                return Angle.Right;
            }
            return Angle.Left;
        }
        return _angle;
    }
    /// <summary>
    /// �����Ə�Ԃ��w�肵�A�j���[�V�����Đ����s��
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="state"></param>
    private void SetAnimation(Angle angle, State state)
    {
        if (state == _state && angle == _animeAngle) { return; }
        _animeAngle = angle;
        _state = state;
        switch (_state)
        {
            case State.Idle:
                _playerAnim.Play(_idleNames[(int)_angle]);
                break;
            case State.Run:
                _playerAnim.Play(_runNames[(int)_angle]);
                break;
            case State.Attack:
                _playerAnim.Play(_attackNames[(int)_angle]);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// �U�����s��
    /// </summary>
    public void Attack()
    {
        SetAnimation(_angle, State.Attack);
        _isAttacking = true;
    }
    /// <summary>
    /// �U���I��
    /// </summary>
    private void EndAttack()
    {
        _isAttacking = false;
    }
}
