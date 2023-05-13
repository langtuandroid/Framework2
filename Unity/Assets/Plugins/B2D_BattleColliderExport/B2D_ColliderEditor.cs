using System;
using System.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 需要注意的是，每个带有UnityCollider2D的UnityGo本身Transform的X,Z不能有偏移, 因为对于Offset我们读取的是UnityCollider2D Offset，而不是Go的
/// </summary>
public class B2D_ColliderEditor : OdinEditorWindow
{
    [TabGroup("Special", "管理")] public B2D_MenuTree MenuTree;

    [LabelText("画线管理者")] [TabGroup("Special", "编辑")]
    public B2D_DebuggerHandler MB2DDebuggerHandler;

    [TabGroup("Special", "编辑")] [OnValueChanged("OnSelectedObjChanged")]
    public GameObject ColliderObj;

    private string curColliderName;

    [HideLabel] [ShowIf("@curColliderName == \"MB2DBoxColliderVisualHelper\"")] [TabGroup("Special", "编辑")]
    public B2D_BoxColliderVisualHelper MB2DBoxColliderVisualHelper;

    [HideLabel] [ShowIf("@curColliderName == \"MB2DCircleColliderVisualHelper\"")] [TabGroup("Special", "编辑")]
    public B2D_CircleColliderVisualHelper MB2DCircleColliderVisualHelper;

    [HideLabel] [ShowIf("@curColliderName == \"MB2DPolygonColliderVisualHelper\"")] [TabGroup("Special", "编辑")]
    public B2D_PolygonColliderVisualHelper MB2DPolygonColliderVisualHelper;

    [HideInInspector] public ColliderNameAndIdInflectSupporter ColliderNameAndIdInflectSupporter =
        new ColliderNameAndIdInflectSupporter();

    [HideInInspector] public ColliderDataSupporter ColliderDataSupporter = new ColliderDataSupporter();

    [MenuItem("Tools/Box2D")]
    private static void OpenWindowCCC()
    {
        var window = GetWindow<B2D_ColliderEditor>();

        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
        window.titleContent = new GUIContent("Box2D可视化编辑器");
    }

    private void OnEnable()
    {
        this.ReadColliderNameAndIdInflect();
        this.ReadColliderData();
        this.MB2DDebuggerHandler = new GameObject("Box2DDebuggerHandler").AddComponent<B2D_DebuggerHandler>();

        MenuTree = new B2D_MenuTree(ColliderNameAndIdInflectSupporter, LoadOneData);
        this.MB2DBoxColliderVisualHelper =
            new B2D_BoxColliderVisualHelper(this);
        this.MB2DCircleColliderVisualHelper =
            new B2D_CircleColliderVisualHelper(this);
        this.MB2DPolygonColliderVisualHelper =
            new B2D_PolygonColliderVisualHelper(this);

        this.MB2DBoxColliderVisualHelper.InitColliderBaseInfo();
        this.MB2DCircleColliderVisualHelper.InitColliderBaseInfo();
        this.MB2DPolygonColliderVisualHelper.InitColliderBaseInfo();

        this.MB2DDebuggerHandler.MB2DColliderVisualHelpers.Add(this.MB2DBoxColliderVisualHelper);
        this.MB2DDebuggerHandler.MB2DColliderVisualHelpers.Add(this.MB2DCircleColliderVisualHelper);
        this.MB2DDebuggerHandler.MB2DColliderVisualHelpers.Add(this.MB2DPolygonColliderVisualHelper);
        EditorApplication.update += this.MB2DDebuggerHandler.OnUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= this.MB2DDebuggerHandler.OnUpdate;
        MB2DDebuggerHandler.CleanCollider();
        if (MB2DDebuggerHandler != null)
        {
            UnityEngine.Object.DestroyImmediate(MB2DDebuggerHandler.gameObject);
        }

        this.MB2DDebuggerHandler = null;
        this.MB2DBoxColliderVisualHelper = null;
        this.MB2DCircleColliderVisualHelper = null;
        this.MB2DPolygonColliderVisualHelper = null;
    }

    private void LoadOneData(string name)
    {
        var go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(
            $"{B2D_BattleColliderExportPathDefine.ColliderPrefabSavePath}/{name}.prefab")) as GameObject;
        ColliderObj = go;
        RefreshCollider();
    }

    /// <summary>
    /// 读取碰撞名称和ID映射表
    /// </summary>
    private void ReadColliderNameAndIdInflect()
    {
        if (File.Exists(
                $"{B2D_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}"))
        {
            var mfile0 =
                File.ReadAllText($"{B2D_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}");
            if (mfile0.Length > 0)
                this.ColliderNameAndIdInflectSupporter =
                    BsonSerializer.Deserialize<ColliderNameAndIdInflectSupporter>(mfile0);
        }
    }

    private void OnSelectedObjChanged()
    {
        if (ColliderObj == null)
        {
            curColliderName = String.Empty;
            return;
        }

        if (MenuTree.ContainsName(ColliderObj.name))
        {
            ShowTips("名字重复！！");
            ColliderObj = null;
            curColliderName = String.Empty;
            return;
        }

        RefreshCollider();
    }

    private void RefreshCollider()
    {
        if (ColliderObj == null)
        {
            curColliderName = String.Empty;
            return;
        }

        var box = ColliderObj.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            MB2DBoxColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
            curColliderName = nameof(MB2DBoxColliderVisualHelper);
            return;
        }

        var circle = ColliderObj.GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            MB2DCircleColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
            curColliderName = nameof(MB2DCircleColliderVisualHelper);
            return;
        }

        var pol = ColliderObj.GetComponent<PolygonCollider2D>();
        if (pol != null)
        {
            MB2DPolygonColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
            curColliderName = nameof(MB2DPolygonColliderVisualHelper);
        }
    }

    public void OnDeleteColliderData(long id)
    {
        ColliderObj = null;
        RefreshCollider();
        MenuTree.RemoveId(id);
    }

    public void OnSaveColliderData(long id)
    {
        MenuTree.RefreshData();
    }

    /// <summary>
    /// 读取所有碰撞数据
    /// </summary>
    private void ReadColliderData()
    {
        Type[] types = typeof(ColliderDataSupporter).Assembly.GetTypes();
        foreach (Type type in types)
        {
            if (!type.IsSubclassOf(typeof(B2D_ColliderDataStructureBase)))
            {
                continue;
            }

            BsonClassMap.LookupClassMap(type);
        }

        if (File.Exists(
                $"{B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath}"))
        {
            var mfile0 =
                File.ReadAllText(
                    $"{B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath}");
            if (mfile0.Length > 0)
                this.ColliderDataSupporter =
                    BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
        }
    }

    public void ShowTips(string msg)
    {
        ShowNotification(new GUIContent(msg));
    }
}