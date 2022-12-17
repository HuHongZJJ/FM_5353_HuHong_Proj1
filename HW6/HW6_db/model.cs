using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations.Schema; 
using System.Linq;
using Npgsql;

namespace HW6
{
    public class OptionContext: DbContext
    {
        public string constring= "Host=localhost;Database=fm5353_hw6;Username=postgres;Password=980217";
        public DbSet<Market> Market {get;set;}
        public DbSet<Exchange> Exchange {get;set;}
        public DbSet<Unit> Unit {get;set;}
        public DbSet<Instrument> Instrument {get;set;}
        public DbSet<Underlying> Underlying {get;set;}
        public DbSet<Option> Option {get;set;}
        public DbSet<OptionProperty> OptionType {get;set;}
        public DbSet<Future> Future {get;set;}
        public DbSet<Trade> Trade {get;set;}
        public DbSet<OptionEvaluation> OptionEvaluation {get;set;}
        public DbSet<UnderlyingEvaluation> UnderlyingEvaluation {get;set;}
        public DbSet<FutureEvaluation> FutureEvaluation {get;set;}
        public DbSet<RatePoint> RatePoint {get;set;}
        public DbSet<RateCurve> RateCurve {get;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(constring);   
    }


    [Table("Exchange")]
    public class Exchange 
    {
        public int Id {get;set;}
        public string ExchangeName {get; set;}
        public string ExchangeSymbol {get; set;}
    }   
    [Table("Market")]
    public class Market
    {
        public int Id{get; set;}
        public string MarketName{get; set;}
        public Exchange Exchange{get; set;}

    }
    [Table("Unit")]
    public class Unit
    {
        public int Id {get;set;}
        public string UnitType {get; set;}
        public int Quantity{get;set;}
    }
    [Table("Instrument")]
    public class Instrument
    {
        public int Id {get; set;}
        public string Symbol{get; set;}
        public Market Market{get; set;}
        public string name {get; set;}
        public Unit Unit{get; set;}
    }
    [Table("Underlying")]
    public class Underlying: Instrument
    {
        public int Id{get;set;}
    }
    [Table("Option")]
    public class Option:Instrument
    {
        public int Id {get;set;}
        public Underlying Underlying {get;set;}
        public DateTime Expirationdate {get;set;}
        public OptionProperty OP {get;set;}
        public double vol {get; set;}
    }
    [Table("OptionProperty")]
    public class OptionProperty
    {
        public int Id{get;set;}
        public string OptionType {get;set;}
        public bool IsCall {get;set;}
        public double PayOut {get; set;}
        public string KnockType {get;set;}
        public double BarrierLevel {get;set;}
    }
    [Table("Future")]
    public class Future:Instrument{
        public int Id {get;set;}
        public DateTime Expirationdate {get;set;}
        public Underlying Underlying {get;set;}

    }
    [Table("Trade")]
    public class Trade
    { 
        public int Id{get;set;}
        public DateTime tradeDate{get; set;}
        public int quantity {get;set;}
        public int instrumentId{get;set;}
        public string instrumentType{get;set;}
        public double price{get;set;}
    }
    [Table("OptionEvaluation")]
    public class OptionEvaluation
    {
        public int Id {get;set;}
        public Option option{get;set;}
        public DateTime dateEvl{get;set;}
        public int TradeId{get;set;}
        public double simulatePrice {get;set;}
        public double pnl{get;set;}
        public double Delta{get; set;}
        public double Theta{get; set;}
        public double Gamma{get; set;}
        public double Vega{get; set;}
        public double Rho{get; set;}
        public double StdErrorNorm{get; set;}
        public double StdErrorReduce{get; set;}
    }
    [Table("UnderlyingEvaluation")]

    public class UnderlyingEvaluation
    {
        public int Id{get;set;}
        public double marketPrice{get;set;}
        public int TradeId{get;set;}
        public DateTime dateEvl{get;set;}
        public double pnl {get;set;}
    }
    [Table("FutureEvaluation")]
    public class  FutureEvaluation
    {
        public int Id{get;set;}
        public double marketPrice{get;set;}
        public int TradeId{get;set;}
        public DateTime EvaluationDate{get;set;}
        public double pnl {get;set;}
    }

    [Table("RatePoint")]
    public class RatePoint
    {
        public int Id {get;set;}
        public int RateCurveId {get;set;}
        public double Rate {get; set;}
        public double Term{get;set;}
        public DateTime RecordDate{get;set;}
    }

    [Table("RateCurve")]
    public class RateCurve
    {
        public int Id{get;set;}
        public string Name{get;set;}
        public string Symbol{get;set;}
    }   
}