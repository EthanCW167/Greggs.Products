using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Greggs.Products.UnitTests;

/* 
        Test Improvements:

        1. Check API response for correct Json content type
        2  Check response time for API is within acceptable limits
        3. Add tests to test optional arguments for API endpoints
        
*/

public class Product // Model Product object
    {
        public string Name { get; set; }
        public decimal PriceInPounds { get; set; }

        public Product Clone() // Create clone of Product Object with new reference
        {
            return new Product {Name = this.Name, PriceInPounds = this.PriceInPounds};
        }

        public bool Equals(Product other)
        {
            if (other is null)
                return false;
            
            return this.Name == other.Name && this.PriceInPounds == other.PriceInPounds;
        }

        public override bool Equals(object obj) => Equals(obj as Product); // Override Equals operation to allow object values comparison
        public override int GetHashCode() => (Name, PriceInPounds).GetHashCode();
}

public class EndpointUnitTest
{

    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:5001") }; // Establish client on local hostr

    public class TestHelper 
    {
        public static async Task AssertResponseMatchContent<Array>(HttpResponseMessage response, Array expectedContent)
        {

            JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true }; // Set Json serializer options
            
            Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<Array>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions)); 
            // Check if content from endpoint is as expected

        }
    }

    [Fact]
    public async Task TestLatestEndpointMatch() // Test if the latest endpoint returns expected content
    {

        var ProductDatabase = new List<Product> // Create expected content Array
        {
        new() { Name = "Sausage Roll", PriceInPounds = 1m },
        new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
        new() { Name = "Steak Bake", PriceInPounds = 1.2m },
        new() { Name = "Yum Yum", PriceInPounds = 0.7m },
        new() { Name = "Pink Jammie", PriceInPounds = 0.5m },
        new() { Name = "Mexican Baguette", PriceInPounds = 2.1m },
        new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m },
        new() { Name = "Coca Cola", PriceInPounds = 1.2m }
        }.ToArray();

        var response = await _httpClient.GetAsync("/latest"); // GET request to latest endpoint

        await TestHelper.AssertResponseMatchContent(response, ProductDatabase); // Call helper function
    }

    [Fact]
    public async Task TestEuroEndpointMatch() // Test if the euro endpoint returns expected content
    {
       var ProductDatabase = new List<Product> // Create expected content Array
        {
        new() { Name = "Sausage Roll", PriceInPounds = 1.11m },
        new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.221m },
        new() { Name = "Steak Bake", PriceInPounds = 1.332m },
        new() { Name = "Yum Yum", PriceInPounds = 0.777m },
        new() { Name = "Pink Jammie", PriceInPounds = 0.555m },
        new() { Name = "Mexican Baguette", PriceInPounds = 2.331m },
        new() { Name = "Bacon Sandwich", PriceInPounds = 2.1645m },
        new() { Name = "Coca Cola", PriceInPounds = 1.332m }
        }.ToArray(); 

        var response = await _httpClient.GetAsync("/euro"); // GET request to euro endpoint

        await TestHelper.AssertResponseMatchContent(response, ProductDatabase); // Call helper function
        
    }
}





