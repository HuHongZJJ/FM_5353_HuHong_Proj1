using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UnderlyingController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<UnderlyingController> _logger;
    public UnderlyingController(ILogger<UnderlyingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetUnderlyingList")]
    public List<Underlying> Get()
    {
        Console.WriteLine("Get all Underlyings.");
        List<Underlying> Underlyings= db.Underlying.ToList<Underlying>();
        return Underlyings;
    }
    [HttpPost(Name = "AddAUnderlying")]
    public Underlying Post(Underlying aUnderlying)
    {
        Console.WriteLine("Add a Underlying.");
        List<Underlying> allRecords = db.Underlying.ToList<Underlying>();
        bool hasAMatch = false;
        foreach(Underlying record in allRecords){
            if (aUnderlying.name.Equals(record.name))
            {
                hasAMatch = true;
                break;
            }
            else if (aUnderlying.Symbol.Equals(record.Symbol))
            {
                hasAMatch = true;
                break;
            }
        }
        if(!hasAMatch) { 
            Underlying lastBeforeAdd = db.Underlying.OrderBy(b => b.Id).Last<Underlying>();
            aUnderlying.Id = lastBeforeAdd.Id + 1;
            db.Underlying.Add(aUnderlying);
            db.SaveChanges();
            return db.Underlying.OrderBy(b => b.Id).Last<Underlying>();
        }
        else
        {
            return null;
        }   
    }
}