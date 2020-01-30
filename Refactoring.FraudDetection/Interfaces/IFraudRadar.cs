using Refactoring.FraudDetection.Models;
using System.Collections.Generic;

namespace Refactoring.FraudDetection.Interfaces
{
    public interface IFraudRadar
    {
        IEnumerable<FraudResult> Check();
    }
}
