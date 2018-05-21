using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class CreateTrainingProgramsTable : ICreateTable
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        public CreateTrainingProgramsTable(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable()
        {
            try
            {
                CreateTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private void CreateTable()
        {
            Console.WriteLine("Creating Training Programs table");
            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                     new AttributeDefinition
                    {
                        AttributeName = "Title",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType= "Hash" // partitional key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Title",
                        KeyType = "Range" // sort key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = "TrainingPrograms"
            };
            var responce = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady();
        }

        public void WaitUntilTableReady()
        {
            string status = null;
            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = "TrainingPrograms"
                    });
                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            while (status != "ACTIVE");
            {

                Console.WriteLine("Table created successfully");
            }
        }
    }
}
