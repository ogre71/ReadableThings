using System;

namespace Ogresoft.Tasks
{
    public class Task
    {
        public Task(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }
}
