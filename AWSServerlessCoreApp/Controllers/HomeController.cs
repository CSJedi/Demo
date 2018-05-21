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

            List<TrainingProgram> trainingPrograms = _dynamoClient.ScanAsync(new ScanRequest { TableName = "TrainingPrograms" })
                .Result.Items.Select(MapTrainingProgram).ToList();

            foreach (TrainingProgram program in trainingPrograms)
            {
                program.Technologyies = _dynamoClient.ScanAsync(
                    new ScanRequest
                    {
                        TableName = "Technologies",
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v_ProgramId", new AttributeValue{N = program.Id.ToString()} }
                    },
                        FilterExpression = "ProgramId = :v_ProgramId",
                        ProjectionExpression = "ProgramId, TechnologyName"
                    }
                ).Result.Items.Select(MapTechnology).ToList();
                program.Requirements = _dynamoClient.ScanAsync(
                    new ScanRequest
                    {
                        TableName = "Requirements",
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v_ProgramId", new AttributeValue{N = program.Id.ToString()} }
                    },
                        FilterExpression = "ProgramId = :v_ProgramId",
                        ProjectionExpression = "ProgramId, Description"
                    }
                ).Result.Items.Select(MapRequirements).ToList();
            }

            return View(trainingPrograms);
        }

        private Technology MapTechnology(Dictionary<string, AttributeValue> result)
        {
            return new Technology
            {
                ProgramId = Convert.ToInt32(result["ProgramId"].N),
                TechnologyName = result["TechnologyName"].S
            };
        }

        private Requirement MapRequirements(Dictionary<string, AttributeValue> result)
        {
            return new Requirement
            {
                ProgramId = Convert.ToInt32(result["ProgramId"].N),
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