namespace RecruitmentTask.DTOs
{
    public class TagResponseDto
    {
        public List<TagDto> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }
}
