using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AWSServerlessCoreApp.Models;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class GetTrainingProgramsItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetTrainingProgramsItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<TrainingProgram>> GetItems(int? id)
        {
            var query = RequestBuilder(id);
            var result = await ScanAsync(query);
            return result.Items.Select(Map).ToList();
        }

        private TrainingProgram Map(Dictionary<string, AttributeValue> result)
        {
            return new TrainingProgram
            {
                Id = Convert.ToInt32(result["Id"].N),
                Title = result["Title"].S
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoClient.ScanAsync(request);
            return response;
        }

        private ScanRequest RequestBuilder(int? id)
        {
            if (id.HasValue == false)
            {
                return new ScanRequest
                {
                    TableName = "TrainingPrograms"
                };
            }
            return new ScanRequest
            {
                TableName = "TrainingPrograms",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_id", new AttributeValue{N = id.ToString()} }
                },
                FilterExpression = "Id = :v_id",
                ProjectionExpression = "Id, Title"
            };
        }
    }
}
