using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Framework
{
    [HideReferenceObjectPicker]
    public struct VTD_EventId
    {
        [ValueDropdown("GetEventId")]
        public string Value;

#if UNITY_EDITOR
        private IEnumerable<string> GetEventId()
        {
            if (NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager != null)
            {
                return NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.EventValues;
            }

            return null;
        }
#endif
    }
}