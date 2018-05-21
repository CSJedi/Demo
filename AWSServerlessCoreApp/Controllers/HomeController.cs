using Microsoft.AspNetCore.Mvc;
using AWSServerlessCoreApp.Models;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System;
using Amazon.DynamoDBv2;
using System.Linq;

namespace AWSServerlessCoreApp.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        public HomeController(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public IActionResult Index()
       {
            var technologies = _dynamoClient.ScanAsync(new ScanRequest { TableName = "Technologies" })
                .Result.Items.Select(MapTechnology).ToList();
            var requirements = _dynamoClient.ScanAsync(new ScanRequest { TableName = "Requirements" })
                .Result.Items.Select(MapRequirements).ToList();
            var trainingProgram = _dynamoClient.ScanAsync(new ScanRequest { TableName = "TrainingPrograms" })
                .Result.Items.Select(MapTrainingProgram).ToList()[0];

            trainingProgram.Technologyies = technologies;
            trainingProgram.Requirements = requirements;

            return View(new List<TrainingProgram> { trainingProgram });
        }

        private Technology MapTechnology(Dictionary<string, AttributeValue> result)
        {
            return new Technology
            {
                Id = Convert.ToInt32(result["Id"].N),
                Name = result["Name"].S
            };
        }

        private Requirement MapRequirements(Dictionary<string, AttributeValue> result)
        {
            return new Requirement
            {
                Id = Convert.ToInt32(result["Id"].N),
                Description = result["Description"].S
            };
        }

        private TrainingProgram MapTrainingProgram(Dictionary<string, AttributeValue> result)
        {
            return new TrainingProgram
            {
                Id = Convert.ToInt32(result["Id"].N),
                Title = result["Title"].S
            };
        }
    }
}