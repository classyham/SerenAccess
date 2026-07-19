using SerenJson;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

namespace SerenAccess
{
    public class WorldStateListener_GameObject : JsonListenerBase
    {
        [Header("Objects")]
        [Tooltip("Objects that will be enabled when the JSON key is true, and disabled when false.")]
        [SerializeField] private GameObject[] enabledObjects;

        [Tooltip("Objects that will be disabled when the JSON key is true, and enabled when false.")]
        [SerializeField] private GameObject[] disabledObjects;

        void Start()
        {
            foreach(GameObject gameObject in enabledObjects)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }

            foreach(GameObject gameObject in disabledObjects)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public override void OnWorldStateChanged()
            {
                if (jsonManager == null)
                {
                    Debug.LogError("[WorldStateListener_GameObjects] JsonManager is not set.");
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

                foreach(GameObject gameObject in enabledObjects)
                {
                    if(gameObject != null)
                    {
                        gameObject.SetActive(bIsOnList);
                    }
                }
                foreach(GameObject gameObject in disabledObjects)
                {
                    if(gameObject != null)
                    {
                        gameObject.SetActive(!bIsOnList);
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
