using SerenJson;
using System.Text;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace SerenAccess
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class WorldStateListener_DisplayNames : JsonListenerBase
    {
        [Header("Display Configuration")]
        [Tooltip("Should we trim the names to content after the last dot? (e.g., 'WorldPerms.Admin' becomes 'Admin')")]
        [SerializeField] private bool trimNames = false;

        [Tooltip("The access viewer script that will display the structured group names and players.")]
        [SerializeField] private AccessListViewer accessListViewer;

        public override void OnWorldStateChanged()
        {
            if (jsonManager == null || accessListViewer == null)
            {
                Debug.LogError("[WorldStateListener_DisplayNames] Missing JsonManager or accessListViewer dependencies.");
                return;
            }

            int totalKeys = 1 + (additionalJsonKeys != null ? additionalJsonKeys.Length : 0);
            string[] keysToProcess = new string[totalKeys];
            keysToProcess[0] = jsonKey;
            
            if (additionalJsonKeys != null)
            {
                for (int i = 0; i < additionalJsonKeys.Length; i++)
                {
                    keysToProcess[i + 1] = additionalJsonKeys[i];
                }
            }

            string[] groupNames = new string[totalKeys];
            string[] groupContents = new string[totalKeys];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < keysToProcess.Length; i++)
            {
                string currentKey = keysToProcess[i];
                if (string.IsNullOrEmpty(currentKey)) continue;

                string processedGroupName = currentKey;
                if (trimNames && currentKey.Contains("."))
                {
                    int lastDotIndex = currentKey.LastIndexOf('.');
                    if (lastDotIndex >= 0 && lastDotIndex < currentKey.Length - 1)
                    {
                        processedGroupName = currentKey.Substring(lastDotIndex + 1);
                    }
                }
                groupNames[i] = processedGroupName.ToUpper();

                DataList dataList = jsonManager.GetArray(currentKey); 

                sb.Clear();
                
                if (dataList != null && dataList.Count > 0)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        DataToken token = dataList[j];
                        if (token.TokenType == TokenType.String)
                        {
                            sb.Append(token.String);
                            if (j < dataList.Count - 1)
                            {
                                sb.AppendLine();
                            }
                        }
                    }
                    groupContents[i] = sb.ToString();
                }
                else
                {
                    groupContents[i] = "None"; 
                }
            }

            accessListViewer.UpdateList(groupNames, groupContents);
        }
    }
}