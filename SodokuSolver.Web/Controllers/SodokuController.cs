using System.Web.Http;

namespace SodokuSolver.Web.Controllers
{
    public class SodokuController : ApiController
    {
        public string[] Get(string input)
        {
            //return Solver.SolvePuzzle(input)
            //             .Select(JsonConvert.SerializeObject)
            //             .ToArray();
            return null;
        }
    }
}