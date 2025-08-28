using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;
using Azure.Data.Tables;

namespace ABCRetail.Controllers
{
	public class OrderController : Controller
	{
		private readonly TableClient _orderTable;

		public OrderController(TableStorageService tableService)
		{
			_orderTable = tableService.GetTable("Orders"); 
		}

		// GET: /Order
		public IActionResult Index()
		{
			var orders = _orderTable.Query<OrderEntity>().ToList();
			return View(orders);
		}

		// GET: /Order/Details/{id}
		public IActionResult Details(string id)
		{
			var order = _orderTable.Query<OrderEntity>(o => o.RowKey == id).FirstOrDefault();
			if (order == null) return NotFound();
			return View(order);
		}

		// GET: /Order/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: /Order/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(OrderEntity order)
		{
			if (ModelState.IsValid)
			{
				var entity = new OrderEntity
				{
					PartitionKey = "ORDER", // Required for Table Storage
					RowKey = Guid.NewGuid().ToString("N"), 
					CustomerName = order.CustomerName,
					ProductName = order.ProductName,
					Quantity = order.Quantity,
					OrderDate = order.OrderDate
				};

				await _orderTable.AddEntityAsync(entity);
				return RedirectToAction(nameof(Index));
			}
			return View(order);
		}

		// GET: /Order/Edit/{id}
		public IActionResult Edit(string id)
		{
			var order = _orderTable.Query<OrderEntity>(o => o.RowKey == id).FirstOrDefault();
			if (order == null) return NotFound();
			return View(order);
		}

		// POST: /Order/Edit/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, OrderEntity updatedOrder)
		{
			var existing = _orderTable.Query<OrderEntity>(o => o.RowKey == id).FirstOrDefault();
			if (existing == null) return NotFound();

			existing.CustomerName = updatedOrder.CustomerName;
			existing.ProductName = updatedOrder.ProductName;
			existing.Quantity = updatedOrder.Quantity;		
			existing.OrderDate = updatedOrder.OrderDate;

			await _orderTable.UpdateEntityAsync(existing, existing.ETag, TableUpdateMode.Replace);
			return RedirectToAction(nameof(Index));
		}

		// GET: /Order/Delete/{id}
		public IActionResult Delete(string id)
		{
			var order = _orderTable.Query<OrderEntity>(o => o.RowKey == id).FirstOrDefault();
			if (order == null) return NotFound();
			return View(order);
		}

		// POST: /Order/Delete/{id}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var order = _orderTable.Query<OrderEntity>(o => o.RowKey == id).FirstOrDefault();
			if (order != null)
			{
				await _orderTable.DeleteEntityAsync(order.PartitionKey, order.RowKey);
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
