namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class TaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public TaskModel(TaskToSend task)
        {
            this.Title = task.Title;
            this.Description = task.Description;
        }
    }
}
