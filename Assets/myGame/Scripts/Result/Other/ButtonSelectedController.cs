using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectedController : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem = default;

    private GameObject _previousSelectedButton = null;

    void Start()
    {
        _previousSelectedButton = _eventSystem.currentSelectedGameObject;
    }
    void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_previousSelectedButton);
        }
        _previousSelectedButton = _eventSystem.currentSelectedGameObject;
    }
}
