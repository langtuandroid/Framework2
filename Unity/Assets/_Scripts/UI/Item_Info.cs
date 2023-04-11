using Framework;
using UnityEngine;

public class Item_InfoComponent
{
    private GameObject go;
    public Item_InfoComponent(GameObject go)
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
                    text = go.transform.Find("Text").GetComponent<Framework.CustomText>();
                }
                return text;
            }
        }

#endregion 
}

[UI("Assets/Res/UI/Item_Info.prefab",true,true)]
public class Item_Info : View
{
    private Item_InfoVM vm;
    private Item_InfoComponent components;
    
    protected override void Start()
    {
        base.Start();
        components = new(Go);
    }
    
    protected override void OnVmChange()
    {
        vm = ViewModel as Item_InfoVM;
    }
     
    public override UILevel UILevel { get; } = UILevel.Common;
    
}