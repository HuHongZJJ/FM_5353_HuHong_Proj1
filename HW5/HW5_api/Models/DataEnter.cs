using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace HW5_api.Models
{ 
    public class OptData 
    {
        public string OptionType {get;set;}
        public double S0{get; set;}
        public double r{get; set;}
        public int Steps{get; set;}
        public int Simulations{get; set;}
        public double Volatility{get; set;}
        public bool IsThread{get; set;}
        public bool IsAntithetic{get; set;}
        public bool IsDeltaCV{get; set;}
        public double Tenor {get; set;}
        public double Strike{get; set;}
        public bool IsCall {get; set;}
        public double PayOut{get; set;}
        public string KnockType{get; set;}
        public double BarrierLevel{get; set;}
    }
}