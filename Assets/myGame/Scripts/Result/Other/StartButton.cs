using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().Select();
    }
}
