using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Flying_new2 : MonoBehaviour
{
    // DESCRIPTION: double click on the button below to see
    /* Description:
     * 1.) each time the pad of the pad of the left controller is
     * clicked, switch state of player from moving to not moving or viceversa
     * 2.) if the player is moving, the direction is defined by the vector that
     * goes from the belly (head - bellyToHead) to the left controller. The
     * velocity is proportional to the magnitude of that vector.
     * 3.) if the left trigger is holded, mySpeed becomes SuperSpeed
     */

    private float my_speed; // speed of the player in the game
    public float speed = 0.01f; // normal speed which is asigned automatically
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Behaviour_Pose controllerPose;
    public Transform head;
    private Vector3 belly;
    public float bellyToHead = 0.5f; // distance between head and belly of the user 

    private bool isFlying = false; // is the player moving?
    private bool isSuperSpeed = false; // is SuperSpeed state active?

    public float superSpeed; // speed the player has when clicking the left trigger
    public SteamVR_Action_Boolean grabPinch;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;


    void Update()
    {
        superSpeed = 1.5f;
        belly = new Vector3(head.transform.position.x, head.transform.position.y - bellyToHead, head.transform.position.z);

        if (teleportAction.GetStateDown(handType))
        {
            isFlying = !isFlying;
        }

        if (isFlying)
        {
            if (grabPinch.GetStateDown(handType))
            {
                isSuperSpeed = true;
            }

            if (grabPinch.GetStateUp(handType))
            {
                isSuperSpeed = false;
            }

            if (isSuperSpeed)
            {
                my_speed = superSpeed;
            }

            if (!isSuperSpeed)
            {
                my_speed = speed;
            }

            Vector3 dir = my_speed * (controllerPose.transform.position - belly);
            transform.position += dir;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            isFlying = false;
            transform.position = new Vector3 (0, 10, 0);
        }
    }

}
