/*
작성자 : NekioEmilia
수정자 :
작성일 : 26-06-15
수정일 : 26-06-15

역할 : ItemTable SO 데이터를 캐싱하고 반환하는 Model 스크립트
방식 : Item_Id를 기준으로 Presenter에서 값을 넘겨주면 올바른 값일 경우 Item_Id에 맞는 ItemTable을 반환
*/

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