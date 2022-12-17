using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesCurveController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<RatesCurveController> _logger;
    public RatesCurveController(ILogger<RatesCurveController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetRateCurveList")]
    public List<RateCurve> Get()
    {
        Console.WriteLine("Get all RateCurves.");
        List<RateCurve> RateCurves= db.RateCurve.ToList<RateCurve>();
        return RateCurves;
    }

    [HttpPost(Name = "AddARateCurve")]
    public RateCurve Post(RateCurve aRateCurve)
    {
        Console.WriteLine("Add a RateCurve.");
        RateCurve existing = db.RateCurve.FirstOrDefault<RateCurve>(b => b.Name.Equals(aRateCurve.Name));
        if(existing == null) 
        {
            RateCurve lastBeforeAdd = db.RateCurve.OrderBy(b => b.Id).LastOrDefault<RateCurve>();
    
            if (lastBeforeAdd == null)
            {
                aRateCurve.Id = 1;
            }
            else
            {
                aRateCurve.Id = lastBeforeAdd.Id + 1;
            }
            db.RateCurve.Add(aRateCurve);
            db.SaveChanges();
            return db.RateCurve.OrderBy(b => b.Id).Last<RateCurve>();
        }
        else
        {
            Console.WriteLine("An existing record found. not adding");
            return null;
        }
    }
}