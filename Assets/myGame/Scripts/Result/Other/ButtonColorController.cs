using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColorController : MonoBehaviour
{
    [SerializeField]
    private Color _nonSelectedColor = Color.red;
    [SerializeField]
    private EventSystem _eventSystem;

    private Image _image = null;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        if (_eventSystem.currentSelectedGameObject != this.gameObject)
        {
            _image.color = _nonSelectedColor;
        }
        else
        {
            _image.color = Color.white;
        }
    }
    private struct ButtonSet
    {
        public Image _image;
        public Button _button;
    }
}
