using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class CloseAllPositions
    {
        public List<long> Success { get; set; }
        public Array Failed { get; set; }
    }
}
