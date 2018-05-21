using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDB
{
    public interface IPutItem
    {
        Task AddNewEntry(int id, string replyDateTime);
    }
}
