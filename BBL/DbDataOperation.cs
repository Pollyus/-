﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BAL.Models;
using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using BLL.Models;
using BBL.Models;

namespace BAL
{
    public class DbDataOperation : IDbCrud
    {
        IDbRepos db;

        public DbDataOperation(IDbRepos repos)
        {
            db = repos;
        }

        public List<ProductModel> GetAllProducts()
        {
            return db.Products.GetList().Select(i => new ProductModel(i)).ToList();
        }

        public void DeleteCart(int id)
        {
            ShoppingCart cart = db.ShoppingCarts.GetItem(id);
            if (cart != null)
            {
                db.ShoppingCarts.Delete(cart.Id);
                Save();
            }
        }
        public void CreateCart(CartModel cart)
        {
            ShoppingCart sc = new ShoppingCart();
            var allcart = db.ShoppingCarts.GetList().Where(i => i.BuyerId == cart.BuyerId && i.ProductId == cart.ProductId).ToList();
            if (allcart.Count != 0) return;

            sc.Amount = 1;
            sc.BuyerId = cart.BuyerId;
            sc.ProductId = cart.ProductId;
            db.ShoppingCarts.Create(sc);
            Save();
        }
        public void UpdateCart(CartModel cart)
        {
            ShoppingCart sc = db.ShoppingCarts.GetItem(cart.Id);
            sc.Amount = cart.Amount;
            Save();
        }
        public void CreateUser(UserModel user, BuyerModel buyer)
        {
            //?????
            User us = new User();
            Buyer bu = new Buyer();
            //user.User_Status_Id = 4;
            bu.Sum = 0;
            us.Login = user.Login;
            us.Name = user.Name;
            us.Password = user.Password;
            bu.Email = buyer.Email;

            db.Users.Create(us);
            Save();

            int cuid = db.Users.GetList().Where(i => i.Login == user.Login).ToList()[0].Id;
            User_Sale sale = new User_Sale();
            sale.UserId = cuid;
            sale.SaleId = 4; //?
            sale.Used = false;

            db.UserSales.Create(sale);
            Save();
        }
        

        public List<OrderModel> GetAllOrders()
        {
            return db.Orders.GetList().Select(i => new OrderModel(i)).ToList();
        }
        public OrderModel GetOrder(int Id)
        {
            return new OrderModel(db.Orders.GetItem(Id));
        }

        public void DeleteOrder(int id)
        {
            if (db.Orders.GetItem(id) != null)
            {
                db.Orders.Delete(id);
                Save();
            }
        }

        public List<OrderLineModel> GetAllOrderLines()
        {
            return db.OrderLines.GetList().Select(i => new OrderLineModel(i)).ToList();
        }

        public List<CategoryModel> GetAllCategories()
        {
            return db.Categories.GetList().Select(i => new CategoryModel(i)).ToList();
        }

        public ProductModel GetProduct(int Id)
        {
            return new ProductModel(db.Products.GetItem(Id));
        }



        public void CreateProduct(ProductModel p)
        {
            db.Products.Create(new Product() { Id = 6, Cost = p.Cost, CategoryId = 1, Name = p.Name });
            Save();

        }

        public void UpdateProduct(ProductModel p)
        {
            Product pr = db.Products.GetItem(p.Id);
            pr.Name = p.Name;
            pr.Cost = p.Cost;
            pr.CategoryId = p.CategoryId;
            Save();
        }

        public void DeleteProduct(int id)
        {
            Product p = db.Products.GetItem(id);
            if (p != null)
            {
                db.Products.Delete(p.Id);
                Save();
            }
        }


        public bool Save()
        {
            if (db.Save() > 0) return true;
            return false;
        }



    }
}