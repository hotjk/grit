﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Demo.Contracts.Commands
{
    public class InvestmentCreateCommand : Command
    {
        public int InvestmentId { get; set; }
        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
    }
}
