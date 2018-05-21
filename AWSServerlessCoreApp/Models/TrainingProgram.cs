
using System.Collections.Generic;

namespace AWSServerlessCoreApp.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Technology> Technologyies { get; set; }
        public List<Requirement> Requirements { get; set; }
    }
}
