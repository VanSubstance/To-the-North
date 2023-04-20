using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventorySlotController[,] inventorySlots = new InventorySlotController[10, 12];
    public static InventorySlotController[,] rootSlots = new InventorySlotController[10, 12];
    public static List<ItemBaseInfo> items = new List<ItemBaseInfo>();
    public static List<ItemBaseInfo> rootItems = new List<ItemBaseInfo>();
    public static Transform leftInventoryTF = null;
    public static Transform rightInventoryTF = null;
    public static Transform movingSpaceTF = null;

    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    void Awake()
    {
    }
    void Start()
    {
    }
    void Update()
    {

    }
}
