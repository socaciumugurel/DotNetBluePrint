using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using BluePrint.core.Infrastructure;
using BluePrint.shared.services.Helpers;
using BluePrint.shared.services.Responses;

namespace BluePrint.shared.services.AzureBlob
{
    [Service(typeof(IAzureBlobService))]
    class AzureBlobService : IAzureBlobService
    {
        private const string DateTimeFormatMetadata = "dd/MM/yyyy HH:mm:ss";
        private const string MetadataCreated = "Created";
        private readonly BlobServiceClient _cloudBlobClient;

        public AzureBlobService(BlobServiceClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        public Uri GetServiceUri() => _cloudBlobClient.Uri;

        public async Task<GenericResult<Uri>> UploadFileAsync(string containerName, string fileNameWithPath, Stream stream,
            CancellationToken cancellationToken)
        {
            var blob = await DefaultUploadFileAsync(containerName, fileNameWithPath, stream, cancellationToken);

            return GenericResult<Uri>.Success(blob.Uri);
        }

        public async Task<Result> DeleteFileAsync(string blobContainerName, string path, string fileName)
        {
            var blob = FindBlobFromContainer(blobContainerName, path, fileName);

            if (blob != null)
            {
                await blob.DeleteIfExistsAsync();
            }

            return Result.Success();
        }

        public async Task<GenericResult<Stream>> GetFileAsync(string blobContainerName, string path, string fileName)
        {
            var blob = FindBlobFromContainer(blobContainerName, path, fileName);

            if (blob == null || !await blob.ExistsAsync())
            {
                return GenericResult<Stream>.Error(ErrorCodes.NotFound);
            }

            var stream = new MemoryStream();
            await blob.DownloadToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return GenericResult<Stream>.Success(stream);
        }

        public GenericResult<Uri> GetFileUrlWithSas(string blobContainerName, string path, string fileName, DateTime sasLifeTime)
        {
            var blob = FindBlobFromContainer(blobContainerName, path, fileName);

            if (blob == null || !blob.Exists())
            {
                return GenericResult<Uri>.Error(ErrorCodes.NotFound);
            }

            var uri = blob.GenerateSasUri(BlobSasPermissions.Read, sasLifeTime);

            return GenericResult<Uri>.Success(uri);
        }

        public async Task<GenericResult<Uri>> MoveFileAsync(string containerName, string fileName, string fromPath, string toPath)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(containerName);
            var fullName = $"{fromPath}/{fileName}";
            var oldBlob = blobContainer.GetBlobClient(fullName);

            var newFullName = $"{toPath}/{fileName}";
            var destinationBlob = blobContainer.GetBlobClient(newFullName);
            await destinationBlob.StartCopyFromUriAsync(oldBlob.Uri);

            await oldBlob.DeleteIfExistsAsync();

            return GenericResult<Uri>.Success(destinationBlob.Uri);
        }

        public async Task<GenericResult<Uri>> CopyFileAsync(string containerName, string fileName, string fromPath, string toPath)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(containerName);
            var fullName = $"{fromPath}/{fileName}";
            var oldFile = blobContainer.GetBlobClient(fullName);

            if (oldFile == null || !await oldFile.ExistsAsync())
            {
                return GenericResult<Uri>.Error(ErrorCodes.NotFound);
            }

            var newFullName = $"{toPath}/{fileName}";
            var destinationBlockBob = blobContainer.GetBlobClient(newFullName);
            await destinationBlockBob.StartCopyFromUriAsync(oldFile.Uri);

            return GenericResult<Uri>.Success(destinationBlockBob.Uri);
        }

        public async Task<GenericResult<Uri>> CopyFileAsync(string containerName, string fromPathWithFileName, string toPathWithFileName)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(containerName);
            var oldFile = blobContainer.GetBlobClient(fromPathWithFileName);

            if (oldFile == null || !await oldFile.ExistsAsync())
            {
                return GenericResult<Uri>.Error(ErrorCodes.NotFound);
            }

            var destinationBlockBob = blobContainer.GetBlobClient(toPathWithFileName);
            await destinationBlockBob.StartCopyFromUriAsync(oldFile.Uri);

            return GenericResult<Uri>.Success(destinationBlockBob.Uri);
        }

        public async Task<Result> DeleteFolderAsync(string containerName, string path)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(containerName);
            foreach (var blob in blobContainer.GetBlobs())
            {
                if (blob.Name.ToLower().Contains(path.ToLower()))
                {
                    var blobClient = blobContainer.GetBlobClient(blob.Name);
                    await blobClient.DeleteIfExistsAsync();
                }
            }

            return Result.Success();
        }

        public async Task<GenericResult<Stream>> ReadStreamFileAsync(string blobContainerName, string path, string fileName)
        {
            var file = FindBlobFromContainer(blobContainerName, path, fileName);

            if (file == null || !await file.ExistsAsync())
            {
                return GenericResult<Stream>.Error(ErrorCodes.NotFound);
            }

            var stream = await file.OpenReadAsync();

            return GenericResult<Stream>.Success(stream);
        }

        public GenericResult<ICollection<string>> GetDirectoryFileList(string blobContainerName, string path)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(blobContainerName);
            var result = blobContainer.GetBlobs(prefix: path).Select(x => x.Name).ToList();

            return GenericResult<ICollection<string>>.Success(result);
        }

        public async Task<Result> DeleteExpiredFilesInContainer(string blobContainerName, DateTime thresholdDate)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(blobContainerName);
            var filesToDelete = blobContainer.GetBlobs().Where(x => x.Properties.CreatedOn < thresholdDate).ToList();

            foreach (var file in filesToDelete)
            {
                await blobContainer.GetBlobClient(file.Name).DeleteIfExistsAsync();
            }

            return Result.Success();
        }

        private BlobClient FindBlobFromContainer(string blobContainerName, string path, string fileName)
        {
            var blobContainer = _cloudBlobClient.GetBlobContainerClient(blobContainerName);
            var fullName = $"{path}/{fileName}".TrimStart('/').TrimEnd('/').Trim();
            var result = blobContainer.GetBlobClient(fullName);
            return result;
        }

        private async Task<BlobClient> DefaultUploadFileAsync(
            string container,
            string fileNameWithPath,
            Stream stream,
            CancellationToken cancellationToken)
        {
            stream.Position = 0;
            var contentType = MimeHelper.GetMimeTypeFromFileName(fileNameWithPath);
            var blob = _cloudBlobClient.GetBlobContainerClient(container).GetBlobClient(fileNameWithPath);

            await blob.UploadAsync(stream, overwrite: true, cancellationToken);

            var blobHeader = new BlobHttpHeaders { ContentType = contentType.Result };
            await blob.SetHttpHeadersAsync(blobHeader, cancellationToken: cancellationToken);

            var metaData =
                new Dictionary<string, string> { { MetadataCreated, DateTime.UtcNow.ToString(DateTimeFormatMetadata) } };
            await blob.SetMetadataAsync(metaData, cancellationToken: cancellationToken);

            return blob;
        }
    }
}
