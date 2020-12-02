using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IVideoEmbedService
    {
        VideoEmbedResult GetVideoDetail(string videoUrl, string pictureSaveName);
    }
}