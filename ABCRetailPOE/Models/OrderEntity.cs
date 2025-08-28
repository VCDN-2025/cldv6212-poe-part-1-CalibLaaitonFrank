using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
	public class OrderEntity : ITableEntity
	{
		public string PartitionKey { get; set; } = "ORDER";
		public string RowKey { get; set; } = Guid.NewGuid().ToString("N");
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string Id
		{
			get => RowKey;
			set => RowKey = value;
		}

		public string CustomerName { get; set; } = string.Empty;
		public string ProductName { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
	}
}
