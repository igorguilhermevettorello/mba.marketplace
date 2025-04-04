using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Core.Services.Interfaces
{
    public interface IAppEnvironment
    {
        string WebRootPath { get; }
    }
}
