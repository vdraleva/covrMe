namespace CovrMe.Models.Deliveries.Result
{
    public partial class SpeedyOfficeModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SiteId { get; set; }

        public bool IsChecked { get; set; }
    }
}
