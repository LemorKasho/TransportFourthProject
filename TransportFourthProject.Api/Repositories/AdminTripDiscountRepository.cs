using Microsoft.EntityFrameworkCore;
using TransportFourthProject.Api.Data;
using TransportFourthProject.Api.DTOs.TripDiscount;
using TransportFourthProject.Api.Enums;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public class AdminTripDiscountRepository : Repository<TripDiscount>, IAdminTripDiscountRepository
    {
        private readonly AppDbContext _context;

        public AdminTripDiscountRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<string> AddTripDiscountAsync(AddTripDiscountDto dto)
        {
            var exists = await _context.TripDiscounts
                .FirstOrDefaultAsync(t => t.Name == dto.Name);

            if (exists != null)
            {
                if (exists.Status == TripDiscountStatus.Disabled)
                {
                    exists.Status = TripDiscountStatus.Active;
                    exists.Percentage = dto.DiscountPercentage;

                    await _context.SaveChangesAsync();
                    return "Discount restored and updated";
                }

                return "Discount already exists";
            }

            var discount = new TripDiscount
            {
                Name = dto.Name,
                Percentage = dto.DiscountPercentage,
                Status = TripDiscountStatus.Active
            };

            await _context.TripDiscounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return "Discount added successfully";
        }

        public async Task<string> UpdateTripDiscountAsync(UpdateTripDiscountDto dto)
        {
            var discount = await _context.TripDiscounts
                .FirstOrDefaultAsync(t => t.Id == dto.TripDiscountId);

            if (discount == null)
                return "Discount not found";

            if (discount.Status == TripDiscountStatus.Disabled)
                return "Cannot update a deleted discount";

            if (!string.IsNullOrWhiteSpace(dto.Name))
                discount.Name = dto.Name;

            if (dto.Percentage.HasValue)
                discount.Percentage = dto.Percentage.Value;

            await _context.SaveChangesAsync();
            return "Discount updated successfully";
        }

        public async Task<string> DeleteTripDiscountAsync(int id)
        {
            var discount = await _context.TripDiscounts
                .FirstOrDefaultAsync(t => t.Id == id);

            if (discount == null)
                return "Discount not found";

            if (discount.Status == TripDiscountStatus.Disabled)
                return "Discount already deleted";

            discount.Status = TripDiscountStatus.Disabled;

            await _context.SaveChangesAsync();
            return "Discount deleted successfully";
        }

        public async Task<string> RestoreTripDiscountAsync(int id)
        {
            var discount = await _context.TripDiscounts
                .FirstOrDefaultAsync(t => t.Id == id);

            if (discount == null) 
                return "Discount not found";

            if (discount.Status == TripDiscountStatus.Active)
                return "Discount is already active";

            discount.Status = TripDiscountStatus.Active;

            await _context.SaveChangesAsync();
            return "Discount restored successfully";
        }

        public async Task<List<TripDiscount>> GetDeletedTripDiscountsAsync()
        {
            return await _context.TripDiscounts
                .Where(t => t.Status == TripDiscountStatus.Disabled)
                .ToListAsync();
        }
        public async Task<List<TripDiscount>> GetActiveTripDiscountsAsync()
        {
            return await _context.TripDiscounts
                .Where(t => t.Status == TripDiscountStatus.Active)
                .ToListAsync();
        }


    }
}












