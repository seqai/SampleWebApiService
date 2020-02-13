using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiService.Business.Errors
{
    public interface IBusinessError
    {
        string Message { get; }
    }
}
