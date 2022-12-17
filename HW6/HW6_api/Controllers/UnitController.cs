using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UnitController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<UnitController> _logger;
    public UnitController(ILogger<UnitController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetUnitList")]
    public List<Unit> Get()
    {
        Console.WriteLine("Get all Units.");
        List<Unit> Units= db.Unit.ToList<Unit>();
        return Units;
    }

    [HttpPost(Name = "AddAUnit")]
    public Unit Post(Unit aUnit)
    {
        Console.WriteLine("Add a Unit.");
        Unit existing = db.Unit.FirstOrDefault<Unit>(b => b.UnitType.Equals(aUnit.UnitType));
        if(existing == null) 
        {
            Unit lastBeforeAdd = db.Unit.OrderBy(b => b.Id).LastOrDefault<Unit>();
    
            if (lastBeforeAdd == null)
            {
                aUnit.Id = 1;
            }
            else
            {
                aUnit.Id = lastBeforeAdd.Id + 1;
            }
            db.Unit.Add(aUnit);
            db.SaveChanges();
            return db.Unit.OrderBy(b => b.Id).Last<Unit>();
        }
        else
        {
            Console.WriteLine("An existing record found. not adding");
            return null;
        }
    }
}