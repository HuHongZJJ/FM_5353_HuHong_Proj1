using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace HW6_api.Models
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
        public DateTime EvalationDate{get;set;}
        public DateTime ExpirationDate {get; set;}

        public double Strike{get; set;}
        public bool IsCall {get; set;}
        public double PayOut{get; set;}
        public string KnockType{get; set;}
        public double BarrierLevel{get; set;}
    }
    public class ExchangeCmt
    {
        public string ExchangeName{get;set;}
        public string ExchangeSymbol{get;set;}
    }
    public class  MarketCmt
    {
        public string MarketName{get;set;}
        public int ExchangeId{get;set;}
    }
    public class UnitCmt
    {
        public string UnitType{get;set;}
        public int Quantity{get;set;}
    }
    public class Underlyingcmt
    {
        public int MarketId{get;set;}
        public string Symbol{get;set;}
        public string name{get;set;}
        public int UnitId{get;set;}
    }
    public class OptionCmt
    {
        public int UnderlyingId{get;set;}
        public string OptionType {get;set;}
        public double Strike{get; set;}
        public double Volatility{get; set;}
        public bool IsCall {get; set;}
        public string KnockType{get; set;}
        public double PayOut{get; set;}
        public double BarrierLevel{get; set;}
    }
    public class FutureCmt
    {
        public int UnderlyingId{get;set;}
        public DateTime ExpirationDate{get;set;}
    }
    public class TradeCmt
    {
        public int InstrumentId{get;set;}
        public string InstrumentType{get;set;}
        public int TradeQuantity{get;set;}
        public double TradePrice{get;set;}
        public DateTime TradeDate{get;set;}
    }
    public class EvaluationCmt
    {
        public int TradeId{get;set;}
        public int RatePointId{get;set;}
        public double S0 {get;set;}
        public DateTime EvaluationDate{get;set;}
        public int Steps{get; set;}
        public int Simulations{get; set;}
        public bool IsThread{get; set;}
        public bool IsAntithetic{get; set;}
        public bool IsDeltaCV{get; set;}
    }
    public class RatePointCmt
    {
        public int RateCurveId{get;set;}
        public double Term{get;set;}
        public double r{get; set;}
        public DateTime RecordDate{get;set;}
    }
    public class RateCurveCmt
    {
        public string RateCurveName{get;set;}
        public string RateCurveSymbol{get;set;}
    }
}