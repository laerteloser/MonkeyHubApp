using MonkeyHubApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyHubApp.Services
{
    public interface IMonkeyHubApiService
    {
        Task<List<Tag>> GetTagAsync();

        Task<List<Content>> GetContentsByTagIdAsync(string tagId);

        Task<List<Content>> GetContentsByFilterAsync(string filter);
    }
}
