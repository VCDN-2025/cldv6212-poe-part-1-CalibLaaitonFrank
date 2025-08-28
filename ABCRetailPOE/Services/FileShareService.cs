using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.Services;

public class FileShareService
{
	private readonly ShareClient _share;
	private readonly ShareDirectoryClient _root;

	public FileShareService(string connectionString, string shareName)
	{
		_share = new ShareClient(connectionString, shareName);
		_share.CreateIfNotExists();
		_root = _share.GetRootDirectoryClient();
	}

	public async Task UploadAsync(Stream stream, string fileName, string contentType = "application/octet-stream")
	{
		var file = _root.GetFileClient(fileName);

	
		await file.CreateAsync(stream.Length);

	
		var options = new ShareFileSetHttpHeadersOptions
		{
			HttpHeaders = new ShareFileHttpHeaders
			{
				ContentType = contentType
			}
		};
		await file.SetHttpHeadersAsync(options);

	
		await file.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
	}




	public async Task<List<string>> ListAsync()
	{
		var list = new List<string>();
		await foreach (ShareFileItem item in _root.GetFilesAndDirectoriesAsync())
			if (!item.IsDirectory) list.Add(item.Name);
		return list;
	}

	public async Task<Stream> DownloadAsync(string fileName)
		=> (await _root.GetFileClient(fileName).DownloadAsync()).Value.Content;
}
