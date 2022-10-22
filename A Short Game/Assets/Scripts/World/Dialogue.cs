using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{

    public bool isChoice;

    public Transform[] choiceBoxes;
    public Transform[] choiceTexts;
    public TMP_Text text;

    public Dialogue[] nextOptions;

    public bool showing; // Record if animations are happening
    public bool showed; // Record if animations are done
    public bool opened; // Used to check if while even though showing and show are false, if the choice has been just closed

    public Vector2 scale;

    // Called when dialogue choice is opened
    public IEnumerator Show()
    {
        // Set variables
        opened = true;
        showing = true;

        // Set timer 
        float time = 0;

        // Get reference to choice boxes and reset their variables/scale
        Vector3[] choiceBoxSizes = new Vector3[choiceBoxes.Length];
        Vector3[] choiceTextSizes = new Vector3[choiceBoxes.Length];
        for (int i = 0; i < choiceBoxes.Length; i++)
        {
            choiceBoxSizes[i] = (choiceBoxes[i].localScale);
            choiceTextSizes[i] = (choiceTexts[i].localScale);
            choiceBoxes[i].localScale = Vector3.zero;
            choiceTexts[i].localScale = Vector3.zero;
            choiceBoxes[i].gameObject.SetActive(true);
            choiceTexts[i].gameObject.SetActive(true);
        }

        // Close existing dialouge box
        DialogueManager.dialogueManager.dialogue.GetChild(1).GetComponent<Animator>().Play("Dialogue Box Close");

        // Wait for animation to finish
        yield return new WaitForSeconds(0.5f);

        // Animation scale of choice boxes
        while (time <= 1)
        {
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                choiceBoxes[i].localScale = Vector3.Lerp(choiceBoxes[i].localScale, choiceBoxSizes[i], Time.deltaTime * 10);
                choiceTexts[i].localScale = Vector3.Lerp(choiceTexts[i].localScale, choiceTextSizes[i], Time.deltaTime * 10);
            }
            time += Time.deltaTime;
            yield return null;
        }
        // Set showed
        showed = true;
    }

    private void Start()
    {
        if (isChoice) scale = choiceBoxes[0].localScale;
    }

    // Kind of bad but I don't care enough to rewrite this system
    private void Update()
    {
        // Handleing dialogue choice selection animations
        if (showing && showed)
        {
            // increase size of chosen dialoguemanager dialouge
            int c = DialogueManager.dialogueManager.dialogueChoice;
            choiceBoxes[c].localScale =
                Vector3.Lerp(choiceBoxes[c].localScale, scale * 1.1f, Time.deltaTime * 10);

            // Set all other dialogues to normal size
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                if (i != c)
                {
                    choiceBoxes[i].localScale =
                        Vector3.Lerp(choiceBoxes[i].localScale, scale, Time.deltaTime * 10);
                }
            }
        }
    }
    public IEnumerator Hide()
    {
        showing = false;

        DialogueManager.dialogueManager.dialogue.GetChild(1).GetComponent<Animator>().Play("Dialogue Box Open");

        float time = 0;
        while (time <= 1)
        {
            for (int i = 0; i < choiceBoxes.Length; i++)
            {
                choiceBoxes[i].localScale = Vector3.Lerp(choiceBoxes[i].localScale, Vector3.zero, Time.deltaTime * 10);
                choiceTexts[i].localScale = Vector3.Lerp(choiceTexts[i].localScale, Vector3.zero, Time.deltaTime * 10);
            }
            time += Time.deltaTime;
            yield return null;
        }
        showed = false;
        DialogueManager.dialogueManager.Next();
    }
}
