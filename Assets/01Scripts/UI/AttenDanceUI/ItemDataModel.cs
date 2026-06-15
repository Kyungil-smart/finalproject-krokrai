
using System;
using UnityEngine;
using System.Collections.Generic;

public class ItemDataModel : MonoBehaviour
{
    [SerializeField] private AutoSOGen_ContaineSO _itemTableSO;
    private Dictionary<int, Item_TableSO> _itemTables = new();

    private void Awake()
    {
        if (_itemTableSO == null || _itemTableSO.scriptableObjects == null)
        {
            Log.Message("Item_Table이 null입니다");
            return;
        }

        for (int i = 0; i < _itemTableSO.scriptableObjects.Length; i++)
        {
            if (_itemTableSO.scriptableObjects[i] is Item_TableSO item)
            {
                if (!_itemTables.ContainsKey(item.Item_Id))
                {
                    _itemTables.Add(item.Item_Id, item);
                }
            }
        }
    }

    public Item_TableSO GetItemTableData(int itemId)
    {
        if (_itemTables.ContainsKey(itemId))
        {
            return _itemTables[itemId];
        }
        
        Log.Message($"<color=red><b>itemId: {itemId}와 일치하는 값이 없어서 null 반환함</b></color>");
        return null;
    }
}