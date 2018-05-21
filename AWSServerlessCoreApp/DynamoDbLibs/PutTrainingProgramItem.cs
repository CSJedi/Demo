using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class PutTrainingProgramItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public PutTrainingProgramItem(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task AddNewEntry(int id, string name)
        {
            var queryRequest = RequestBuilder(id, name);
            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string title)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"Title", new AttributeValue{S = title} }
            };
            return new PutItemRequest
            {
                TableName = "TrainingPrograms",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoDbClient.PutItemAsync(request);
        }
    }

}
