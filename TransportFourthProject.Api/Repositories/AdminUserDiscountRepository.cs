using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.UserDiscount;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class AdminUserDiscountRepository : Repository<UserDiscount>, IAdminUserDiscountRepository   
    {
        private readonly AppDbContext _context;

        public AdminUserDiscountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> AddUserDiscountAsync(AddUserDiscountDto dto)
        {
            var exists = await _context.UserDiscounts
                .FirstOrDefaultAsync(t => t.Name == dto.Name);

            if (exists != null)
            {
                if (exists.Status == UserDiscountStatus.Disabled)
                {
                    exists.Status = UserDiscountStatus.Active;
                    exists.Percentage = dto.DiscountPercentage;

                    await _context.SaveChangesAsync();
                    return "Discount restored and updated";
                }

                return "Discount already exists";
            }

            var discount = new UserDiscount
            {
                Name = dto.Name,
                Percentage = dto.DiscountPercentage,
                Status = UserDiscountStatus.Active
            };

            await _context.UserDiscounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return "Discount added successfully";
        }

        public async Task<string> UpdateUserDiscountAsync(UpdateUserDiscountDto dto)
        {
            var discount = await _context.UserDiscounts
                .FirstOrDefaultAsync(t => t.Id == dto.UserDiscountId);

            if (discount == null)
                return "Discount not found";

            if (discount.Status == UserDiscountStatus.Disabled)
                return "Cannot update a deleted discount";

            if (!string.IsNullOrWhiteSpace(dto.Name))
                discount.Name = dto.Name;

            if (dto.DiscountPercentage.HasValue)
                discount.Percentage = dto.DiscountPercentage.Value;

            await _context.SaveChangesAsync();
            return "Discount updated successfully";
        }

        public async Task<string> DeleteUserDiscountAsync(int id)
        {
            var discount = await _context.UserDiscounts
                .FirstOrDefaultAsync(t => t.Id == id);

            if (discount == null)
                return "Discount not found";

            if (discount.Status == UserDiscountStatus.Disabled)
                return "Discount already deleted";

            discount.Status = UserDiscountStatus.Disabled;

            await _context.SaveChangesAsync();
            return "Discount deleted successfully";
        }

        public async Task<string> RestoreUserDiscountAsync(int id)
        {
            var discount = await _context.UserDiscounts
                .FirstOrDefaultAsync(t => t.Id == id);

            if (discount == null)
                return "Discount not found";

            if (discount.Status == UserDiscountStatus.Active)
                return "Discount is already active";

            discount.Status = UserDiscountStatus.Active;

            await _context.SaveChangesAsync();
            return "Discount restored successfully";
        }

        public async Task<List<UserDiscount>> GetDeletedUserDiscountsAsync()
        {
            return await _context.UserDiscounts
                .Where(t => t.Status == UserDiscountStatus.Disabled)
                .ToListAsync();
        }
        public async Task<List<UserDiscount>> GetActiveUserDiscountsAsync()
        {
            return await _context.UserDiscounts
                .Where(t => t.Status == UserDiscountStatus.Active)
                .ToListAsync();
        }
    }
}





