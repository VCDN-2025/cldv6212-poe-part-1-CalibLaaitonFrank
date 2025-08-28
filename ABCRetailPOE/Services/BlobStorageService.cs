using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ABCRetail.Services;

public class BlobStorageService
{
	private readonly BlobServiceClient _blobServiceClient;

	public BlobStorageService(string connectionString, string v)
	{
		_blobServiceClient = new BlobServiceClient(connectionString);
	}

	// Upload a file and return its blob name
	public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName)
	{
		var container = _blobServiceClient.GetBlobContainerClient(containerName);
		await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

		var blob = container.GetBlobClient(fileName);
		await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
		return blob.Name;
	}

	// Get blob URL
	public string GetBlobUrl(string blobName, string containerName)
	{
		var container = _blobServiceClient.GetBlobContainerClient(containerName);
		return container.GetBlobClient(blobName).Uri.ToString();
	}
}
