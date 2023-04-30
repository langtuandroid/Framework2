using System;
using System.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 需要注意的是，每个带有UnityCollider2D的UnityGo本身Transform的X,Z不能有偏移, 因为对于Offset我们读取的是UnityCollider2D Offset，而不是Go的
    /// </summary>
    public class B2S_ColliderEditor : OdinEditorWindow
    {
        [TabGroup("Special", "管理")] public B2S_MenuTree MenuTree;

        [LabelText("画线管理者")] [TabGroup("Special", "编辑")]
        public B2S_DebuggerHandler MB2SDebuggerHandler;

        [TabGroup("Special", "编辑")] [OnValueChanged("OnSelectedObjChanged")]
        public GameObject ColliderObj;

        private string curColliderName;

        [HideLabel] [ShowIf("@curColliderName == \"MB2SBoxColliderVisualHelper\"")] [TabGroup("Special", "编辑")]
        public B2S_BoxColliderVisualHelper MB2SBoxColliderVisualHelper;

        [HideLabel]
        [ShowIf("@curColliderName == \"MB2SCircleColliderVisualHelper\"")]
        [TabGroup("Special", "编辑")]
        public B2S_CircleColliderVisualHelper MB2SCircleColliderVisualHelper;

        [HideLabel]
        [ShowIf("@curColliderName == \"MB2SPolygonColliderVisualHelper\"")]
        [TabGroup("Special", "编辑")]
        public B2S_PolygonColliderVisualHelper MB2SPolygonColliderVisualHelper;

        [HideInInspector] public ColliderNameAndIdInflectSupporter ColliderNameAndIdInflectSupporter =
            new ColliderNameAndIdInflectSupporter();

        [HideInInspector] public ColliderDataSupporter ColliderDataSupporter = new ColliderDataSupporter();

        [MenuItem("Tools/Box2D")]
        private static void OpenWindowCCC()
        {
            var window = GetWindow<B2S_ColliderEditor>();

            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("Box2D可视化编辑器");
        }

        private void OnEnable()
        {
            this.ReadcolliderNameAndIdInflect();
            this.ReadcolliderData();
            this.MB2SDebuggerHandler = new GameObject("Box2DDebuggerHandler").AddComponent<B2S_DebuggerHandler>();

            MenuTree = new B2S_MenuTree(ColliderNameAndIdInflectSupporter, LoadOneData);
            this.MB2SBoxColliderVisualHelper =
                new B2S_BoxColliderVisualHelper(this);
            this.MB2SCircleColliderVisualHelper =
                new B2S_CircleColliderVisualHelper(this);
            this.MB2SPolygonColliderVisualHelper =
                new B2S_PolygonColliderVisualHelper(this);

            this.MB2SBoxColliderVisualHelper.InitColliderBaseInfo();
            this.MB2SCircleColliderVisualHelper.InitColliderBaseInfo();
            this.MB2SPolygonColliderVisualHelper.InitColliderBaseInfo();

            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SBoxColliderVisualHelper);
            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SCircleColliderVisualHelper);
            this.MB2SDebuggerHandler.MB2SColliderVisualHelpers.Add(this.MB2SPolygonColliderVisualHelper);
            EditorApplication.update += this.MB2SDebuggerHandler.OnUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= this.MB2SDebuggerHandler.OnUpdate;
            MB2SDebuggerHandler.CleanCollider();
            if (MB2SDebuggerHandler != null)
            {
                UnityEngine.Object.DestroyImmediate(MB2SDebuggerHandler.gameObject);
            }

            this.MB2SDebuggerHandler = null;
            this.MB2SBoxColliderVisualHelper = null;
            this.MB2SCircleColliderVisualHelper = null;
            this.MB2SPolygonColliderVisualHelper = null;
        }

        private void LoadOneData(string name)
        {
            var go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(
                $"{B2S_BattleColliderExportPathDefine.ColliderPrefabSavePath}/{name}.prefab")) as GameObject;
            ColliderObj = go;
            RefreshCollider();
        }

        /// <summary>
        /// 读取碰撞名称和ID映射表
        /// </summary>
        private void ReadcolliderNameAndIdInflect()
        {
            if (File.Exists(
                    $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/ColliderNameAndIdInflect.bytes"))
            {
                byte[] mfile0 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/ColliderNameAndIdInflect.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
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
                MB2SBoxColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
                curColliderName = nameof(MB2SBoxColliderVisualHelper);
                return;
            }

            var circle = ColliderObj.GetComponent<CircleCollider2D>();
            if (circle != null)
            {
                MB2SCircleColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
                curColliderName = nameof(MB2SCircleColliderVisualHelper);
                return;
            }

            var pol = ColliderObj.GetComponent<PolygonCollider2D>();
            if (pol != null)
            {
                MB2SPolygonColliderVisualHelper.theObjectWillBeEdited = ColliderObj;
                curColliderName = nameof(MB2SPolygonColliderVisualHelper);
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
        private void ReadcolliderData()
        {
            Type[] types = typeof(ColliderDataSupporter).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(B2S_ColliderDataStructureBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            if (File.Exists(
                    $"{B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath}/ColliderData.bytes"))
            {
                byte[] mfile0 =
                    File.ReadAllBytes(
                        $"{B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath}/ColliderData.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
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
}