#if UNITY_EDITOR

using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 矩形可视化辅助
/// </summary>
public class B2D_BoxColliderVisualHelper : B2D_ColliderVisualHelperBase
{
    [InlineEditor] [DisableInEditorMode] [Required("需要至少一个Unity2D矩形碰撞器")] [BsonIgnore]
    public BoxCollider2D mCollider2D;

    [LabelText("碰撞体数据")]
    [ShowInInspector]
    public B2D_BoxColliderDataStructure MB2D_BoxColliderDataStructure
    {
        get => dataStructureBase as B2D_BoxColliderDataStructure;
        set => dataStructureBase = value;
    }

    private Vector3[] Points = new Vector3[4];

    public override void InitColliderBaseInfo()
    {
        this.MB2D_BoxColliderDataStructure.B2D_ColliderType = B2D_ColliderType.BoxColllider;
    }

    [Button("重新绘制矩形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
    public override void InitPointInfo()
    {
        BoxCollider2D tempBox2D = this.mCollider2D;
        // 这里不需要再去根据Go的缩放计算了，因为Unity已经帮我们计算好了
        this.MB2D_BoxColliderDataStructure.hx = tempBox2D.bounds.size.x / 2;
        this.MB2D_BoxColliderDataStructure.hy = tempBox2D.bounds.size.y / 2;

        MB2D_BoxColliderDataStructure.finalOffset = mCollider2D.offset;

        // 从左上角开始顺时针计算顶点
        Points[0] = new Vector3(
            -MB2D_BoxColliderDataStructure.hx + MB2D_BoxColliderDataStructure.finalOffset.x, 0,
            MB2D_BoxColliderDataStructure.hy + MB2D_BoxColliderDataStructure.finalOffset.y);
        Points[1] = new Vector3(
            MB2D_BoxColliderDataStructure.hx + MB2D_BoxColliderDataStructure.finalOffset.x, 0,
            MB2D_BoxColliderDataStructure.hy + MB2D_BoxColliderDataStructure.finalOffset.y);
        Points[2] = new Vector3(
            MB2D_BoxColliderDataStructure.hx + MB2D_BoxColliderDataStructure.finalOffset.x, 0,
            -MB2D_BoxColliderDataStructure.hy + MB2D_BoxColliderDataStructure.finalOffset.y);
        Points[3] = new Vector3(
            -MB2D_BoxColliderDataStructure.hx + MB2D_BoxColliderDataStructure.finalOffset.x, 0,
            -MB2D_BoxColliderDataStructure.hy + MB2D_BoxColliderDataStructure.finalOffset.y);

        this.canDraw = true;
    }

    public override void DrawCollider()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(Points[i], Points[(i + 1) % 4]);
        }
    }

    public override void FillDataStructure()
    {
        if (mCollider2D == null) return;
        this.MB2D_BoxColliderDataStructure.hx = mCollider2D.bounds.size.x / 2;
        this.MB2D_BoxColliderDataStructure.hy = mCollider2D.bounds.size.y / 2;
        MB2D_BoxColliderDataStructure.finalOffset = mCollider2D.offset;
        MB2D_BoxColliderDataStructure.isSensor = mCollider2D.isTrigger;
    }

    [Button("保存矩形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
    public override void SaveColliderData()
    {
        var objTrans = theObjectWillBeEdited.transform;
        if (objTrans.position != Vector3.zero || objTrans.eulerAngles != Vector3.zero ||
            objTrans.localScale != Vector3.one)
        {
            colliderEditor.ShowTips("物体的旋转位移缩放不是默认值");
            return;
        }

        SavePrefab();
        if (this.theObjectWillBeEdited != null && this.mCollider2D != null && MB2D_BoxColliderDataStructure.id != 0)
        {
            if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2D_BoxColliderDataStructure.id))
            {
                B2D_BoxColliderDataStructure b2DBoxColliderDataStructure = new B2D_BoxColliderDataStructure();
                b2DBoxColliderDataStructure.id = MB2D_BoxColliderDataStructure.id;
                b2DBoxColliderDataStructure.finalOffset.x = MB2D_BoxColliderDataStructure.finalOffset.x;
                b2DBoxColliderDataStructure.finalOffset.y = MB2D_BoxColliderDataStructure.finalOffset.y;
                b2DBoxColliderDataStructure.isSensor = MB2D_BoxColliderDataStructure.isSensor;
                b2DBoxColliderDataStructure.B2D_ColliderType = MB2D_BoxColliderDataStructure.B2D_ColliderType;
                b2DBoxColliderDataStructure.hx = MB2D_BoxColliderDataStructure.hx;
                b2DBoxColliderDataStructure.hy = this.MB2D_BoxColliderDataStructure.hy;
                this.MColliderDataSupporter.colliderDataDic.Add(this.MB2D_BoxColliderDataStructure.id,
                    b2DBoxColliderDataStructure);
            }
            else
            {
                this.MColliderDataSupporter.colliderDataDic[this.MB2D_BoxColliderDataStructure.id] =
                    this.MB2D_BoxColliderDataStructure;
            }
        }

        if (SavecolliderNameAndIdInflect())
        {
            using (FileStream file =
                   File.Create(
                       $"{B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath}"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }

            colliderEditor.OnSaveColliderData(dataStructureBase.id);
            colliderEditor.ShowTips("保存成功");
        }
    }

    [Button("清除此矩形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
    public override void DeletecolliderData()
    {
        if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
        {
            base.DeletecolliderData();
        }
    }

    public override void OnUpdate()
    {
        if (CachedGameObject != theObjectWillBeEdited)
        {
            if (theObjectWillBeEdited != null)
                CachedGameObject = theObjectWillBeEdited;
            ResetData();
            return;
        }

        if (theObjectWillBeEdited == null)
        {
            ResetData();
            return;
        }

        if (mCollider2D == null)
        {
            mCollider2D = theObjectWillBeEdited.GetComponent<BoxCollider2D>();
            if (mCollider2D == null)
            {
                this.canDraw = false;
            }
        }

        FillDataStructure();

        if (this.MB2D_BoxColliderDataStructure.id == 0)
        {
            this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                this.theObjectWillBeEdited.name,
                out this.MB2D_BoxColliderDataStructure.id);
            if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2D_BoxColliderDataStructure.id))
            {
                this.MB2D_BoxColliderDataStructure =
                    (B2D_BoxColliderDataStructure)this.MColliderDataSupporter.colliderDataDic[
                        this.MB2D_BoxColliderDataStructure.id];
            }
        }
    }

    private void ResetData()
    {
        mCollider2D = null;
        this.canDraw = false;
        this.MB2D_BoxColliderDataStructure.id = 0;
        this.MB2D_BoxColliderDataStructure.isSensor = false;
        MB2D_BoxColliderDataStructure.finalOffset = new float2(0);
    }


    public B2D_BoxColliderVisualHelper(B2D_ColliderEditor colliderEditor) : base(colliderEditor)
    {
        dataStructureBase = new B2D_BoxColliderDataStructure();
    }
}

#endif