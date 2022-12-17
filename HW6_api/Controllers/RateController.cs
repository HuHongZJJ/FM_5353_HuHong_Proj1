using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RateController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<RateController> _logger;
    public RateController(ILogger<RateController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetRateList")]
    public List<RatePoint> Get()
    {
        Console.WriteLine("Get all Rates.");
        List<RatePoint> Rate= db.RatePoint.ToList<RatePoint>();
        return Rate;
    }
    [HttpPost(Name = "AddARate")]
    public RatePoint Post(RatePoint aRate)
    {
        Console.WriteLine("Add a Rate.");
        List<RatePoint> allRecords = db.RatePoint.ToList<RatePoint>();
        bool hasAMatch = false;
        
        RatePoint lastBeforeAdd = db.RatePoint.OrderBy(b => b.Id).Last<RatePoint>();
        aRate.Id = lastBeforeAdd.Id + 1;
        db.RatePoint.Add(aRate);
        db.SaveChanges();
        return db.RatePoint.OrderBy(b => b.Id).Last<RatePoint>();
        
    }
}