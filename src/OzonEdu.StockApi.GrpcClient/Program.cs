using Grpc.Net.Client;
using OzonEdu.StockApi.Grpc;
using System.Threading.Channels;

using var channel = GrpcChannel.ForAddress("https://localhost:7008");
var client = new StockApiGrpc.StockApiGrpcClient(channel);

var response = await client.GetAllStockItemsAsync(new GetAllStockItemsRequest(),cancellationToken: CancellationToken.None);

foreach(var item in  response.Stocks) Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");