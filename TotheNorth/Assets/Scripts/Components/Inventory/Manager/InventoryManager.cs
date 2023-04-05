using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager Instance = null;

    public static InventorySlotController[,] inventorySlots = new InventorySlotController[10, 13];
    public static InventorySlotController[,] rootSlots = new InventorySlotController[10, 13];
    public static List<ItemBaseInfo> items = new List<ItemBaseInfo>();
    public static List<ItemBaseInfo> rootItems = new List<ItemBaseInfo>();
    public static Transform leftInventoryTF = null;
    public static Transform rightInventoryTF = null;
    public static Transform movingSpaeceTF = null;
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
    }
    void Update()
    {

    }
}
