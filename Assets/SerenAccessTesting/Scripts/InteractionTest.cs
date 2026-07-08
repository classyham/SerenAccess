
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class InteractionTest : UdonSharpBehaviour
{
    public override void Interact()
    {
        Debug.Log("Interact called");
    }
}
