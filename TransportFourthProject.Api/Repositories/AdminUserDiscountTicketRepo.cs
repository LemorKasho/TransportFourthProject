using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.UserDiscountTicket;
using TransportFourthProject.Api.Models;
using static TransportFourthProject.Api.Repositories.AdminUserDiscountTicketRepository;

namespace TransportFourthProject.Api.Repositories
{
    public class AdminUserDiscountTicketRepository : Repository<UserDiscountTicket>, IAdminUserDiscountTicketRepo
    {
        private readonly AppDbContext _context;

        public AdminUserDiscountTicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> AddUserDiscountTicketAsync(AddUserDiscountTicketDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return "User not found";

            var discount = await _context.UserDiscounts.FindAsync(dto.UserDiscountId);
            if (discount == null)
                return "Discount not found";

            if (dto.EndDate <= dto.StartDate)
                return "End date must be greater than start date";

            var ticket = new UserDiscountTicket
            {
                UserId = dto.UserId,
                DiscountId = dto.UserDiscountId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _context.UserDiscountTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return "User discount ticket added successfully";
        }

        public async Task<List<UserDiscountTicketViewDto>> GetUserDiscountTicketsAsync(int userId)
        {
            var tickets = await _context.UserDiscountTickets
                .Where(t => t.UserId == userId)
                .Include(t => t.UserDiscount)
                .Select(t => new UserDiscountTicketViewDto
                {
                    TicketId = t.Id,
                    DiscountName = t.UserDiscount.Name,
                    Percentage = t.UserDiscount.Percentage,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                })
                .ToListAsync();

            return tickets;
        }
    }
}










