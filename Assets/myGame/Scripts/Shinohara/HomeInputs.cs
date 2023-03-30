using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HomeInputs : MonoBehaviour
{
    [SerializeField, Header("�ŏ��ɑI������Ă���{�^��")]
    Button _firstSelectButton = default;


    /// <summary>���ݑI������Ă���{�^��</summary>
    Button _currentSelectButton = default;

    private void Start()
    {
        _currentSelectButton = _firstSelectButton;
        _currentSelectButton.Select();
    }
}
