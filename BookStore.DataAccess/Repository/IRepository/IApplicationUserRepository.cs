﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Models;

namespace BookStore.DataAccess.Repository.IRepository;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser applicationUser);
}
