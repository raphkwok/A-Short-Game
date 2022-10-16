using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public bool isChoice;

    public Animator[] choices;
    public TMP_Text text;

    public Dialogue[] nextOptions;

}
