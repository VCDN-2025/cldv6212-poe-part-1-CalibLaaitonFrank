using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers;

public class ContractsController : Controller
{
	private readonly FileShareService _files;
	private readonly QueueService _queue;

	public ContractsController(FileShareService files, QueueService queue)
	{ _files = files; _queue = queue; }

	public async Task<IActionResult> Index()
		=> View(await _files.ListAsync());

	[HttpPost]
	public async Task<IActionResult> Upload(IFormFile contract)
	{
		if (contract != null && contract.Length > 0)
		{
			using var s = contract.OpenReadStream();
			await _files.UploadAsync(s, contract.FileName, contract.ContentType);
			await _queue.EnqueueAsync($"Contract uploaded: {contract.FileName}");
		}
		return RedirectToAction(nameof(Index));
	}

	public async Task<FileStreamResult> Download(string id)
	{
		var stream = await _files.DownloadAsync(id);
		return File(stream, "application/octet-stream", id);
	}
}
