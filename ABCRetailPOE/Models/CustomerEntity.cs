using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models;

public class CustomerEntity : ITableEntity
{
	public string PartitionKey { get; set; } = "Customer";
	public string RowKey { get; set; } = Guid.NewGuid().ToString("N");
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }

	// Domain fields
	public string FullName { get; set; } = "";
	public string Email { get; set; } = "";
	public string Phone { get; set; } = "";
	public string City { get; set; } = "";
	public string LoyaltyTier { get; set; } = "Bronze";
}
