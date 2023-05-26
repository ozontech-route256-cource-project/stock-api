using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OzonEdu.StockApi.Grpc;
using OzonEdu.StockApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.GrpcServices;

public class StockApiGrpcService : StockApiGrpc.StockApiGrpcBase
{
    private readonly IStockService stockService;

    public StockApiGrpcService(IStockService stockService)
    {
        this.stockService = stockService;
    }

    public override async Task<GetAllStockItemsWithNullsResponse> GetAllStockItemsWithNulls(Empty request, ServerCallContext context)
    {
        var stockItems = await stockService.GetAll(context.CancellationToken);
        return new GetAllStockItemsWithNullsResponse
        {
            Stocks = { stockItems.Select(x => new GetAllStockItemsWithNullResponseUnit
            {
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        ItemName = null
                    } )
            }
        };
    }

    public override async Task<GetAllStockItemsMapResponse> GetAllStockItemsMap(GetAllStockItemsRequest request, ServerCallContext context)
    {
        var stockItems = await stockService.GetAll(context.CancellationToken);
        return new GetAllStockItemsMapResponse
        {
            Stocks = { stockItems.ToDictionary(x => x.ItemId ,x=>new GetAllStockItemsResponseUnit
            {
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        ItemName = x.ItemName
                    } )
            }
        };
    }

    public override async Task<GetAllStockItemsResponse> GetAllStockItems(GetAllStockItemsRequest request, ServerCallContext context)
    {
        var stockItems = await stockService.GetAll(context.CancellationToken);
        return new GetAllStockItemsResponse
        {
            Stocks = { stockItems.Select(x => new GetAllStockItemsResponseUnit
            {
                        ItemId = x.ItemId,
                        Quantity = x.Quantity,
                        ItemName = x.ItemName
                    } )
            }
        };
    }

    public override Task<Empty> AddStockItem(AddStockItemRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, "validation failed"),
            new Metadata
            {
                new Metadata.Entry("key","value")
            });
    }

    public override async Task<Empty> AddStockItemStreaming(IAsyncStreamReader<AddStockItemRequest> requestStream, ServerCallContext context)
    {
        while(!context.CancellationToken.IsCancellationRequested)
        {
            if(await requestStream.MoveNext())
            {
                var currentItem = requestStream.Current;
                await stockService.Add(new StockItemCreationModel
                {
                    ItemName = currentItem.ItemName,
                    Quantity = currentItem.Quantity
                }
                , context.CancellationToken);
            }
        }
        return new Empty();
    }

    public override async Task GetAllStockItemsStream(GetAllStockItemsRequest request, IServerStreamWriter<GetAllStockItemsResponseUnit> responseStream, ServerCallContext context)
    {
        var stockItems = await stockService.GetAll(context.CancellationToken);
        foreach(var item in stockItems)
        {
            if(context.CancellationToken.IsCancellationRequested) break;
            await responseStream.WriteAsync(
                new GetAllStockItemsResponseUnit
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    ItemName = item.ItemName
                }
                 );
        }

    }
}
