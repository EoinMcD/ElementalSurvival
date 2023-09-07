using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] string[] npcDialogue;
    int timesInteracted=0;

    public void Interact() {
        Debug.Log(npcDialogue[timesInteracted]);
        timesInteracted++;
        if(timesInteracted == npcDialogue.Length){
            timesInteracted=0;
        }
    }
}
