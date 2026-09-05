using Microsoft.AspNetCore.JsonPatch;
using TransportFourthProject.Api.DTOs.RoutePrice;
using TransportFourthProject.Api.Models;

namespace TransportFourthProject.Api.Repositories
{
    public interface IEmployeeRoutePriceRepository : IRepository<RoutePrice>
    {
        Task<List<RoutePriceDto>> GetAllRoutePricesAsync();
        Task<List<RoutePriceDto>> GetDeletedRoutePricesAsync();

        Task<string> PatchRoutePriceAsync(int id, JsonPatchDocument<RoutePrice> patchDoc);

        Task<string> DeleteRoutePriceAsync(int id);

        Task<string> RestoreRoutePriceAsync(int id);

        Task<string> AddRoutePriceAsync(AddRoutePriceForEmployeeDto dto);
        Task<RoutePriceStatusesDto> GetRoutePriceStatusesAsync();

        Task<List<RoutePriceByBusTypeDto>> GetAllRoutePricesByBusTypeIdAsync(int busTypeId); 

        Task<List<UsedRoutePriceDto>> GetMostActiveUsedRoutePricesAsync();

        Task<List<UsedRoutePriceDto>> GetLeastActiveUsedRoutePricesAsync();

        decimal SuggestPriceForRoutePrice(decimal distanceKm, int capacity);

        Task<List<UsedRoutePriceDto>> SuggestRoutePricesForDeletionAsync();

        Task<List<UpdatePriceForRoutePriceSuggest>> UpdatePriceForRoutePriceSuggestAsync();
    }
}






