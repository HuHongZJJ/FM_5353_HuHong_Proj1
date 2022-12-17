using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<ExchangeController> _logger;
    public ExchangeController(ILogger<ExchangeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetExchangeList")]
    public List<Exchange> Get()
    {
        Console.WriteLine("Get all Exchanges.");
        List<Exchange> Exchanges= db.Exchange.ToList<Exchange>();
        return Exchanges;
    }

    [HttpPost(Name = "AddAExchange")]
    public Exchange Post(Exchange aExchange)
    {
        Console.WriteLine("Add a Exchange.");
        Exchange existing = db.Exchange.FirstOrDefault<Exchange>(b => b.ExchangeName.Equals(aExchange.ExchangeName));
        if(existing == null) 
        {
            Exchange lastBeforeAdd = db.Exchange.OrderBy(b => b.Id).LastOrDefault<Exchange>();
    
            if (lastBeforeAdd == null)
            {
                aExchange.Id = 1;
            }
            else
            {
                aExchange.Id = lastBeforeAdd.Id + 1;
            }
            db.Exchange.Add(aExchange);
            db.SaveChanges();
            return db.Exchange.OrderBy(b => b.Id).Last<Exchange>();
        }
        else
        {
            Console.WriteLine("An existing record found. not adding");
            return null;
        }
    }
}