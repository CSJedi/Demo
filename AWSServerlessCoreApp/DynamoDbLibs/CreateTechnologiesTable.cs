﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public class CreateTechnologiesTable : ICreateTable
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        public CreateTechnologiesTable(IAmazonDynamoDB dynamoDbClient)
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
            Console.WriteLine("Creating Technologies table");
            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "ProgramId",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "TechnologyName",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "ProgramId",
                        KeyType= "Hash" // partitional key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "TechnologyName",
                        KeyType = "Range" // sort key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 1
                },
                TableName = "Technologies"
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
                        TableName = "Technologies"
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
