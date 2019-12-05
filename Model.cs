using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    public class Model
    {
        public string Modeltype { get; set; }
        public int Pk { get; set; }
        public UserMessage UserMessage { get; set; }
        public TradeTemplate TradeTemplate { get; set; }

    }
}
