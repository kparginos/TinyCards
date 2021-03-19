using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBank.Core
{
    public class ApiResult<T>
    {
        public int Code { get; set; }
        public string ErrorText { get; set; }
        public T Data { get; set; }

        public ApiResult()
        {
            Code = Constants.ApiResultCode.Success;
        }

        public bool IsSuccessful()
        {
            return Code == Constants.ApiResultCode.Success;
        }
    }
}
