using SerenAccess;
using Texel;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

public class WorldStateHandler_TXL : AccessControlHandler
{
    [Header("World State")]
    [Tooltip("The World State Manager script.")]
    [SerializeField] private WorldStateManager worldStateManager;

    [Tooltip("The JSON key that will be read from the world state. The key uses dot notation to access nested objects, e.g. 'WorldPerms.Admin'.")]
    [SerializeField] private string jsonKey;

    protected override void _Init()
    {
            base._Init();
    }
    
    public override AccessHandlerResult _CheckAccess(VRCPlayerApi player)
    {
        if (worldStateManager == null)
        {
            Debug.LogError("[WorldStateHandler_TXL] worldStateManager is not set.");
            return AccessHandlerResult.Deny;
        }

        bool bIsOnList = false;

        DataList dataList = worldStateManager.GetArray(jsonKey);

        if (dataList.Contains(player.displayName))
        {
            bIsOnList = true;
        }

        return bIsOnList ? AccessHandlerResult.Allow : AccessHandlerResult.Deny;
    }



}
