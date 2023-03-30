using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForwardImageController : MonoBehaviour
{
    [SerializeField]
    private ResultController _resultController = default;

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }
    void Update()
    {
        if (!_resultController.IsSkip)
        {
            _image.enabled = _resultController.IsFastForward;
        }

        if (_resultController.IsSkip || _resultController.IsEnd)
        {
            _image.enabled = false;
        }
    }
}
