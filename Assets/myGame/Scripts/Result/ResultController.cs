using System.Collections;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    // 早送り・スキップ仕様についてメモ
    // Fire1ボタンを押下で早送り開始、もう一度Fire1ボタン押下で早送り終了。
    // Fire1ボタンを3秒以上継続して押下したとき演出スキップ

    [Tooltip("早送り時の再生速度"), SerializeField]
    private float _fastForwardPlaySpeed = 2f;
    [Tooltip("スキップ時間"), SerializeField]
    private float _skipTime = 3f;

    /// <summary> スキップタイマーが生きているかどうか表す値 </summary>
    private bool _isSkipTimerAlive = false;

    /// <summary> 現在の再生速度 </summary>
    public float CurrentPlaySpeed { get; private set; } = 1f;
    /// <summary> スキップするまでの時間 </summary>
    public float SkipTime => _skipTime;
    /// <summary> 早送りするかどうかを表す値 </summary>
    public bool IsFastForward { get; private set; } = false;
    /// <summary> スキップするかどうかを表す値 </summary>
    public bool IsSkip { get; private set; } = false;
    /// <summary> 演出が完了したかどうか表す値 </summary>
    public bool IsEnd { get; set; } = false;
    /// <summary> 早送り時の再生速度 </summary>
    public float FastForwardPlaySpeed => _fastForwardPlaySpeed;

    // 入力周りメモ
    // PlayerInput 入力を司るシングルトン
    // SetEnterInput(), SetExitInput()で登録する
    // LiftEnterInput(), LiftExitInput()で解除する
    // InputType.Fire1 に早送り
    // InputType.Fire2 にスキップを割り当てる
    private void OnEnable()
    {
        // 早送り・スキップ機能をボタンに登録する
        PlayerInput.SetEnterInput(InputType.Fire1, FastForwardEnter);
        PlayerInput.SetEnterInput(InputType.Fire1, SkipEnter);
        PlayerInput.SetExitInput(InputType.Fire1, FastForwardExit);
        PlayerInput.SetExitInput(InputType.Fire1, SkipExit);
    }
    private void OnDisable()
    {
        // ボタンに割り当てられた早送り・スキップ機能を解除する
        PlayerInput.LiftEnterInput(InputType.Fire1, FastForwardEnter);
        PlayerInput.LiftEnterInput(InputType.Fire1, SkipEnter);
        PlayerInput.LiftExitInput(InputType.Fire1, FastForwardExit);
        PlayerInput.LiftExitInput(InputType.Fire1, SkipExit);
    }
    /// <summary> 早送りボタン押下時処理 </summary>
    private void FastForwardEnter()
    {
        IsFastForward ^= true; // 早送りするかどうかを表す値を更新
        if (IsFastForward)
        {
            CurrentPlaySpeed = _fastForwardPlaySpeed; // 現在速度を早送り用に変更する。
        }
        else
        {
            CurrentPlaySpeed = 1f;
        }
    }
    /// <summary> 早送りボタン開放時処理 </summary>
    private void FastForwardExit()
    {

    }

    /// <summary> スキップ用タイマー </summary>
    private float _skipTimer = 0f;
    public float SkipTimer => _skipTimer;

    /// <summary> スキップボタン押下時処理 </summary>
    private void SkipEnter()
    {
        _isSkipTimerAlive = true; // タイマー開始
    }
    private void SkipUpdate()
    {
        if (_isSkipTimerAlive && _skipTimer < _skipTime)
        {
            // タイマー加算
            _skipTimer += Time.deltaTime;
        }
        if (_skipTimer > _skipTime)
        {
            IsSkip = true;
        }
    }
    /// <summary> スキップボタン開放時処理 </summary>
    private void SkipExit()
    {
        _isSkipTimerAlive = false; // タイマー停止
        _skipTimer = 0f;// タイマー初期化
    }

    private void Update()
    {
        SkipUpdate();
    }
}
