using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class KnapsackManager : MonoBehaviour
{

    private static KnapsackManager _instance;
    public static KnapsackManager Instance { get { return _instance; } }
    private Dictionary<int, Item> ItemList;
    public goodsUI goodsUI;

    public TooltipUI TooltipUI;
    private bool isShow = false;

    public DragItemUI DragItemUI;
    private bool isDrag = false;

    void Awake()
    {
        //单例
        _instance = this;
        Load();
        GridUI.OnEnter += GridUI_OnEnter;
        GridUI.OnExit += GridUI_OnExit;

        GridUI.OnLeftBeginDrag += GridUI_OnLeftBeginDrag;
        GridUI.OnLeftEndDrag += GridUI_OnLeftEndDrag;
    }

    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("KnapsackUI").transform as RectTransform, Input.mousePosition, null, out position);

        if (isDrag)
        {
            DragItemUI.Show();
            DragItemUI.SetLocalPosition(position);
        }

        if (isShow)
        {
            TooltipUI.Show();
            TooltipUI.SetLocalPosition(position);
        }
    }

    private void GridUI_OnEnter(Transform gridTransform)
    {
        Item item = ItemModel.GetItem(gridTransform.name);
        if (item == null)
        {
            return;
        }
        else
        {
            string text = GetTooltipText(item);
            TooltipUI.UpdateTooltip(text);
            isShow = true;
        }
    }
    private void GridUI_OnExit()
    {
        isShow = false;
        TooltipUI.Hide();
    }

    private void GridUI_OnLeftEndDrag(Transform prevTransform, Transform enterTransform)
    {
        isDrag = false;
        DragItemUI.Hide();
        if (enterTransform == null)
        {
            ItemModel.DeleteItem(prevTransform.name);
        }
        else if (enterTransform.tag == "Grid")
        {
            if (enterTransform.childCount == 0)
            {
                Item item = ItemModel.GetItem(prevTransform.name);
                this.CreateNewItem(item, enterTransform);
                ItemModel.DeleteItem(prevTransform.name);
            }
            else
            {
                //删除原来的物品
                Destroy(enterTransform.GetChild(0).gameObject);
                //获取数据
                Item prevGirdItem = ItemModel.GetItem(prevTransform.name);
                Item enterGirdItem = ItemModel.GetItem(enterTransform.name);
                //交换的两个物体
                this.CreateNewItem(prevGirdItem, enterTransform);
                this.CreateNewItem(enterGirdItem, prevTransform);
            }
        }
        else
        {
            Item item = ItemModel.GetItem(prevTransform.name);
            this.CreateNewItem(item, prevTransform);
        }
    }

    private void GridUI_OnLeftBeginDrag(Transform gridTransform)
    {
        if (gridTransform.childCount == 0) return;
        else
        {
            Item item = ItemModel.GetItem(gridTransform.name);
            Texture2D aa = (Texture2D)Resources.Load(item.Icon) as Texture2D;
            Sprite kk = Sprite.Create(aa, new Rect(0, 0, aa.width, aa.height), new Vector2(0.5f, 0.5f));
            DragItemUI.UpdateItem(kk);
            Destroy(gridTransform.GetChild(0).gameObject);
            isDrag = true;
        }
    }

    private void CreateNewItem(Item item, Transform parent)
    {
        GameObject itemPrefab = Resources.Load<GameObject>("prefabs/Item");
        Texture2D aa = (Texture2D)Resources.Load(item.Icon) as Texture2D;
        Sprite kk = Sprite.Create(aa, new Rect(0, 0, aa.width, aa.height), new Vector2(0.5f, 0.5f));
        itemPrefab.GetComponent<ItemUI>().UpdateItem(kk);
        GameObject itemGo = GameObject.Instantiate(itemPrefab);
        itemGo.transform.SetParent(parent);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        ItemModel.StoreItem(parent.name, item);
    }


    //存物品
    public void StoreItem(int itemid)
    {
        if (!ItemList.ContainsKey(itemid))
            return;
        Transform emptyGrid = goodsUI.GetEmptyGrid();
        if (emptyGrid == null)
        {
            Debug.LogWarning("背包已满");
            return;
        }
        Item temp = ItemList[itemid];
        GameObject itemPrefab = Resources.Load<GameObject>("prefabs/Item");
        Texture2D aa = (Texture2D)Resources.Load(temp.Icon) as Texture2D;
        Sprite kk = Sprite.Create(aa, new Rect(0, 0, aa.width, aa.height), new Vector2(0.5f, 0.5f));
        itemPrefab.GetComponent<ItemUI>().UpdateItem(kk);
        GameObject itemGo = GameObject.Instantiate(itemPrefab);
        itemGo.transform.SetParent(emptyGrid);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        ItemModel.StoreItem(emptyGrid.name, temp);
    }

    //取数据
    private void Load()
    {
        ItemList = new Dictionary<int, Item>();

        //武器
        Weapon w1 = new Weapon(0, "单手剑", "牛B的剑！", 20, 10, "sources/单手剑", 20);
        Weapon w2 = new Weapon(1, "盾牌", "牛B的护盾！", 15, 10, "sources/盾牌", 20);

        //耗材
        Consumable c1 = new Consumable(2, "治愈药水", "加血", 39, 19, "sources/血瓶", 20, 0);
        Consumable c2 = new Consumable(3, "魔法药水", "加蓝", 39, 19, "sources/蓝瓶", 0, 20);

        //盔甲
        Armor a1 = new Armor(4, "头盔", "保护脑袋！", 128, 83, "sources/头盔", 5, 20, 1);
        Armor a2 = new Armor(5, "护甲", "勇士护甲", 1000, 0, "sources/护甲", 15, 25, 11);
        Armor a3 = new Armor(6, "护腿", "轻便的护腿", 153, 0, "sources/护腿", 25, 30, 11);

        //增加武器
        ItemList.Add(w1.Id, w1);
        ItemList.Add(w2.Id, w2);

        //增加耗材
        ItemList.Add(c1.Id, c1);
        ItemList.Add(c2.Id, c2);

        //增加盔甲
        ItemList.Add(a1.Id, a1);
        ItemList.Add(a2.Id, a2);
        ItemList.Add(a3.Id, a3);
    }


    private string GetTooltipText(Item item)
    {
        if (item == null)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<color=red>{0}</color>\n\n", item.Name);
        switch (item.ItemType)
        {
            case "Armor":
                Armor armor = item as Armor;
                sb.AppendFormat("力量:{0}\n防御:{1}\n敏捷:{2}\n\n", armor.Power, armor.Defend, armor.Agility);
                break;
            case "Consumable":
                Consumable consumable = item as Consumable;
                sb.AppendFormat("HP:{0}\nMP:{1}\n\n", consumable.BackHp, consumable.BackMp);
                break;
            case "Weapon":
                Weapon weapon = item as Weapon;
                sb.AppendFormat("攻击:{0}\n\n", weapon.Damage);
                break;
            default:
                break;
        }
        sb.AppendFormat("<size=25><color=white>购买价格：{0}\n出售价格：{1}</color></size>\n\n<color=yellow><size=20>描述：{2}</size></color>", item.BuyPrice, item.SellPrice, item.Description);
        return sb.ToString();
    }
}
