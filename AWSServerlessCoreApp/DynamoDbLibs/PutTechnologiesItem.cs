using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class PutTechnologiesItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutTechnologiesItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(int id, string name)
        {
            var queryRequest = RequestBuilder(id, name);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string name)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"Name", new AttributeValue{S = name} }
            };
            return new PutItemRequest
            {
                TableName = "Technologies",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }
    }

}
