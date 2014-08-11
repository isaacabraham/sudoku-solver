using Lydian.SodokuSolver.Website;
using Microsoft.FSharp.Core;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace Lydian.SodokuSolver.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            app.UseStaticFiles("/Web");
            app.UseWebApi(config);
        }
    }
    public class SodokuController : ApiController
    {
        public class Cell
        {
            public int? Value { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public async Task<Cell[]> Post([FromBody] Cell[][][][] cells)
        {
            var cellsAll = cells.SelectMany(c =>
                               c.SelectMany(d =>
                               d.SelectMany(e =>
                               e.Select(f => f))))
                                .Select((cell, index) => Solver.convertToCell(index, cell.X, cell.Y, cell.Value))
                                .ToArray();

            var result =
                Solver.SolvePuzzleFromCells(cellsAll)
                      .Select(cell =>
                      {
                          var value = default(int?);
                          if (FSharpOption<int>.get_IsSome(cell.Value))
                              value = cell.Value.Value;
                          return new Cell { Value = value, X = cell.X, Y = cell.Y };
                      })
                      .ToArray();
            return result;
        }
    }
}
