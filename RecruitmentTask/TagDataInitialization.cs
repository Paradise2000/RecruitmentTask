
using RecruitmentTask.Services;

namespace RecruitmentTask
{
    public class TagDataInitialization : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TagDataInitialization(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
                await tagService.GetTagsFromExternalAPI(182);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
