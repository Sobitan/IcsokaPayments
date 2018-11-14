using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcsokaPayments.Web.Models
{
    public class ConfirmModel
    {
        public ConfirmModel()
        {
            Warnings = new List<string>();
        }
        public List<string> Warnings { get; set; }
    }
}