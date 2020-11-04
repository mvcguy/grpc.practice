using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService1.Services.CustomerSrv
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

        public override async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var id = await repository.CreateCustomer(request.ToCustomer());
            return new CreateCustomerResponse { Id = id.ToString() };
        }

        public override async Task<DeleteCustomerResponse> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            if (await repository.DeleteCustomer(Guid.Parse(request.Id)))
            {
                return new DeleteCustomerResponse { Id = request.Id, Result="SUCCESS" };
            }

            return new DeleteCustomerResponse { Id = string.Empty, Result="FAILURE" };
        }
    }

}
