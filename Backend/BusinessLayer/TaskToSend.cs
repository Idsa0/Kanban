using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class TaskToSend
    {

        public int ID { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public TaskToSend(int id, DateTime creationTime, DateTime dueDate, string title, string description)
        {
            this.ID = id;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
        }
    }
}
