using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    public class MainTests
    {
        UserTests ut;
        BoardTests bt;
        TaskTests tt;


        public MainTests() { ut = new UserTests(); bt = new BoardTests(); tt = new TaskTests(); }

        public void Run(string input)
        {
            switch (input)
            {
                case "Register":
                    ut.Register();
                    break;

                case "Login":
                    ut.Login();
                    break;

                case "Logout":
                    ut.Logout();
                    break;

                case "LimitColumn":
                    tt.LimitColumn();
                    break;

                case "GetColumnLimit":
                    tt.GetColumnLimit();
                    break;

                case "GetColumnName":
                    tt.GetColumnName();
                    break;

                case "AddTask":
                    tt.AddTask();
                    break;

                case "UpdateTaskDueDate":
                    tt.UpdateTaskDueDate();
                    break;

                case "UpdateTaskTitle":
                    tt.UpdateTaskTitle();
                    break;

                case "UpdateTaskDescription":
                    tt.UpdateTaskDescription();
                    break;

                case "AdvanceTask":
                    tt.AdvanceTask();
                    break;

                case "GetColumn":
                    tt.GetColumn();
                    break;

                case "CreateBoard":
                    bt.CreateBoard();
                    break;

                case "DeleteBoard":
                    bt.DeleteBoard();
                    break;

                case "InProgressTasks":
                    tt.InProgressTasks();
                    break;

                case "GetUserBoards":
                    bt.GetUserBoards();
                    break;

                case "JoinBoard":
                    bt.JoinBoard();
                    break;

                case "LeaveBoard":
                    bt.LeaveBoard();
                    break;

                case "GetBoardName":
                    bt.GetBoardName();
                    break;

                case "TransferOwnership":
                    bt.TransferOwnership();
                    break;

                case "AssignTask":
                    tt.AssignTask();
                    break;

                default:
                    {
                  
                        ut.Register();
                        ut.Login();
                        ut.Logout();
                        bt.CreateBoard();
                        bt.DeleteBoard();
                        bt.GetUserBoards();
                        bt.JoinBoard();
                        bt.LeaveBoard();
                        bt.GetBoardName();
                        bt.TransferOwnership();
                        tt.GetColumn();
                        tt.GetColumnLimit();
                        tt.LimitColumn();
                        tt.GetColumnName();
                        tt.AddTask();
                        tt.UpdateTaskDueDate();
                        tt.UpdateTaskTitle();
                        tt.UpdateTaskDescription();
                        tt.AdvanceTask();
                        tt.InProgressTasks();
                        tt.AssignTask();
                        break;
                    }
            }
        }
    }
}
