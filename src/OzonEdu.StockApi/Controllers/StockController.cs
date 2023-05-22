using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.StockApi.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockService stockService;

    public StockController(IStockService stockService)
    {
        this.stockService = stockService;
    }


    [HttpGet]
    [ProducesResponseType(typeof(List<StockItem>), 200)]
    public async Task<ActionResult<List<StockItem>>> GetAll(CancellationToken token)
    {
        return Ok(await stockService.GetAll(token));
    }

    /// <summary>
    /// get Item
    /// </summary>
    /// <param name="id">id</param>        
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(StockItem), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StockItem>> GetById(long id, CancellationToken token)
    {
        var stockItem = await stockService.GetById(id, token);
        if(stockItem == null) return NotFound();
        return Ok(stockItem);
    }

    [HttpPost]
    public async Task<ActionResult<StockItem>> AddItem(StockItemModel stockItem, CancellationToken token)
    {
        return await stockService.Add(new StockItemCreationModel { ItemName = stockItem.ItemName, Quantity = stockItem.Quantity }, token);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StockItem>> UpdateItem(long id, StockItemModel stockItem, CancellationToken token)
    {
        //  return await stockService.Update(new StockItemCreationModel { ItemName = stockItem.ItemName, Quantity = stockItem.Quantity }, token);
        throw new NotImplementedException();

    }


    public class StockItemModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

}
