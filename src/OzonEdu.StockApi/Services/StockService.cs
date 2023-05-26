using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.Services;

public interface IStockService
{
    Task<List<StockItem>> GetAll(CancellationToken token);
    Task<StockItem> GetById(long ItemId, CancellationToken token);
    Task<StockItem> Add(StockItemCreationModel item, CancellationToken token);
}

public class StockItemCreationModel
{
    public string ItemName { get; set; }
    public int Quantity { get; set; }
}

public class StockService : IStockService
{
    private readonly List<StockItem> StockItems = new List<StockItem>
        {
            new StockItem(1,"Футболка",10),
            new StockItem(2,"Толстовка",20),
            new StockItem(3,"Кепка",15)
        };

    public Task<StockItem> Add(StockItemCreationModel stockItem, CancellationToken token)
    {
        var itemId = StockItems.Max(x => x.ItemId) + 1;
        var newStockItem = new StockItem(itemId, stockItem.ItemName, stockItem.Quantity);
        StockItems.Add(newStockItem);
        return Task.FromResult(newStockItem);
    }

    public Task<List<StockItem>> GetAll(CancellationToken token) => Task.FromResult(StockItems);

    public Task<StockItem> GetById(long itemId, CancellationToken token)
    {
        return Task.FromResult(StockItems.FirstOrDefault(x => x.ItemId == itemId));
    }
}

public class StockItem
{
    public StockItem(long itemId, string itemName, int quantity)
    {
        ItemId = itemId;
        ItemName = itemName;
        Quantity = quantity;
    }

    public long ItemId { get; }
    public string ItemName { get; }
    public int Quantity { get; }
}

public class CustomException : Exception
{
    public CustomException() : base("custom exception")
    {
    }
}

