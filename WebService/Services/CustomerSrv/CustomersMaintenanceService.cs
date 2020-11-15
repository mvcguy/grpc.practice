using Grpc.Core;
using System;
using System.Threading.Tasks;
using WebService.CustomersProtos;

namespace WebService.Services.CustomerSrv
{
    public class CustomersMaintenanceService : CustomersMaintenance.CustomersMaintenanceBase
    {
        private readonly ICustomersRepository repository;

        public CustomersMaintenanceService(ICustomersRepository repository)
        {
            this.repository = repository;
        }

        public override async Task GetAllCustomers(CustomersRequest request,
            IServerStreamWriter<CustomersResponse> responseStream, ServerCallContext context)
        {
            await foreach (var item in repository.GetCustomers())
            {
                await responseStream.WriteAsync(item.ToCustomerResponse());
            }
        }

        public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request,
            ServerCallContext context)
        {
            var id = await repository.CreateCustomer(request.ToCustomer());
            return new CreateCustomerResponse { Id = id.ToString() };
        }

        public override async Task<DeleteCustomerResponse> DeleteCustomer(DeleteCustomerRequest request,
            ServerCallContext context)
        {
            if (await repository.DeleteCustomer(Guid.Parse(request.Id)))
            {
                return new DeleteCustomerResponse { Id = request.Id, Result = "SUCCESS" };
            }

            return new DeleteCustomerResponse { Id = string.Empty, Result = "FAILURE" };
        }

        public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request,
            ServerCallContext context)
        {
            var existing = await repository.GetCustomer(Guid.Parse(request.Id));
            if (existing == null)
            {
                return new UpdateCustomerResponse { Result = "FAILURE", Message = "Not found" };
            }

            //
            // Add more validations here
            //

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                existing.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                existing.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                existing.Phone = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.Address))
                existing.Address = request.Address;

            if (await repository.UpdateCustomer(existing))
                return new UpdateCustomerResponse { Result = "SUCCESS", Message = "Customer is updated" };

            return new UpdateCustomerResponse { Result = "FAILURE", Message = "Internal error" };

        }

        public override async Task<DeleteAllCustomersResponse> DeleteAllCustomers(DeleteAllCustomersRequest request,
            ServerCallContext context)
        {
            if (await repository.DeleteAllCustomers())
                return new DeleteAllCustomersResponse { Result = "SUCCESS", Message = "All customers deleted" };

            return new DeleteAllCustomersResponse { Result = "FAILURE", Message = "Internal error" };

        }

    }

}
