using Azure.Data.Tables;

namespace ABCRetail.Services;

public class TableStorageService
{
	private readonly TableServiceClient _tableServiceClient;
	public TableStorageService(string connectionString)
		=> _tableServiceClient = new TableServiceClient(connectionString);

	public TableClient GetTable(string tableName)
	{
		var client = _tableServiceClient.GetTableClient(tableName);
		client.CreateIfNotExists();
		return client;
	}
}
