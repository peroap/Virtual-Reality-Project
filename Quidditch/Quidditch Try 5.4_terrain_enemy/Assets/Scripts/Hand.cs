using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    // DESCRIPTION: double click the button below to see description
    /* Description:
     * 1.) The script Hand is attached to the right controller  
     * 2.) The player can only interact (hold) objects that have the Tag "Interactable"
     * and when it is "colliding" with the object collider   
     * 3.) When holding the trigger, the player catches the object, when not,
     * holding, the object falls
     * 4.) If more than one object was close to the player, only the nearest
     * object will be grabbed
     */
        
    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>();

    public Camera myCamera;
    public Vector3 mySpeed;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    private void Update()
    {
        // Down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Trigger Down");
            Pickup();
        }
        // Up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Trigger Upn");
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable")) 
        return;

        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());


    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable")) 
        return;

        m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());

    }

    public void Pickup()
    {
        // Get nearest
        m_CurrentInteractable = GetNearestInteractable();
        
        // Null check
        if(!m_CurrentInteractable)
            return;
        
        // Already held, check
        if(m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();
        
        // Position
        m_CurrentInteractable.transform.position = transform.position;
        
        // Attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;
        
        // Set active hand
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        // Null check
        if (!m_CurrentInteractable)
            return;

        // Apply velocity
        mySpeed = myCamera.velocity;
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        //ADDED
        targetBody.velocity = m_Pose.GetVelocity() + mySpeed;
        //...ADDED
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();
        
        // Detach
        m_Joint.connectedBody = null;
        
        // Clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;
        
        foreach(Interactable interactable in m_ContactInteractables)
        {
                distance = (interactable.transform.position - transform.position).sqrMagnitude;
                
                if(distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = interactable;
                    }
        }
        
        return nearest;
    }
}
