﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySoon : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

}
