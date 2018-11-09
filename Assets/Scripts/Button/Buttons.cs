using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void Enable()
    {
        Transform childTransform = gameObject.GetComponentInChildren<Transform>();

        foreach (Transform child in childTransform)
        {
            child.GetComponent<Button>().interactable = true;
        }
    }

    public void Disable()
    {
        Transform childTransform = gameObject.GetComponentInChildren<Transform>();

        foreach (Transform child in childTransform)
        {
            child.GetComponent<Button>().interactable = false;
        }
    }
}
