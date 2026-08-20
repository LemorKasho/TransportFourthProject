namespace TransportFourthProject.Api.DTOs.User
{
    public class GetAllUserDiscountTicketsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int DiscountTicketNumber { get; set; }
        public string DiscountName { get; set; } = string.Empty;
        public string Percentage { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
    }
}