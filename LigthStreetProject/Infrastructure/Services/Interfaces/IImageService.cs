using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageService
    {
            Task<string> DownloadIMageFromStorageAsync(string pointId);
            Task DeleteImageFromStorageAsync(string pointId);
            Task UploadImageToStorageAsync(string pointId, string image);
    }
}
