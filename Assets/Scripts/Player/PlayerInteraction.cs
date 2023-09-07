using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] KeyCode interactKey = KeyCode.V;
    [SerializeField] float rayCastLength = 2f;



    private void Update() {
        if (Input.GetKeyDown(interactKey)) {
            Debug.Log("Pressing V");
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, rayCastLength)){
                InteractWithObject(hit.collider.gameObject);
            }
        }
    } 
    
    void InteractWithObject(GameObject objectToInteractWith){
        if (objectToInteractWith.TryGetComponent(out IInteractable interactable)){
            interactable.Interact();
        }
    }
}
