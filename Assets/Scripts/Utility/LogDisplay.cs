using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    public Text[] text = null;

    public void DebagLog(string str, int num)
    {
        text[num].text = str;
    }
}
