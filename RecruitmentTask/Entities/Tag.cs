namespace RecruitmentTask.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public bool has_synonyms { get; set; }
        public bool is_moderator_only { get; set; }
        public bool is_required { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public double? percentageShare { get; set; }
    }
}
