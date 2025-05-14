using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

public class BlobStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public BlobStorageService(IConfiguration config)
    {
        _connectionString = config["Azure:BlobConnection"];
        _containerName = "venueimages";
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return blobClient.Uri.ToString();
    }
}
