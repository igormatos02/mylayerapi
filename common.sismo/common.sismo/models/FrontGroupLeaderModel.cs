namespace common.sismo.models
{
    public class FrontGroupLeaderModel
    {
        public int OperationalFrontId { get; set; }
        public OperationalFrontModel OperationalFront { get; set; }
        public string TeamLeaderUserLogin { get; set; }
        public string Name { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public string LastEditorUserLogin { get; set; }
        public bool IsActive { get; set; }
    }
}
