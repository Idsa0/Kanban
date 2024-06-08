using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class UserVM : Notifiable
    {
        private BackendController controller;

        public UserVM(UserModel user)
        {
            controller = user.Controller;
        }

        public UserVM(BackendController controller)
        {
            this.controller = controller;
        }

        public UserVM()
        {
            controller = new BackendController();
        }

        private string errorMessage = "";

        public  string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        internal BoardModel GetBoard(UserModel user, string boardName)
        {
            return controller.GetBoard(user, boardName);
        }
    }
}