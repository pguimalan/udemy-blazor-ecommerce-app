using BlazorEcommerceApp.Shared.DTOs;

namespace BlazorEcommerceApp.Server.Services.AddressService
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddress();

        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}
