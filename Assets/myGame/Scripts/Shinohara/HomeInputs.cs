using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HomeInputs : MonoBehaviour
{
    [SerializeField, Header("最初に選択されているボタン")]
    Button _firstSelectButton = default;


    /// <summary>現在選択されているボタン</summary>
    Button _currentSelectButton = default;

    private void Start()
    {
        _currentSelectButton = _firstSelectButton;
        _currentSelectButton.Select();
    }
}
