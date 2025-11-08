using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI; // thêm dòng này để dùng IRecyclableScrollRectDataSource

using MyProject.Data; // dùng cho class InvenItems

public class RecyclableInventoryManager : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    private RecyclableScrollRect _recyclableScrollRect;

    [SerializeField]
    private int _dataLength;

    // Dummy data List
    private List<InvenItems> _invenItems = new List<InvenItems>();

    // Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _recyclableScrollRect.DataSource = this;

        // ví dụ khởi tạo dữ liệu mẫu
        for (int i = 0; i < _dataLength; i++)
        {
            _invenItems.Add(new InvenItems($"Item {i}", $"Description for item {i}"));
        }
    }

    // Trả về tổng số item trong danh sách
    public int GetItemCount()
    {
        return _invenItems.Count;
    }

    // Gán dữ liệu cho từng cell
    public void SetCell(ICell cell, int index)
    {
        // Ép kiểu về CellItemData (class bạn đã tạo)
        CellItemData item = cell as CellItemData;
        item.ConfigureCell(_invenItems[index], index);
    }
    private void Start()
    {
        List<InvenItems> lstItem = new List<InvenItems>();
        for(int i=0;i < 50; i++)
        {
            InvenItems invenItem = new InvenItems();
            invenItem.name = "Name_" + i.ToString();
            invenItem.descrition = "Des_" + i.ToString();
            lstItem.Add(invenItem);
             
        }
        SetLstItem(lstItem);
        _recyclableScrollRect.ReloadData();
    }
    public void SetLstItem(List<InvenItems> lst)
    {
        _invenItems  = lst;
    }
}
