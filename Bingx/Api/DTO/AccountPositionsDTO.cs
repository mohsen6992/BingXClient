using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class AccountPositionsResponseDTO
    {
        public long code { get; set; }
        public string msg { get; set; }
        public List<AccountPositions> data { get; set; }
    }

}
