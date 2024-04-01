using Newtonsoft.Json;
using RecruitmentTask.DTOs;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using System.Diagnostics;
using System.Text;

namespace RecruitmentTask.Services
{
    public interface ITagService
    {
        public Task GetTagsFromExternalAPI(int count);
        public Task<PaginationModel<TagDto>> GetTags(int pageIndex, int pageSize, string OrderBy, SortDirection? sortDirection);
    }

    public class TagService : ITagService
    {
        private readonly RecruitmentTaskDbContext _context;

        public TagService(RecruitmentTaskDbContext context)
        {
            _context = context;
        }

        public async Task GetTagsFromExternalAPI(int count)
        {
            _context.Tags.RemoveRange(_context.Tags);
            await _context.SaveChangesAsync();

            using (var httpClient = new HttpClient())
            {
                int pagesToFetch = (int)Math.Ceiling((double)count / 100);

                for (int page = 1; page <= pagesToFetch; page++)
                {
                    int pageSize = (page == pagesToFetch && count % 100 != 0) ? count % 100 : 100;

                    var result = await httpClient.GetAsync($"https://api.stackexchange.com/2.3/tags?pagesize={pageSize}&page={page}&order=desc&sort=popular&site=stackoverflow");

                    using (var stream = await result.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream))
                    {
                        var json = await reader.ReadToEndAsync();
                        var tags = JsonConvert.DeserializeObject<TagResponseDto>(json);

                        await _context.Tags.AddRangeAsync(tags.items.Select(p => new Tag
                        {
                            has_synonyms = p.has_synonyms,
                            is_moderator_only = p.is_moderator_only,
                            is_required = p.is_required,
                            count = p.count,
                            name = p.name
                        }));

                        await _context.SaveChangesAsync();
                    }
                }
            }

            double sum = _context.Tags.Sum(c => c.count);
            foreach(var entity in _context.Tags)
            {
                entity.percentageShare = (Convert.ToDouble(entity.count)/sum) * 100;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PaginationModel<TagDto>> GetTags(int pageIndex, int pageSize, string OrderBy, SortDirection? sortDirection)
        {
            var tags = _context.Tags.Select(t => new TagDto
            {
                has_synonyms = t.has_synonyms,
                is_moderator_only = t.is_moderator_only,
                is_required = t.is_required,
                count = t.count,
                name = t.name,
                percentageShare = t.percentageShare
            }).ToList();

            var paginatedList = PaginationModel<TagDto>.Create(tags, pageIndex, pageSize, OrderBy, sortDirection ?? SortDirection.ASC);

            return paginatedList;
        }
        

    }
}
