
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

namespace SerenAccess
{
    public class WorldStateListener_Colliders : WorldStateListenerBase
    {

        [Header("Colliders")]
        [Tooltip("Colliders that will be enabled when the JSON key is true, and disabled when false.")]
        [SerializeField] private Collider[] colliders;


        private void Start()
        {
            foreach (var col in colliders)
            {
                if (col != null)
                {
                    col.enabled = false; // Start with colliders disabled until we check the world state
                }
            }
        }

        public override void OnWorldStateChanged()
        {
            if (worldStateManager == null)
            {
                Debug.LogError("[WorldStateListener_Colliders] worldStateManager is not set.");
                return;
            }

            bool bIsOnList = false;
            if (CheckKeys(jsonKey))
            {
                bIsOnList = true;
            }
            else
            {
                foreach (var key in additionalJsonKeys)
                {
                    if (CheckKeys(key))
                    {
                        bIsOnList = true;
                        break;
                    }
                }
            }

            foreach (var col in colliders)
            {
                if (col != null)
                {
                    col.enabled = bIsOnList;
                }
            }
        }

        private bool CheckKeys(string key)
        {
        DataList dataList = worldStateManager.GetArray(key);

            if (dataList.Contains(Networking.LocalPlayer.displayName))
            {
                return true;
            }
            return false;
        }
    }
}
