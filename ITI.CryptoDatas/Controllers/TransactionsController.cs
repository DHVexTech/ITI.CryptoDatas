﻿using System;
using System.Linq;
using System.Security.Claims;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly TransactionsManager _transactionManager;

        public TransactionsController(TransactionsManager transactionsManager)
        {
        }

        [HttpPost("give")]
        [Authorize]
        public ActionResult<Transaction> Give([FromBody]Transaction transaction)
        {
            throw new NotImplementedException();
        }

    }
}