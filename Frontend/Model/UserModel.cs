using System.Collections.Generic;
using System.Windows.Documents;

namespace IntroSE.Kanban.Frontend.Model
{
    public class UserModel : NotifiableModel
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private List<string> boards;
        public List<string> Boards
        {
            get => boards;
            set
            {
                boards = value;
                RaisePropertyChanged("Boards");
            }
        }

        internal BackendController controller;

        public UserModel(BackendController controller, string email, List<string> boards) : base(controller)
        {
            this.controller = controller;
            this.email = email;
            this.boards = boards;
        }
    }
}