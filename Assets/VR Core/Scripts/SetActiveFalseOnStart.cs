using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalseOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

}
