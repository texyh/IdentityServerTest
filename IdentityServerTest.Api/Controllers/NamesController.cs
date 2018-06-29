using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerTest.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Names")]
    [Authorize]
    public class NamesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> GetNames()
        {
            return new List<string> { "Emeka", "Victor", "Allen", "Bayo" };
        }
    }
}