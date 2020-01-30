using Refactoring.FraudDetection.Models;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Interfaces
{
    public interface IDetectFraudService
    {
        bool IsFraudOrder(List<Order> currentOrders, Order newOrder);

    }
}
