using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AdminProject.Services.Interface;
using Sihirdar.WebService.Provider.RiotApi.Model.Misc;

namespace AdminProject.Controllers
{
    [RoutePrefix("api/riot")]
    public class RiotApiController : ApiController
    {
        private readonly IRiotService _riotService;

        public RiotApiController(IRiotService riotService)
        {
            _riotService = riotService;
        }

        [Route("what-your-k")]
        [HttpGet]
        public IHttpActionResult WhatYourK(string name, Region region = Region.tr)
        {
            var player = _riotService.GetWhatYourK1(name, region);

            return Ok(player);
        }
    }
}