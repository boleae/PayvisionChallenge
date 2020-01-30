using Refactoring.FraudDetection.Interfaces;
using Refactoring.FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.FraudDetection.Services
{
    public class DetectFraudService : IDetectFraudService
    {
        public bool IsFraudOrder(List<Order> currentOrders, Order newOrder)
        {
            if (currentOrders.Any(o => o.DealId.Equals(newOrder.DealId) && o.Email.Equals(newOrder.Email) && !o.CreditCard.Equals(newOrder.CreditCard)))
                return true;
            if (currentOrders.Any(o => o.DealId.Equals(newOrder.DealId)
                        && o.State.Equals(newOrder.State)
                        && o.ZipCode.Equals(newOrder.ZipCode)
                        && o.Street.Equals(newOrder.Street)
                        && o.City.Equals(newOrder.City)
                        && !o.CreditCard.Equals(newOrder.CreditCard)))
                return true;
            return false;
        }
    }
}
