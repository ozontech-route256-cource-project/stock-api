using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using OzonEdu.StockApi.Grpc;

using var channel = GrpcChannel.ForAddress("https://localhost:7008");
var client = new StockApiGrpc.StockApiGrpcClient(channel);

var response = await client.GetAllStockItemsAsync(new GetAllStockItemsRequest(), cancellationToken: CancellationToken.None);

foreach(var item in response.Stocks) Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");


var responseNull = await client.GetAllStockItemsWithNullsAsync(new Empty(), cancellationToken: CancellationToken.None);

foreach(var item in responseNull.Stocks) Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");

var responseMap = await client.GetAllStockItemsMapAsync(new GetAllStockItemsRequest(), cancellationToken: CancellationToken.None);


foreach(var item in responseMap.Stocks) Console.WriteLine($"{item.Value.ItemId}.{item.Value.ItemName},{item.Value.Quantity}");
try
{
    await client.AddStockItemAsync(new AddStockItemRequest { ItemName = "sdsd", Quantity = 5 });

}
catch(RpcException ex)
{
    var meta = ex.Trailers;
    Console.WriteLine($"ex - {ex.ToString()} , meta - {meta.Select(x => $"{x.Key}:{x.Value} ").Aggregate((a, b) => $"{a},{b} ")}");
};

using var stream = client.GetAllStockItemsStream(new GetAllStockItemsRequest());
await foreach(var item in stream.ResponseStream.ReadAllAsync())
    Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");


using var stream1 = client.GetAllStockItemsStream(new GetAllStockItemsRequest());

while(await stream1.ResponseStream.MoveNext())
{
    var item = stream1.ResponseStream.Current;
    Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");
}

using var clientStreamingCall = client.AddStockItemStreaming(cancellationToken: CancellationToken.None);
await clientStreamingCall.RequestStream.WriteAsync(
   new AddStockItemRequest() { ItemName = "sdsdsd", Quantity = 4 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd1", Quantity = 74 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd2", Quantity = 41 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd3", Quantity = 42 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd4", Quantity = 43 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd5", Quantity = 45 });
await clientStreamingCall.RequestStream.WriteAsync(
    new AddStockItemRequest() { ItemName = "sdsdsd6", Quantity = 47 });

await clientStreamingCall.RequestStream.CompleteAsync();

using var stream2 = client.GetAllStockItemsStream(new GetAllStockItemsRequest());
await foreach(var item in stream2.ResponseStream.ReadAllAsync())
    Console.WriteLine($"{item.ItemId}.{item.ItemName},{item.Quantity}");


Console.ReadKey();