﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders.Testing;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private readonly IDataAccess<Product> DBui = new ProductAccess();  // Instantiate Interface for DataAccess


    [HttpGet("/latest")]
    public Array Latest(int? pageStart, int? pageSize){ // GET pageStart and pageSize values

        Array products =  DBui.List(pageStart, pageSize).ToList().ToArray(); // Query *DB* for list of products

        return products; // Return latest products
    }


    [HttpGet("/euro")]
    public Array EuroConversion(int? pageStart, int? pageSize){ // GET pageStart and pageSize values

        decimal euroExchangeRate = 1.11m;

        List<Product> products = DBui.List(pageStart, pageSize).ToList(); // Query *DB* for list of products

        List<Product> euroArray = new List<Product>();

        for (int i=0; i<products.Count; i++) // Iterate through list
        {
            euroArray.Add(products[i].Clone()); // Clone Product object to euroArray
            euroArray[i].PriceInPounds *= euroExchangeRate;  // Perform conversion from GBP to EUR
            
        }
        return euroArray.ToArray(); // Return latest products
    }

}

        /* Potential improvements:

        1. Product price in pounds no longer applies when converting to Euro, could map the object to another with price in Euros.
        2. Sort by name or sort by price
        3. Search by name for specific product
        4. Add a few unit tests to test the controller
        */

   


    /*
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        var rng = new Random();
        return Enumerable.Range(1, pageSize).Select(index => new Product
            {
                PriceInPounds = rng.Next(0, 10),
                Name = Products[rng.Next(Products.Length)]
            })
            .ToArray();
    }
    */
