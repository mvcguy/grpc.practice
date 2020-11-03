using System;

namespace GrpcService1.Services.CustomerSrv
{
    public class Customer
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }
    }

}
