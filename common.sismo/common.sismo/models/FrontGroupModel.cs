namespace common.sismo.models
{
    public class FrontGroupModel
    {
        public int OperationalFrontId { get; set; }
        public OperationalFrontModel OperationalFront { get; set; }
        public string Name { get; set; }
        public int FrontGroupId { get; set; }
        public int? FrontGroupType { get; set; }
        public string LastEditorUserLogin { get; set; }
        public bool IsActive { get; set; }
    }
}
