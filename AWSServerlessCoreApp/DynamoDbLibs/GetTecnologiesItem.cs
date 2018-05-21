using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AWSServerlessCoreApp.Models;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class GetTecnologiesItem 
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetTecnologiesItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<Technology>> GetItems(int? id)
        {
            var query = RequestBuilder(id);
            var result = await ScanAsync(query);
            return result.Items.Select(Map).ToList();
        }

        private Technology Map(Dictionary<string, AttributeValue> result)
        {
            return new Technology
            {
                Id = Convert.ToInt32(result["Id"].N),
                Name = result["Name"].S
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
                    TableName = "Technologies"
                };
            }
            return new ScanRequest
            {
                TableName = "Technologies",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_id", new AttributeValue{N = id.ToString()} }
                },
                FilterExpression = "Id = :v_id",
                ProjectionExpression = "Id, Name"
            };
        }
    }
}
