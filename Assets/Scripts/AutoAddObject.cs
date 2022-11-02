using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddObject : MonoBehaviour
{
    void Awake()
    {
      //  GameManager.instance.allObjects.Add(gameObject);
    }

    void OnDestroy()
    {
      //  GameManager.instance.allObjects.Remove(gameObject);
    }
}
