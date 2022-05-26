using Ambulance.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambulance.Controllers
{
    public class BaseControllerClass : ControllerBase
    {
        protected ClaimData ExtractClaims()
        {


            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {

                return new ClaimData()
                {
                    Name = identity.FindFirst("Name").Value,
                    UserId = identity.FindFirst("UserId").Value,
                    Userrole = identity.FindFirst("Userrole").Value
                };
            }
            else
            {
                return null;
            }


        }
    }
}
