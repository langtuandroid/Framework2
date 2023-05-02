using System.Collections.Generic;
using System.Reflection;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

[HideReferenceObjectPicker]
public class VTD_CollisionId
{
    [ValueDropdown("GetEventId")] public int Value;

#if UNITY_EDITOR
    private static B2D_CollisionRelationConfigFactory factory;
    
    private IEnumerable<int> GetEventId()
    {
        if (factory == null)
        {
            var bytes = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(typeof(B2D_CollisionRelationConfigFactory)
                .GetCustomAttribute<ConfigAttribute>().Path).text;
            factory = SerializeHelper.Deserialize<B2D_CollisionRelationConfigFactory>(bytes);
        }

        if (factory != null)
        {
            return factory.GetAll().Keys;
        }

        return null;
    }
#endif
}