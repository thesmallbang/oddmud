using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddMud.Web.Game.Database;
using OddMud.Web.Game.Database.Entities;

namespace OddMud.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        public IActionResult Seed()
        {
            using (var context = new GameDbContext())
            {
                if (!context.Elements.Any())
                {
                    context.Elements.Add(new DbElement()
                    {
                        Name = "Physical",
                        TextColor = View.MudLike.TextColor.Gray,
                        Ranges = new List<DbElementRange>() {
                            new DbElementRange() { Text = "hits", TextColor = View.MudLike.TextColor.Red }
                        }
                    });
                    context.SaveChanges();
                }
            }
            return new OkResult();
        }
    }
}