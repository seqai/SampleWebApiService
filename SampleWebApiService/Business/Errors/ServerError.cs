using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApiService.Business.Errors
{
    public class NoSuchItemError : IBusinessError
    {
        public NoSuchItemError(string message = "No such item exits")
        {
            Message = message;
        }

        public string Message { get; }
    }
}
