using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public AddressService(DataContext dataContext, IAuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;
            if(dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _dataContext.Addresses.Add(address);
                response.Data = dbAddress;
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.State = address.State;
                dbAddress.City = address.City;
                dbAddress.Country = address.Country;
                dbAddress.Street = address.Street;
                dbAddress.Zip = address.Zip;
                response.Data = dbAddress;
            }

            await _dataContext.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            int userId = _authService.GetUserId();
            var address = await _dataContext.Addresses.FirstOrDefaultAsync(a => a.UserId == userId);

            return new ServiceResponse<Address>
            {
                Data = address,
            };
        }
    }
}
