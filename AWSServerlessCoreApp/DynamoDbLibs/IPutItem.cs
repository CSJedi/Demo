using System.Threading.Tasks;

namespace AWSServerlessCoreApp.DynamoDbLibs
{
    public interface IPutItem
    {
        Task AddNewEntry(int id, string name);
    }
}
