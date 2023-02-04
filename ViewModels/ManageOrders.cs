namespace SAMSUNG_4_YOU.ViewModels
{
    public class ManageOrders
    {
        public int OrderId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public int OrderStatus { get; set; }
    }
}
