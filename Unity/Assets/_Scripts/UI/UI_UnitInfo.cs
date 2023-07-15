using Framework;
using UnityEngine;

public class UI_UnitInfoComponent
{
    private GameObject go;
    public UI_UnitInfoComponent(GameObject go)
    {
        this.go = go;
    }
   
    #region component

        private Framework.CustomTextMeshPro textHp;
        public Framework.CustomTextMeshPro TextHp
        {
            get
            {
                if (textHp == null)
                {
                    textHp = go.transform.Find("Image/TextHp").GetComponent<Framework.CustomTextMeshPro>();
                }
                return textHp;
            }
        }

        private UnityEngine.RectTransform skillContent;
        public UnityEngine.RectTransform SkillContent
        {
            get
            {
                if (skillContent == null)
                {
                    skillContent = go.transform.Find("Scroll View (1)/Viewport/Content").GetComponent<UnityEngine.RectTransform>();
                }
                return skillContent;
            }
        }

        private UnityEngine.RectTransform buffContent;
        public UnityEngine.RectTransform BuffContent
        {
            get
            {
                if (buffContent == null)
                {
                    buffContent = go.transform.Find("Scroll View (2)/Viewport/Content").GetComponent<UnityEngine.RectTransform>();
                }
                return buffContent;
            }
        }

        private Framework.CustomButton close;
        public Framework.CustomButton Close
        {
            get
            {
                if (close == null)
                {
                    close = go.transform.Find("Close").GetComponent<Framework.CustomButton>();
                }
                return close;
            }
        }

#endregion 
}

[UI("Assets/Res/UI/UI_UnitInfo.prefab",true,true)]
public class UI_UnitInfo : View
{
    private UI_UnitInfoVM vm;
    private UI_UnitInfoComponent components;
    
    protected override void Start()
    {
        base.Start();
        components = new(Go);
    }
    
    protected override void OnVmChange()
    {
        vm = ViewModel as UI_UnitInfoVM;
        Binding.BindCommand(components.Close, Close);
        Binding.BindData(vm.MaxHp, vm.CurHp, (max, cur) =>
        {
            components.TextHp.text = $"HP:{cur.ToString()}/{max.ToString()}";
        } );
    }
    
    public override UILevel UILevel { get; } = UILevel.Common;
    
}