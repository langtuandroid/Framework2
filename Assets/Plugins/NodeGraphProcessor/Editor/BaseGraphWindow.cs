using System;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    [Serializable]
    public abstract class BaseGraphWindow : EditorWindow
    {
	    /// <summary>
	    ///     上一帧记录时间点
	    /// </summary>
	    public static double LastTimePoint;

	    /// <summary>
	    ///     加载Views单次Tick最长耗时，单位为ms，用于分帧加载
	    /// </summary>
	    public static double LoadViewsMaxLimitTime = 33;

        [SerializeField] protected BaseGraph graph;

        private readonly string graphWindowStyle = "GraphProcessorStyles/BaseGraphView";
        protected BaseGraphView graphView;

        private bool reloadWorkaround;
        protected VisualElement rootView;

        public bool isGraphLoaded => graphView != null && graphView.graph != null;

        protected virtual void Update()
        {
            // Workaround for the Refresh option of the editor window:
            // When Refresh is clicked, OnEnable is called before the serialized data in the
            // editor window is deserialized, causing the graph view to not be loaded
            if (reloadWorkaround && graph != null)
            {
                InitializeGraph(graph);
                reloadWorkaround = false;
            }

            LastTimePoint = EditorApplication.timeSinceStartup;
        }

        /// <summary>
        ///     Called by Unity when the window is enabled / opened
        ///     只会在EditorWindow初次打开/重新编译/进入PlayMode的时候才会执行一次
        /// </summary>
        protected virtual void OnEnable()
        {
            InitializeRootView();

            graphLoaded = baseGraph => { baseGraph?.OnGraphEnable(); };
            graphUnloaded = baseGraph => { baseGraph?.OnGraphDisable(); };
            //注意，一定不能在EditorWindow的OnEnable中进行一些序列化相关的操作，因为执行完OnEnable后Unity内部就会进行GC（依照Unity的规则）
            //所以这里GraphView相关数据的操作就统一放到OnEnable之后去执行，防止一些数据刚组装好，就直接GC了
            reloadWorkaround = true;
        }

        /// <summary>
        ///     Called by Unity when the window is disabled (happens on domain reload)
        /// </summary>
        protected virtual void OnDisable()
        {
            if (graph != null && graphView != null)
            {
                graphView.SaveGraphToDisk();
                // Unload the graph
                graphUnloaded?.Invoke(graph);
            }
        }

        /// <summary>
        ///     Called by Unity when the window is closed
        /// </summary>
        protected virtual void OnDestroy()
        {
            graphView?.Dispose();
        }

        public event Action<BaseGraph> graphLoaded;
        public event Action<BaseGraph> graphUnloaded;

        private void InitializeRootView()
        {
            rootView = rootVisualElement;

            rootView.name = "graphRootView";

            rootView.styleSheets.Add(Resources.Load<StyleSheet>(graphWindowStyle));
        }

        /// <summary>
        ///     只有两种情况会调用到这个函数
        ///     1.打开GraphEditorWindow（初次打开或者在已经打开的基础上更换GraphAsset）
        ///     2.编译/进入PlayMode而导致的GraphEditorWindow重载
        /// </summary>
        /// <param name="graph"></param>
        public void InitializeGraph(BaseGraph graph)
        {
            if (this.graph != null && graph != this.graph)
            {
                // Save the graph to the disk
                GraphCreateAndSaveHelper.SaveGraphToDisk(this.graph);
                // Unload the graph
                graphUnloaded?.Invoke(this.graph);
            }

            graphLoaded?.Invoke(graph);
            this.graph = graph;

            if (graphView != null) rootView.Remove(graphView);

            InitializeWindow(graph);
            rootView.Add(graphView);

            graphView = rootView.Children().FirstOrDefault(e => e is BaseGraphView) as BaseGraphView;

            if (graphView == null)
            {
                Debug.LogError("GraphView has not been added to the BaseGraph root view !");
                return;
            }

            EditorCoroutineUtility.StartCoroutine(graphView.Initialize(graph), this);

            InitializeGraphView(graphView);

            // TOOD: onSceneLinked...

            if (graph.IsLinkedToScene())
                LinkGraphWindowToScene(graph.GetLinkedScene());
            else
                graph.onSceneLinked += LinkGraphWindowToScene;
            //防止在外部调用InitializeGraph时重复执行InitializeGraph
            reloadWorkaround = false;
        }

        /// <summary>
        ///     主动刷新EditorWindow
        /// </summary>
        public void RefreshWindow()
        {
            reloadWorkaround = true;
        }

        private void LinkGraphWindowToScene(Scene scene)
        {
            EditorSceneManager.sceneClosed += CloseWindowWhenSceneIsClosed;

            void CloseWindowWhenSceneIsClosed(Scene closedScene)
            {
                if (scene == closedScene)
                {
                    Close();
                    EditorSceneManager.sceneClosed -= CloseWindowWhenSceneIsClosed;
                }
            }
        }

        public virtual void OnGraphDeleted()
        {
            if (graph != null && graphView != null)
                rootView.Remove(graphView);

            graphView = null;
        }

        /// <summary>
        ///     可根据BaseGraph初始化EditorWindow，以及根据BaseGraph自定义BaseGraphView
        /// </summary>
        /// <param name="graph"></param>
        protected abstract void InitializeWindow(BaseGraph graph);

        /// <summary>
        ///     BaseGraphView已初始化完毕，重写此函数可以进行后续的自定义操作
        /// </summary>
        /// <param name="view"></param>
        protected virtual void InitializeGraphView(BaseGraphView view)
        {
        }
    }
}