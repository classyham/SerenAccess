using SerenJson;
using SerenAccess;
using Texel;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace SerenAccess
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class WorldStateWhitelistSource : AccessControlUserSource
    {
        [Header("Data")]

        [Tooltip("The World State Manager script.")]
        [SerializeField] protected JsonManager jsonManager;

        [Tooltip("The JSON key that will be read from the world state. The key uses dot notation to access nested objects, e.g. 'WorldPerms.Admin'.")]
        [SerializeField] protected string jsonKey;

        [Tooltip("The debug log script.")]
        [SerializeField] protected DebugLog debugLog;

        [Tooltip("Enable debug logging to the console.")]
        [SerializeField] protected bool debugLogging;

        public string[] userList = new string[0];
        private DataDictionary userDict;

        protected override void _Init()
        {
            base._Init();

            userDict = new DataDictionary();
            _SetUserDict(userList);
        }

        private void _SetUserDict(string[] names)
        {
            _EnsureInit();

            userDict.Clear();
            for (int i = 0; i < userList.Length; i++)
            {
                if (userList[i] != "" && !userDict.ContainsKey(userList[i]))
                    userDict.Add(userList[i], userList[i]);
            }
        }

        public void OnWorldStateChanged()
        {
            if (jsonManager == null)
            {
                _DebugLog("[WorldStateWhitelistSource] JsonManager is not set.");
                return;
            }

            DataList dataList = jsonManager.GetArray(jsonKey);

            string[] newUserList = new string[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList.TryGetValue(i, out DataToken name))
                {
                    newUserList[i] = name.ToString();
                }
            }

            userList = newUserList;

            _SetUserDict(newUserList);

            _DebugLog($"Updated user list from world state. Total users: {userList.Length}");
        }

         public override bool _ContainsName(string name)
        {
            _EnsureInit();

            return userDict.ContainsKey(name);
        }

        private void _DebugLog(string message)
        {
            if (debugLogging)
                Debug.Log("[SerenAccess] " + message);
            if (debugLog)
                debugLog._Write("SerenAccess", message);
        }

    }
}
