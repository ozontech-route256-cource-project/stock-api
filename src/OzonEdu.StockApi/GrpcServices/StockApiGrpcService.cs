using Grpc.Core;
using OzonEdu.StockApi.Grpc;
using OzonEdu.StockApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.GrpcServices
{
    public class StockApiGrpcService : StockApiGrpc.StockApiGrpcBase
    {
        private readonly IStockService stockService;

        public StockApiGrpcService(IStockService stockService)
        {
            this.stockService = stockService;
        }

        public override async Task<GetAllStockItemsResponse> GetAllStockItems(GetAllStockItemsRequest request, ServerCallContext context)
        {
            var stockItems =  await stockService.GetAll(context.CancellationToken);
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
              
    }
}
