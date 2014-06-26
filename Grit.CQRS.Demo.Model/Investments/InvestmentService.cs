﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS.Demo.Model.Investments
{
    public class InvestmentService : IInvestmentService
    {
        private IInvestmentService _repository;
        public InvestmentService(IInvestmentService repository)
        {
            _repository = repository;
        }

        public Investment Get(int id)
        {
            return _repository.Get(id);
        }
    }
}
