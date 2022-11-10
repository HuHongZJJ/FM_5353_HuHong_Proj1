using Microsoft.AspNetCore.Mvc;
using HW4.Models;


namespace HW4.Controllers;

[ApiController]
[Route("[controller]")]
public class OptionPricingController : ControllerBase
{
    private readonly ILogger<OptionPricingController> _logger;

    public OptionPricingController(ILogger<OptionPricingController> logger)
    {
        _logger = logger;
    }
    
    
    [HttpPost]
    public OptionResult Post(OptData ds)
    {   
        
        OptionResult results =  new OptionResult();
        try
        {   
            int CPUnum = System.Environment.ProcessorCount;
            int SimulationMultiplier = 1;
            if (ds.OptionType.ToLower() == "european")
            {
                Simulator.European O = new Simulator.European();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                EuropeanOption euro = new EuropeanOption();            
                
                S.S0 = ds.S0;
                O.Strike = ds.Strike;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                O.IsCall = ds.IsCall;
                S.IsAntithetic = ds.IsAntithetic;
                S.IsDeltaCV = ds.IsDeltaCV;
                S.IsThread = ds.IsThread;

                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;
                results.Price = euro.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = euro.DeltaandGamma(O);
                results.Theta = euro.ThetaValue(O);
                results.Vega = euro.VegaValue(O);
                results.Rho = euro.RhoValue(O);
                results.StdErrorNorm = euro.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = euro.StandardError(O, true);
                }
            }
            else if (ds.OptionType.ToLower() == "asian")
            {
                Simulator.Aisan O = new Simulator.Aisan();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                AsianOption asia = new AsianOption(); 

                                
                S.S0 = ds.S0;
                O.Strike = ds.Strike;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                O.IsCall = ds.IsCall;
                S.IsAntithetic = ds.IsAntithetic;
                S.IsDeltaCV = ds.IsDeltaCV;
                S.IsThread = ds.IsThread;

                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;

                results.Price = asia.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = asia.DeltaandGamma(O);
                results.Theta = asia.ThetaValue(O);
                results.Vega = asia.VegaValue(O);
                results.Rho = asia.RhoValue(O);
                results.StdErrorNorm = asia.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = asia.StandardError(O, true);
                }
            }

            else if (ds.OptionType.ToLower() == "digital")
            {
                Simulator.Digital O = new Simulator.Digital();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                DigitalOption digi = new DigitalOption(); 
                
                S.S0 = ds.S0;
                O.Strike = ds.Strike;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                O.IsCall = ds.IsCall;
                S.IsAntithetic = ds.IsAntithetic;
                S.IsDeltaCV = ds.IsDeltaCV;
                S.IsThread = ds.IsThread;
                O.PayOut = ds.PayOut;

                
                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;
                                
                results.Price = digi.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = digi.DeltaandGamma(O);
                results.Theta = digi.ThetaValue(O);
                results.Vega = digi.VegaValue(O);
                results.Rho = digi.RhoValue(O);
                results.StdErrorNorm = digi.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = digi.StandardError(O, true);
                }

            }

            else if (ds.OptionType.ToLower() == "barrier")
            {
                Simulator.Barrier O = new Simulator.Barrier();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                BarrierOption barrier = new BarrierOption(); 

                                
                S.S0 = ds.S0;
                O.Strike = ds.Strike;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                O.IsCall = ds.IsCall;
                O.BarrierLevel = ds.BarrierLevel;
                if (ds.KnockType.ToLower() == "up and in")
                {
                    O.KnockType = Simulator.KnockType.UpAndIn;
                }
                else if (ds.KnockType.ToLower()== "up and out")
                {
                    O.KnockType = Simulator.KnockType.UpAndOut;
                }
                if (ds.KnockType.ToLower() == "down and in")
                {
                    O.KnockType = Simulator.KnockType.DownAndIn;
                }
                if (ds.KnockType.ToLower() == "down and out")
                {
                    O.KnockType = Simulator.KnockType.DownAndOut;
                }
                S.IsAntithetic = ds.IsAntithetic;
                S.IsDeltaCV = ds.IsDeltaCV;
                S.IsThread = ds.IsThread;

                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;

                results.Price = barrier.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = barrier.DeltaandGamma(O);
                results.Theta = barrier.ThetaValue(O);
                results.Vega = barrier.VegaValue(O);
                results.Rho = barrier.RhoValue(O);
                results.StdErrorNorm = barrier.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = barrier.StandardError(O, true);
                }
            }

            else if (ds.OptionType.ToLower() == "lookback")
            {
                Simulator.Lookback O = new Simulator.Lookback();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                LookbackOption lb = new LookbackOption();
                S.S0 = ds.S0;
                O.Strike = ds.Strike;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                O.IsCall = ds.IsCall;
                S.IsAntithetic = ds.IsAntithetic;
                S.IsDeltaCV = ds.IsDeltaCV;
                S.IsThread = ds.IsThread;
                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;

                results.Price = lb.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = lb.DeltaandGamma(O);
                results.Theta = lb.ThetaValue(O);
                results.Vega = lb.VegaValue(O);
                results.Rho = lb.RhoValue(O);
                results.StdErrorNorm = lb.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = lb.StandardError(O, true);
                }
            }
            else if (ds.OptionType.ToLower() == "range")
            {
                Simulator.Range O = new Simulator.Range();
                Simulator.SimulationParameters S = new Simulator.SimulationParameters();
                RangeOption rg = new RangeOption(); 
                S.S0 = ds.S0;
                O.Tenor = ds.Tenor;
                S.r = ds.r;
                S.Volatility = ds.Volatility;
                S.IsAntithetic = ds.IsAntithetic;
                S.IsThread = ds.IsThread;
                if (S.IsThread == true)
                {
                    SimulationMultiplier = SimulationMultiplier * CPUnum;
                }
                S.Simulations = ds.Simulations * SimulationMultiplier;
                S.Steps = ds.Steps;
                O.S = S;
            
                results.Price = rg.OptionPandStd(O).VP;
                (results.Delta,results.Gamma) = rg.DeltaandGamma(O);
                results.Theta = rg.ThetaValue(O);
                results.Vega = rg.VegaValue(O);
                results.Rho = rg.RhoValue(O);
                results.StdErrorNorm = rg.StandardError(O, false);
                if (ds.IsAntithetic == true)
                {
                    results.StdErrorReduce = rg.StandardError(O, true);
                }

            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
        return results;
    }
}
