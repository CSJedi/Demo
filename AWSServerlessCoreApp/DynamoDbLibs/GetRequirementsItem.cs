using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AWSServerlessCoreApp.Models;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class GetRequirementsItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetRequirementsItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<List<Requirement>> GetItems(int? id)
        {
            var query = RequestBuilder(id);
            var result = await ScanAsync(query);
            return result.Items.Select(Map).ToList();
        }

        private Requirement Map(Dictionary<string, AttributeValue> result)
        {
            return new Requirement
            {
                ProgramId = Convert.ToInt32(result["ProgramId"].N),
                Description = result["Description"].S
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
                    TableName = "Requirements"
                };
            }
            return new ScanRequest
            {
                TableName = "Requirements",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_ProgramId", new AttributeValue{N = id.ToString()} }
                },
                FilterExpression = "ProgramId = :v_ProgramId",
                ProjectionExpression = "ProgramId, Description"
            };
        }
    }
}
