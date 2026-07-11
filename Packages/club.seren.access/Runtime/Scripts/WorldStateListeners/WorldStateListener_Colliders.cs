using SerenJson;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

namespace SerenAccess
{
    public class WorldStateListener_Colliders : JsonListenerBase
    {

        [Header("Colliders")]
        [Tooltip("Colliders that will be enabled when the JSON key is true, and disabled when false.")]
        [SerializeField] private Collider[] enabledColliders;

        [Tooltip("Colliders that will be disabled when the JSON key is true, and enabled when false.")]
        [SerializeField] private Collider[] disabledColliders;

        private void Start()
        {
            foreach (var col in enabledColliders)
            {
                if (col != null)
                {
                    col.enabled = false; // Start with colliders disabled until we check the world state
                }
            }

            foreach (var col in disabledColliders)
            {
                if (col != null)
                {
                    col.enabled = true; // Start with disabled colliders enabled until we check the world state
                }
            }
        }

        public override void OnWorldStateChanged()
        {
            if (jsonManager == null)
            {
                Debug.LogError("[WorldStateListener_Colliders] JsonManager is not set.");
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

            foreach (var col in enabledColliders)
            {
                if (col != null)
                {
                    col.enabled = bIsOnList;
                }
            }

            foreach (var col in disabledColliders)
            {
                if (col != null)
                {
                    col.enabled = !bIsOnList;
                }
            }
        }

        private bool CheckKeys(string key)
        {
        DataList dataList = jsonManager.GetArray(key);

            if (dataList.Contains(Networking.LocalPlayer.displayName))
            {
                return true;
            }
            return false;
        }
    }
}
