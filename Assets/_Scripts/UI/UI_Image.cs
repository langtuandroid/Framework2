using Framework;
using UnityEngine;

public class UI_ImageComponent
{
    private GameObject go;
    public UI_ImageComponent(GameObject go)
    {
        this.go = go;
    }
   
    #region component

        private Framework.CustomButton button;
        public Framework.CustomButton Button
        {
            get
            {
                if (button == null)
                {
                    button = go.transform.Find("Button").GetComponent<Framework.CustomButton>();
                }
                return button;
            }
        }

        private UnityEngine.UI.Image self;
        public UnityEngine.UI.Image Self
        {
            get
            {
                if (self == null)
                {
                    self = go.transform.Find("").GetComponent<UnityEngine.UI.Image>();
                }
                return self;
            }
        }

#endregion 
}

[UI("Assets/UI_Image.prefab",true,true)]
public class UI_Image : View
{
    private UI_ImageVM vm;
    private UI_ImageComponent components;
    
    protected override void Start()
    {
        base.Start();
        components = new(Go);
    }
    
    protected override void OnVmChange()
    {
        vm = ViewModel as UI_ImageVM;
    }
     
    public override UILevel UILevel { get; } = UILevel.Common;
    
}