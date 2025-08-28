using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models;

public class ProductEntity : ITableEntity
{
	public string PartitionKey { get; set; } = "CarPart";
	public string RowKey { get; set; } = Guid.NewGuid().ToString("N");
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }

	public string Sku { get; set; } = "";
	public string Name { get; set; } = "";
	public string Category { get; set; } = "";
	public string Brand { get; set; } = "";
	public double Price { get; set; }
	public int Stock { get; set; }
	public string ImageBlobName { get; set; } = "";
	public string ImageUrl { get; set; } = "";
}
