﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Models;

namespace BBL.Interfaces
{
    public interface ICategoryService
    {
       List<CategoryTypeModel> GetTypeModels();
    }
}
