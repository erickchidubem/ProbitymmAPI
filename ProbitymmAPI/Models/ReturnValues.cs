using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Models
{
    public class ReturnValues
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class ReturnValuesBool
    {
        public int StatusCode { get; set; }
        public bool StatusFlag { get; set; }
        public string StatusMessage { get; set; }
    }
}