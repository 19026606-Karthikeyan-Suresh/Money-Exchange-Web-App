using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MoneyExchangeWebApp.Models;

namespace MoneyExchangeWebApp.Controllers
{
    [Route("api/HomeApi")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        [HttpGet("Convert/{fromCurr}/{toCurr}/{Amount}")]
        public IActionResult Convert(string fromCurr, string toCurr, int Amount)
        {

            ExchangeRates from = DBUtl.GetList<ExchangeRates>($"SELECT * FROM ExchangeRates WHERE QuoteCurrency = '{fromCurr}'").FirstOrDefault();
            ExchangeRates to = DBUtl.GetList<ExchangeRates>($"SELECT * FROM ExchangeRates WHERE QuoteCurrency = '{toCurr}'").FirstOrDefault();
            double amt = Amount * (to.ExchangeRate / from.ExchangeRate);
            return Ok(amt);
        }
    }
}
