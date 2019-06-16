using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage25.Services
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
