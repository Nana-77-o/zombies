using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    [SerializeField] State _currntState = State.Previous;
    Animator _anim => GetComponent<Animator>();

    private void Start()
    {
        if (_currntState != State.None)
        {
            _anim.Play(_currntState.ToString());
        }
    }


    enum State
    {
        Previous,
        Back,
        Left,
        Right,
        None,
    }
}
