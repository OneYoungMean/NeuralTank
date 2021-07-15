using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boob : MonoBehaviour //OYM:µØÀ×Å¶
{
    public bool IsDetected {
        get { return !gameObject.activeSelf; }
        set { gameObject.SetActive(!value); }
    }
}
