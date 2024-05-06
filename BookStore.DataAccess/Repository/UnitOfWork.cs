﻿using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    
    private ApplicationDbContext _context;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public ICompanyRepository Company { get; private set; }
    public IShoppingCartRepository ShoppingCart { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Product = new ProductRepository(_context);
        Category = new CategoryRepository(_context);
        Company = new CompanyRepository(_context);
        ShoppingCart = new ShoppingCartRepository(_context);
        ApplicationUser = new ApplicationUserRepository(_context);
        OrderDetail = new OrderDetailRepository(_context);
        OrderHeader = new OrderHeaderRepository(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}

