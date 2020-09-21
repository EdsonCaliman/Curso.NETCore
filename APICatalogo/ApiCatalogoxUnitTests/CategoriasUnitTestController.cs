using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString =
           "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=root";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseMySql(connectionString)
               .Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        [Fact]
        public void GetAllCategorias_Returno_Ok()
        {
            //Arrange  
            var controller = new CategoriasController(repository, null, mapper);

            //Act  
            var data = controller.GetAll();

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Result.Value);
        }

        [Fact]
        public void GetCategoriaById_Return_OkResult()
        {
            var controller = new CategoriasController(repository, null, mapper);
            var catId = 2;

            var data = controller.GetId(catId);
            Console.WriteLine(data);

            Assert.IsType<CategoriaDTO>(data.Result.Value);
        }

        [Fact]
        public void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, null, mapper);
            var catId = 9999;

            //Act  
            var data = controller.GetId(catId);

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result.Result);
        }
    }
}
