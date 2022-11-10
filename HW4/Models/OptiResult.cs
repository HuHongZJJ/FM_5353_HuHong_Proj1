using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW4.Models
{ 
    public class OptionResult 
    {
        public double Price {get; set;}
        public double Delta{get; set;}
        public double Theta{get; set;}
        public double Gamma{get; set;}
        public double Vega{get; set;}
        public double Rho{get; set;}
        public double StdErrorNorm{get; set;}
        public double StdErrorReduce{get; set;}
    }
}