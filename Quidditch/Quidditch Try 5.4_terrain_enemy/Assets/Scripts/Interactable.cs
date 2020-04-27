using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    // DESCRIPTION: double click the button below to see description
    /*Description:
     * This script is used so that objects with the Tag "Interactables" can 
     * interact with the "Hand" (left controller)
     */

    [HideInInspector]
    public Hand m_ActiveHand = null;
}
