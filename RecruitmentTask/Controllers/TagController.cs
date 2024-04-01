using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Models;
using RecruitmentTask.Services;

namespace RecruitmentTask.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("GetTagsFromExternalAPI")]
        public async Task<ActionResult>GetTagsFromExternalAPI(int count)
        {
            await _tagService.GetTagsFromExternalAPI(count);

            return Ok();
        }

        [HttpGet("GetTags")]
        public async Task<ActionResult>GetTags(int pageIndex, int pageSize, string OrderBy, SortDirection? sortDirection)
        {
            var tags = await _tagService.GetTags(pageIndex, pageSize, OrderBy, sortDirection);

            return Ok(tags);
        }
    }
}
