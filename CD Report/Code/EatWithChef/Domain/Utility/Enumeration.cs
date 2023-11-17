using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utility
{
    public enum UserRoleEnum
    {
        Admin = 1,
        Chef = 2,
        Staff = 3,
        Customer = 4
    }

    public enum OrderStatusEnum
    {
        WaitForProcess = 1,
        Confirmed = 2,
        WaitForDelivery = 3,
        Delivered = 4,
        Cancel = 0
    }

    public enum PaymentStatusEnum {
        UnPay = 1,
        Paid = 2,
        Error = 0
    }

    public enum BillStatusEnum { 
        Deleted = 0,
        New = 1,
        Complete = 2
    }
    public enum CustomerTypeEnum { 
        Normal = 1,
        VIP = 2
    }
}
