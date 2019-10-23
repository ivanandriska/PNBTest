using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.Api.Models
{
    public class SmsItem
    {
        public long Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsComplete
        {
            get;
            set;
        }

        public Int16 IdUsuario
        {
            get;
            set;
        }
    }
}