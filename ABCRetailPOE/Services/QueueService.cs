using Azure.Storage.Queues;

namespace ABCRetail.Services;

public class QueueService
{
	private readonly QueueClient _queue;
	public QueueService(string connectionString, string queueName)
	{
		_queue = new QueueClient(connectionString, queueName);
		_queue.CreateIfNotExists();
	}

	public Task EnqueueAsync(string message) => _queue.SendMessageAsync(message);
	public async Task<List<string>> PeekAsync(int max = 16)
	{
		var msgs = await _queue.PeekMessagesAsync(max);
		return msgs.Value.Select(m => m.MessageText).ToList();
	}
}
