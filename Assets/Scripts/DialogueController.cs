using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject dialogue1;
    [SerializeField] GameObject dialogue2;
    void Start()
    {
        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        dialogue1.SetActive(true);
        dialogue2.SetActive(false);

        yield return new WaitForSeconds(3f);

        dialogue1.SetActive(false);
        dialogue2.SetActive(true);

        yield return new WaitForSeconds(3f);

        dialogue2.SetActive(false);
    }
}
