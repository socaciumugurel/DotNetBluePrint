using BluePrint.shared.services.Responses;
using Microsoft.AspNetCore.StaticFiles;

namespace BluePrint.shared.services.Helpers
{
    public static class MimeHelper
    {
        public static GenericResult<string> GetMimeTypeFromFileName(string fileName)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var mimeType);
            return mimeType != null ? GenericResult<string>.Success(mimeType) : GenericResult<string>.Error(ErrorCodes.UnsupportedMediaType());
        }
    }
}
