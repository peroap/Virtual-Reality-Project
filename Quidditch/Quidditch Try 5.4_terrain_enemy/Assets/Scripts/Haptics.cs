using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    // DESCRIPTION: double click the button below to see description
    /* Description:
     * If left trigger pushed (SuperSpeed activated), haptic feedback to player,
     * to simulate high speed   
     */

    public SteamVR_Action_Vibration hapticAction;
    public SteamVR_Action_Boolean triggerAction;

    // Update is called once per frame
    void Update()
    {
        if (triggerAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(2, 150, 75, SteamVR_Input_Sources.LeftHand);
        }
    }
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
