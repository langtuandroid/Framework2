using Framework;
using UnityEngine;

public class Item_BuffComponent
{
    private GameObject go;
    public Item_BuffComponent(GameObject go)
    {
        this.go = go;
    }
   
    #region component

        private Framework.CustomText text;
        public Framework.CustomText Text
        {
            get
            {
                if (text == null)
                {
                    text = go.transform.Find("Overlay").GetComponent<Framework.CustomText>();
                }
                return text;
            }
        }

#endregion 
}

[UI("Assets/Res/UI/Item_Buff.prefab",true,true)]
public class Item_Buff : View
{
    private Item_BuffVM vm;
    private Item_BuffComponent components;
    
    protected override void Start()
    {
        base.Start();
        components = new(Go);
    }
    
    protected override void OnVmChange()
    {
        vm = ViewModel as Item_BuffVM;
    }
     
    public override UILevel UILevel { get; } = UILevel.Common;
    
}