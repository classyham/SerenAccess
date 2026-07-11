using SerenJson;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

namespace SerenAccess
{
    public class WorldStateListener_UIButtons : JsonListenerBase
    {

        [Header("UI Buttons")]
        [Tooltip("Buttons that will be enabled when the JSON key is true, and disabled when false.")]
        [SerializeField] private Button[] enabledButtons;

        [Tooltip("Buttons that will be disabled when the JSON key is true, and enabled when false.")]
        [SerializeField] private Button[] disabledButtons;

        private void Start()
        {
            foreach (Button button in enabledButtons)
            {
                if (button != null)
                {
                    button.interactable = false;
                }
            }

            foreach (Button button in disabledButtons)
            {
                if (button != null)
                {
                    button.interactable = true;
                }
            }
        }

        public override void OnWorldStateChanged()
        {
            if (jsonManager == null)
            {
                Debug.LogError("[WorldStateListener_UIButtons] JsonManager is not set.");
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

            foreach (Button button in enabledButtons)
            {
                if (button != null)
                {
                    button.interactable = bIsOnList;
                }
            }

            foreach (Button button in disabledButtons)
            {
                if (button != null)
                {
                    button.interactable = !bIsOnList;
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
