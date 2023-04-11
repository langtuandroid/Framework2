using Framework;
using UnityEngine;

public class Item_SkillComponent
{
    private GameObject go;
    public Item_SkillComponent(GameObject go)
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
                    text = go.transform.Find("CostTxt").GetComponent<Framework.CustomText>();
                }
                return text;
            }
        }

        private Framework.CustomButton item_Skill;
        public Framework.CustomButton Item_Skill
        {
            get
            {
                if (item_Skill == null)
                {
                    item_Skill = go.transform.Find("").GetComponent<Framework.CustomButton>();
                }
                return item_Skill;
            }
        }

#endregion 
}

[UI("Assets/Res/UI/Item_Skill.prefab",true,true)]
public class Item_Skill : View
{
    private Item_SkillVM vm;
    private Item_SkillComponent components;
    
    protected override void Start()
    {
        base.Start();
        components = new(Go);
    }
    
    protected override void OnVmChange()
    {
        vm = ViewModel as Item_SkillVM;
    }
     
    public override UILevel UILevel { get; } = UILevel.Common;
    
}