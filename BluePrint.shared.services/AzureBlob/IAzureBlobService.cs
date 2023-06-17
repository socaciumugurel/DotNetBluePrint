using BluePrint.shared.services.Responses;

namespace BluePrint.shared.services.AzureBlob
{
    public interface IAzureBlobService
    {
        /// <summary>
        /// Get Azure Blob root Uri
        /// </summary>
        /// <returns></returns>
        Uri GetServiceUri();

        /// <summary>
        /// Generic file uploader
        /// </summary>
        /// <param name="containerName">name of blob container</param>
        /// <param name="fileNameWithPath">The full path but without the file name</param>
        /// <param name="stream">Stream file</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GenericResult<Uri>> UploadFileAsync(
            string containerName,
            string fileNameWithPath,
            Stream stream,
            CancellationToken cancellationToken);

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="blobContainerName">containerName container name</param>
        /// <param name="path">additional path from container</param>
        /// <param name="fileName">the name of the file</param>
        /// <returns></returns>
        Task<Result> DeleteFileAsync(string blobContainerName, string path, string fileName);

        /// <summary>
        /// Get file from azure containerName storage
        /// </summary>
        /// <param name="blobContainerName">containerName container name</param>
        /// <param name="path">additional path from container</param>
        /// <param name="fileName">the name of the file</param>
        /// <returns></returns>
        Task<GenericResult<Stream>> GetFileAsync(string blobContainerName, string path, string fileName);

        /// <summary>
        /// Get file url with SAS from azure containerName storage
        /// </summary>
        /// <param name="blobContainerName">containerName container name</param>
        /// <param name="path">additional path from container</param>
        /// <param name="fileName">the name of the file</param>
        /// <param name="sasLifeTime">Token lifetime</param>
        /// <returns></returns>
        GenericResult<Uri> GetFileUrlWithSas(
            string blobContainerName,
            string path,
            string fileName,
            DateTime sasLifeTime);

        /// <summary>
        /// Move file from on path to other path
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        Task<GenericResult<Uri>> MoveFileAsync(string containerName, string fileName, string fromPath, string toPath);

        /// <summary>
        /// Copy file from on path to other path
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <returns></returns>
        Task<GenericResult<Uri>> CopyFileAsync(string containerName, string fileName, string fromPath, string toPath);

        /// <summary>
        /// Copy file from on path to other path
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fromPathWithFileName"></param>
        /// <param name="toPathWithFileName"></param>
        /// <returns></returns>
        Task<GenericResult<Uri>> CopyFileAsync(string containerName, string fromPathWithFileName, string toPathWithFileName);

        /// <summary>
        /// Delete all files inside the "folder"
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<Result> DeleteFolderAsync(string containerName, string path);

        /// <summary>
        /// Its made to read big files from Azure BlobClient Stream
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<GenericResult<Stream>> ReadStreamFileAsync(string blobContainerName, string path, string fileName);

        /// <summary>
        /// Get directory file list
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        GenericResult<ICollection<string>> GetDirectoryFileList(string blobContainerName, string path);

        /// <summary>
        /// Delete all blobs from a container that were created before threshold date
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="thresholdDate"></param>
        /// <returns></returns>
        Task<Result> DeleteExpiredFilesInContainer(string blobContainerName, DateTime thresholdDate);
    }
}
