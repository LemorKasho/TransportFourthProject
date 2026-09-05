namespace TransportFourthProject.Api.DTOs.UserDiscountTicket
{
    public class UserDiscountTicketViewDto
    {
        public int TicketId { get; set; }
        public string DiscountName { get; set; }
        public int Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
