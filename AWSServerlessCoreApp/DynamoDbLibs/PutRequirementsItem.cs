using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class PutRequirementsItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutRequirementsItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(int id, string description)
        {
            var queryRequest = RequestBuilder(id, description);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string description)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"Description", new AttributeValue{S = description} }
            };
            return new PutItemRequest
            {
                TableName = "Requirements",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }
    }

}
