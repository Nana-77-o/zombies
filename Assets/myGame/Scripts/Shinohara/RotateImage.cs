using UnityEngine;

/// <summary>�I�u�W�F�N�g���J�����̕����Ɍ�������ׂ̃N���X </summary>
public class RotateImage : MonoBehaviour
{
    /// <summary>�J�����I�u�W�F�N�g�̖��O </summary>
    const string CAMERA_NAME = "CMVC1";

    Transform _cameraPosition;
    Quaternion _rotationValue;

    void Start()
    {
        _cameraPosition = GameObject.Find(CAMERA_NAME).GetComponent<Transform>();
        _rotationValue = Quaternion.Euler(_cameraPosition.localEulerAngles.x, 0f, 0);
        transform.rotation = _rotationValue;
    }

    private void Update()
    {
        _rotationValue =  Quaternion.Euler(_cameraPosition.localEulerAngles.x, 0f, 0);
        transform.rotation = _rotationValue;
    }
}
