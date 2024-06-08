using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend
{
    public abstract class NotifiableModel : Notifiable
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModel(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}