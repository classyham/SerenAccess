using TMPro;
using UdonSharp;
using UnityEngine;

namespace SerenAccess
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class AccessListViewer : UdonSharpBehaviour
    {
        private TextMeshProUGUI _TextComponent;

        private void Start()
    {
        Transform childTransform = transform.Find("Content");
        
        if (childTransform != null)
        {
            _TextComponent = childTransform.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Could not find a child named 'Content'!");
        }
    }

        public void UpdateList(string[] GroupNames, string[] GroupContents)
        {
            if (GroupNames == null || GroupContents == null || GroupNames.Length != GroupContents.Length)
            {
                if (_TextComponent != null) _TextComponent.text = "Error: Data mismatch.";
                return;
            }

            string finalDisplay = "";

            for (int i = 0; i < GroupNames.Length; i++)
            {
                string currentGroupName = GroupNames[i];
                string currentGroupContent = GroupContents[i];

                if (string.IsNullOrEmpty(currentGroupName)) continue;

                finalDisplay += $"<size=150%><b>{currentGroupName}</b></size>\n";

                finalDisplay += $"<indent=5%>{currentGroupContent}</indent>\n";
            }

            if (_TextComponent != null)
            {
                _TextComponent.text = finalDisplay;
            }
        }
    }
}