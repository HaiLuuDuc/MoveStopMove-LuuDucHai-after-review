using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class PlayerWearSkinItems : CharacterWearSkinItems
{
    [SerializeField] private Player player;
    public GameObject currentHat;
    private GameObject currentShield;
    private GameObject currentWing;
    private GameObject currentLeftHandObject;
    private GameObject currentTail;

    public override GameObject WearHat(int index)
    {
        DestroyCurrentHat();
        currentHat = base.WearHat(index);
        return null;
    }

    public void DestroyCurrentHat()
    {
        if (currentHat != null)
        {
            Destroy(currentHat);
        }
    }

    public override void WearPants(int index)
    {
        if (index >= 0)
        {
            pants.material = Inventory.Instance.pants[index];
        }
    }

    public void DestroyCurrentPants()
    {
        pants.material = Colors.instance.transparent100;
    }

    public override GameObject WearShield(int index)
    {
        DestroyCurrentShield();
        currentShield = base.WearShield(index);
        return null;
    }

    public void DestroyCurrentShield()
    {
        if (currentShield != null)
        {
            Destroy(currentShield);
        }
    }

    public void WearFullSet(int index)
    {
        DestroyCurrentFullSet();
        player.SetSkinnedMeshRenderer(Inventory.Instance.fullSetDatas[(int)index].materialType);
        DataManager.ins.playerData.currentBodyMatType = Inventory.Instance.fullSetDatas[(int)index].materialType;
        player.SetCurrentBodyMatType(Inventory.Instance.fullSetDatas[(int)index].materialType);
        if (Inventory.Instance.fullSetDatas[index].head != null)
        {
            currentHat = Instantiate(Inventory.Instance.fullSetDatas[index].head);
            Cache.GetTransform(currentHat.gameObject).SetParent(dinhdau);
            ZeroTheChild(currentHat);
        }
        if (Inventory.Instance.fullSetDatas[index].wing != null)
        {
            currentWing = Instantiate(Inventory.Instance.fullSetDatas[index].wing);
            Cache.GetTransform(currentWing.gameObject).SetParent(lung);
            ZeroTheChild(currentWing);
        }
        if (Inventory.Instance.fullSetDatas[index].leftHandObject != null)
        {
            currentLeftHandObject = Instantiate(Inventory.Instance.fullSetDatas[index].leftHandObject);
            Cache.GetTransform(currentLeftHandObject).SetParent(leftHand2);
            ZeroTheChild(currentLeftHandObject);
        }
        if (Inventory.Instance.fullSetDatas[index].tail != null)
        {
            currentTail = Instantiate(Inventory.Instance.fullSetDatas[index].tail);
            Cache.GetTransform(currentTail.gameObject).SetParent(tail);
            ZeroTheChild(currentTail);
        }
    }

    public void DestroyCurrentFullSet()
    {
        player.SetSkinnedMeshRenderer(MaterialType.White);
        player.SetCurrentBodyMatType(MaterialType.White);
        DataManager.ins.playerData.currentBodyMatType = MaterialType.White;
        if (currentHat != null)
        {
            Destroy(currentHat);
        }
        if (currentWing != null)
        {
            Destroy(currentWing);
        }
        if (currentLeftHandObject != null)
        {
            Destroy(currentLeftHandObject);
        }
        if(currentTail != null)
        {
            Destroy(currentTail);
        }
    }

    public void DestroyAllItemsOnBody()
    {
        DestroyCurrentHat();
        DestroyCurrentPants();
        DestroyCurrentShield();
        DestroyCurrentFullSet();
    }

    public void ZeroTheChild(GameObject obj)
    {
        Cache.GetTransform(obj).localPosition = Vector3.zero;
        Cache.GetTransform(obj).localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void PutOnItemsAtFirst()//itemControllers need to be stored outside canvas
                            //solution: sẽ lấy đồ từ scriptable object chứ không lấy từ các itemcontroller
    {
        //wear fullset
        if (DataManager.ins.playerData.usingItemIndexs[3] >= 0)
        {
            WearFullSet(DataManager.ins.playerData.usingItemIndexs[3]);
        }
        else
        {
            DestroyCurrentFullSet();
        }
        //test fullset:

        //wear hat
        if (DataManager.ins.playerData.usingItemIndexs[0] >= 0)//nếu đã mua mũ rồi thì dùng cái mũ đó
        {
            WearHat(DataManager.ins.playerData.usingItemIndexs[0]);
        }
        else//nếu chưa mua cái nào mà chỉ đang thử thì sẽ phải trả lại mũ đang thử cho shop
        {
            if (DataManager.ins.playerData.usingItemIndexs[3] < 0)//tranh truong hop destroy hat cua fullset
                DestroyCurrentHat();
        }
        //wear pants
        if (DataManager.ins.playerData.usingItemIndexs[1] >= 0)
        {
            WearPants(DataManager.ins.playerData.usingItemIndexs[1]);
        }
        else
        {
            DestroyCurrentPants();
        }
        //wear shield
        if (DataManager.ins.playerData.usingItemIndexs[2] >= 0)
        {
            WearShield(DataManager.ins.playerData.usingItemIndexs[2]);
        }
        else
        {
            DestroyCurrentShield();
        }

    }

    public void PutOnItemsWhenCloseShop()//itemControllers need to be stored outside canvas
        //solution: sẽ lấy đồ từ scriptable object chứ không lấy từ các itemcontroller
    {
        //wear fullset
        ItemController fullsetController = SkinShopManager.instance.itemControllers[(int)ItemType.FullSet];
        if (fullsetController.usingIndex >= 0)
        {
            WearFullSet(fullsetController.usingIndex);
        }
        else
        {
            DestroyCurrentFullSet();
        }
        //test fullset:

        //wear hat
        ItemController hatController = SkinShopManager.instance.itemControllers[(int)ItemType.Hat];
        if (hatController.usingIndex >= 0)//nếu đã mua mũ rồi thì dùng cái mũ đó
        {
            WearHat(hatController.usingIndex);
        }
        else//nếu chưa mua cái nào mà chỉ đang thử thì sẽ phải trả lại mũ đang thử cho shop
        {
            if (SkinShopManager.instance.itemControllers[(int)ItemType.FullSet].usingIndex < 0)//tranh truong hop destroy hat cua fullset
                DestroyCurrentHat();
        }
        //wear pants
        ItemController pantsController = SkinShopManager.instance.itemControllers[(int)ItemType.Pants];
        if (pantsController.usingIndex >= 0)
        {
            WearPants(pantsController.usingIndex);
        }
        else
        {
            DestroyCurrentPants();
        }
        //wear shield
        ItemController shieldController = SkinShopManager.instance.itemControllers[(int)ItemType.Shield];
        if (shieldController.usingIndex >= 0)
        {
            WearShield(shieldController.usingIndex);
        }
        else
        {
            DestroyCurrentShield();
        }

    }

}
