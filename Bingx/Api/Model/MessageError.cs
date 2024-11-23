using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class MessageError<T>
    {
        public bool IsSucess { get; set; } = false;
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class MessageError
    {
        public bool IsSucess { get; set; } = false;
        public string Message { get; set; }
    }
}
