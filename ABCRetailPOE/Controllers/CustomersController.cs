using ABCRetail.Models;
using ABCRetail.Services;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers;

public class CustomerController : Controller
{
	private readonly TableClient _table;
	private readonly QueueService _queue;

	public CustomerController(TableStorageService tables, QueueService queue)
	{
		_table = tables.GetTable("Customers");
		_queue = queue;
	}

	public IActionResult Index()
	{
		var customers = _table.Query<CustomerEntity>(e => e.PartitionKey == "Customer").ToList();
		return View(customers);
	}

	[HttpGet]
	public IActionResult Create() => View(new CustomerEntity());

	[HttpPost]
	public async Task<IActionResult> Create(CustomerEntity model)
	{
		model.PartitionKey = "Customer";
		model.RowKey = Guid.NewGuid().ToString("N");
		await _table.AddEntityAsync(model);
		await _queue.EnqueueAsync($"New customer created: {model.FullName}");
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public IActionResult Edit(string rowKey, string partitionKey)
	{
		var customer = _table.GetEntity<CustomerEntity>(partitionKey, rowKey).Value;
		return View(customer);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(CustomerEntity model)
	{
		_ = await _table.UpdateEntityAsync(model, ETag.All, TableUpdateMode.Replace);
		await _queue.EnqueueAsync($"Customer updated: {model.FullName}");
		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Delete(string rowKey, string partitionKey)
	{
		try
		{
			await _table.DeleteEntityAsync(partitionKey, rowKey, ETag.All);
			await _queue.EnqueueAsync($"Customer deleted: {rowKey}");
		}
		catch (RequestFailedException ex)
		{
			return BadRequest($"Error deleting customer: {ex.Message}");
		}

		return RedirectToAction(nameof(Index));
	}
}
