
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AWSServerlessCoreApp.DynamoDbLibs;

namespace AWSServerlessCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/DBManager")]
    public class DBManager :Controller
    {
        private readonly ICreateTable _createTable;
        private readonly PutTechnologiesItem _putTechnologiesItem;
        private readonly PutRequirementsItem _putRequirementsItem;
        private readonly PutTrainingProgramItem _putTrainingProgramItem;
        private readonly GetTecnologiesItem _getTecnologiesItem;
        private readonly GetRequirementsItem _getRequirementsItem;
        private readonly GetTrainingProgramsItem _getTrainingProgramsItem;
        public DBManager(ICreateTable createTable, 
            PutTechnologiesItem putTechnologiesItem,
            PutRequirementsItem putRequirementsItem,
            PutTrainingProgramItem putTrainingProgramItem,
            GetTecnologiesItem getTecnologiesItem, 
            GetRequirementsItem getRequirementsItem,
            GetTrainingProgramsItem getTrainingProgramsItem)
        {
            _createTable = createTable;
            _putTechnologiesItem = putTechnologiesItem;
            _putRequirementsItem = putRequirementsItem;
            _putTrainingProgramItem = putTrainingProgramItem;
            _getTecnologiesItem = getTecnologiesItem;
            _getTrainingProgramsItem = getTrainingProgramsItem;
            _getRequirementsItem = getRequirementsItem;
        }

        [Route("createTable")]
        public IActionResult CreateDynamoDbTable()
        {
            _createTable.CreateDynamoDbTable();
            return Ok();
        }

        [Route("putTechnologiesItem")]
        public IActionResult PutTechnologiesItem([FromQuery] int id, string name)
        {
            _putTechnologiesItem.AddNewEntry(id, name);
            return Ok();
        }

        [Route("putRequirementsItem")]
        public IActionResult PutRequirementsItem([FromQuery] int id, string description)
        {
            _putRequirementsItem.AddNewEntry(id, description);
            return Ok();
        }

        [Route("putTrainingProgramsItem")]
        public IActionResult PutTrainingProgramsItems([FromQuery] int id, string title)
        {
            _putTrainingProgramItem.AddNewEntry(id, title);
            return Ok();
        }

        [Route("getTechnologiesItems")]
        public async Task<IActionResult> GetTecnologiesItems([FromQuery] int? id)
        {
            var response = await _getTecnologiesItem.GetItems(id);
            return Ok(response);
        }

        [Route("getRequirementsItems")]
        public async Task<IActionResult> GetRequirementsItems([FromQuery] int? id)
        {
            var response = await _getRequirementsItem.GetItems(id);
            return Ok(response);
        }

        [Route("getTrainingProgramsItems")]
        public async Task<IActionResult> GetTrainingProgramsItem([FromQuery] int? id)
        {
            var response = await _getTrainingProgramsItem.GetItems(id);
            return Ok(response);
        }
    }
}
