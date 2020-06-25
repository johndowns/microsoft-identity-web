﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
#if (OrganizationalAuth)
using Microsoft.AspNetCore.Authorization;
#endif
#if (GenerateApi)
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Net;
using System.Net.Http;
using Company.WebApplication1.Services;
#endif
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Company.WebApplication1.Models;

namespace Company.WebApplication1.Controllers
{
#if (OrganizationalAuth)
    [Authorize]
#endif
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

#if (GenerateApi)
        private readonly IDownstreamWebApi _downstreamWebApi;

        public HomeController(ILogger<HomeController> logger,
                              IDownstreamWebApi downstreamWebApi)
        {
             _logger = logger;
            _downstreamWebApi = downstreamWebApi;
       }

        [AuthorizeForScopes(ScopeKeySection = "CalledApi:CalledApiScopes")]
        public async Task<IActionResult> Index()
        {
            ViewData["ApiResult"] = await _downstreamWebApi.CallWebApi();

            // You can also specify the relative endpoint and the scopes
            // ViewData["ApiResult"] = await _downstreamWebApi.CallWebApi("me", new string[] {"user.read"});
            
            return View();
        }
#else
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

#endif
        public IActionResult Privacy()
        {
            return View();
        }

#if (OrganizationalAuth)
        [AllowAnonymous]
#endif
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
