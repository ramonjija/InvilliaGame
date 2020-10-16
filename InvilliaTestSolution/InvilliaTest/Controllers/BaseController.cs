using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InvilliaTest.Controllers
{
    public class BaseController : Controller
    {
        protected int GetUserId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "UserId").Value);
        }
        protected string GetUserRole()
        {
            return User.Claims.First(i => i.Type == ClaimTypes.Role).Value;
        }
    }
}
