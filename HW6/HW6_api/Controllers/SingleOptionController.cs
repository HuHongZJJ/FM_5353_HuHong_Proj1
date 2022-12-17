
using Microsoft.AspNetCore.Mvc;
using HW6_api.Models;
using HW6;

namespace HW6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class OptionController : ControllerBase
{
    OptionContext db = new OptionContext();
    private readonly ILogger<OptionController> _logger;
    public OptionController(ILogger<OptionController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetOptionList")]
    public List<Option> Get()
    {
        Console.WriteLine("Get all Options.");
        List<Option> Options= db.Option.ToList<Option>();
        return Options;
    }
    [HttpPost(Name = "AddAOption")]
    public Option Post(Option aOption)
    {
        Console.WriteLine("Add a Option.");
        List<Option> allRecords = db.Option.ToList<Option>();
        Option lastBeforeAdd = db.Option.OrderBy(b => b.Id).Last<Option>();
        aOption.Id = lastBeforeAdd.Id + 1;
        db.Option.Add(aOption);
        db.SaveChanges();
        return db.Option.OrderBy(b => b.Id).Last<Option>();
    }
}