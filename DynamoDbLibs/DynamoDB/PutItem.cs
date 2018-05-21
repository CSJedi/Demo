using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WSServerlessCoreApp.DynamoDbLibs
{
    public class PutItem: IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(int id, string replyDateTime)
        {
            var queryRequest = RequestBuilder(id, replyDateTime);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string replyDataTime)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"ReplyDateTime", new AttributeValue{N = replyDataTime} }
            };
            return new PutItemRequest
            {
                TableName = "TenpDynamoDbTable",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }
    }

}
