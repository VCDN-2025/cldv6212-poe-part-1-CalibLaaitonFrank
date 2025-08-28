using ABCRetail.Models;
using ABCRetail.Services;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers;

public class ProductsController : Controller
{
	private readonly TableClient _table;
	private readonly BlobStorageService _blobs;
	private readonly QueueService _queue;
	private const string ContainerName = "products"; 

	public ProductsController(TableStorageService tables, BlobStorageService blobs, QueueService queue)
	{
		_table = tables.GetTable("Products");
		_blobs = blobs;
		_queue = queue;
	}

	// INDEX 
	public IActionResult Index()
	{
		var products = _table.Query<ProductEntity>(e => e.PartitionKey == "Toys").ToList();
		return View(products);
	}

	// CREATE 
	[HttpGet]
	public IActionResult Create() => View(new ProductEntity());

	[HttpPost]
	public async Task<IActionResult> Create(ProductEntity model, IFormFile? imageFile)
	{
		model.PartitionKey = "Toys";
		model.RowKey = Guid.NewGuid().ToString("N");

		if (imageFile != null && imageFile.Length > 0)
		{
			using var s = imageFile.OpenReadStream();
			var savedName = $"{model.Sku}-{Guid.NewGuid():N}{Path.GetExtension(imageFile.FileName)}";

			model.ImageBlobName = await _blobs.UploadAsync(s, savedName, imageFile.ContentType, ContainerName);
			model.ImageUrl = _blobs.GetBlobUrl(model.ImageBlobName, ContainerName);

			await _queue.EnqueueAsync($"Uploading image '{savedName}' for SKU {model.Sku}");
		}

		await _table.AddEntityAsync(model);
		await _queue.EnqueueAsync($"Product created: {model.Sku} - {model.Name}");
		return RedirectToAction(nameof(Index));
	}

	// EDIT 
	[HttpGet]
	public IActionResult Edit(string rowKey, string partitionKey)
	{
		var product = _table.GetEntity<ProductEntity>(partitionKey, rowKey).Value;
		return View(product);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(ProductEntity model, IFormFile? imageFile)
	{
		if (imageFile != null && imageFile.Length > 0)
		{
			using var s = imageFile.OpenReadStream();
			var savedName = $"{model.Sku}-{Guid.NewGuid():N}{Path.GetExtension(imageFile.FileName)}";

			model.ImageBlobName = await _blobs.UploadAsync(s, savedName, imageFile.ContentType, ContainerName);
			model.ImageUrl = _blobs.GetBlobUrl(model.ImageBlobName, ContainerName);

			await _queue.EnqueueAsync($"Updated image '{savedName}' for SKU {model.Sku}");
		}

		model.ETag = ETag.All;
		await _table.UpdateEntityAsync(model, model.ETag, TableUpdateMode.Replace);
		await _queue.EnqueueAsync($"Product updated: {model.Sku} - {model.Name}");
		return RedirectToAction(nameof(Index));
	}

	// DELETE 
	[HttpGet]
	public IActionResult Delete(string rowKey, string partitionKey)
	{
		var product = _table.GetEntity<ProductEntity>(partitionKey, rowKey).Value;
		return View(product);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(string rowKey, string partitionKey)
	{
		await _table.DeleteEntityAsync(partitionKey, rowKey);
		await _queue.EnqueueAsync($"Product deleted: {rowKey}");
		return RedirectToAction(nameof(Index));
	}
}
