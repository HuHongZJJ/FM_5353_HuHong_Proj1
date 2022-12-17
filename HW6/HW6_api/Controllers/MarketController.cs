using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using Microsoft.EntityFrameworkCore;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<MarketController> _logger;
    public MarketController(ILogger<MarketController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMarketList")]
    public List<Market> Get()
    {
        Console.WriteLine("Get all Markets.");
        List<Market> markets= db.Market.ToList<Market>();
        return markets;
    }
    [HttpPost(Name = "AddAMarket")]
    public Market Post(Market aMarket)
    {
        Console.WriteLine("Add a Market.");
        Market existing = db.Market.FirstOrDefault<Market>(e => e.MarketName.Equals(aMarket.MarketName));
        if(existing  == null)
        {
            Market lastBeforeAdd = db.Market.OrderBy(e => e.Id).LastOrDefault<Market>();
            if (lastBeforeAdd == null)
            {
                aMarket.Id = 1;
            }
            else
            {
                aMarket.Id = lastBeforeAdd.Id + 1;
            }
            Exchange existing1 = db.Exchange.FirstOrDefault<Exchange>(m => m.Id.Equals(aMarket.Exchange.Id));
            if (existing1 == null)
            {
                //existing1 = db.Exchange.FirstOrDefault<Market>(e => e.ExchangeName.Equals(aMarket.Exchange.ExchangeName));
                if (existing1 != null)
                {
                    aMarket.Exchange = existing1;
                }
                else
                {

                }
            }
            else
            {
                aMarket.Exchange = existing1;
            }
            db.Market.Add(aMarket);
            db.SaveChanges();
            return db.Market.OrderBy(m => m.Id).Include(e => e.Exchange).Last<Market>();
        }
        else
        {
            Console.WriteLine("An existing record found. not adding");
            return null;
        }
    }
}