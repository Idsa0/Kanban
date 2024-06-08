using System.Collections.Generic;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BoardModel : NotifiableModel
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private List<string> backlogTasks;
        public List<string> BacklogTasks
        {
            get => backlogTasks;
            set
            {
                backlogTasks = value;
                RaisePropertyChanged("BacklogTasks");
            }
        }

        private List<string> inProgressTasks;
        public List<string> InProgressTasks
        {
            get => inProgressTasks;
            set
            {
                inProgressTasks = value;
                RaisePropertyChanged("InProgressTasks");
            }
        }

        private List<string> doneTasks;
        public List<string> DoneTasks
        {
            get => doneTasks;
            set
            {
                doneTasks = value;
                RaisePropertyChanged("DoneTasks");
            }
        }

        private UserModel user;
        public UserModel User
        {
            get => user;
        }

        public BoardModel(BackendController controller, UserModel user, string name) : base(controller)
        {
            this.name = name;
            this.user = user;
            backlogTasks = new List<string>();
            inProgressTasks = new List<string>();
            doneTasks = new List<string>();
        }
    }
}