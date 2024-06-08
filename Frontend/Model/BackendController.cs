using System;
using System.Collections.Generic;
using System.Text.Json;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Linq;

namespace IntroSE.Kanban.Frontend.Model
{
    public class BackendController
    {
        private GradingService service { get; set; }

        public BackendController(GradingService service)
        {
            this.service = service;
        }

        public BackendController()
        {
            service = new GradingService();
            Response res = JsonSerializer.Deserialize<Response>(service.LoadData());
            if (res.ErrorMessage != null)
                throw new Exception(res.ErrorMessage);
        }

        public Tuple<UserModel?, string> Login(string email, string password)
        {
            Response res = JsonSerializer.Deserialize<Response>(service.Login(email, password));
            if (res.ErrorMessage != null)
                return Tuple.Create<UserModel?, string>(null, res.ErrorMessage);
            return Tuple.Create<UserModel?, string>(new UserModel(this, email, GetUserBoards(email)), res.ErrorMessage);
        }

        public Tuple<UserModel?, string> Register(string email, string password)
        {
            Response res = JsonSerializer.Deserialize<Response>(service.Register(email, password));
            if (res.ErrorMessage != null)
                return Tuple.Create<UserModel?, string>(null, res.ErrorMessage);
            return Tuple.Create<UserModel?, string>(new UserModel(this, email, new List<string>()), res.ErrorMessage);
        }

        public List<string> GetUserBoards(string email)
        {
            Response res = JsonSerializer.Deserialize<Response>(service.GetUserBoards(email));
            List<int> l = JsonSerializer.Deserialize<List<int>>((JsonElement)res.ReturnValue);
            List<string> boardNames = new List<string>();
            foreach (int i in l)
                boardNames.Add(JsonSerializer.Deserialize<Response>(service.GetBoardName(i)).ReturnValue.ToString());
            return boardNames;
        }

        public BoardModel GetBoard(UserModel user, string boardName)
        {
            Response res0 = JsonSerializer.Deserialize<Response>(service.GetColumn(user.Email, boardName, 0));
            Response res1 = JsonSerializer.Deserialize<Response>(service.GetColumn(user.Email, boardName, 1));
            Response res2 = JsonSerializer.Deserialize<Response>(service.GetColumn(user.Email, boardName, 2));

            BoardModel board = new BoardModel(this, user, boardName);
            List<TaskToSend> t0 = JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)res0.ReturnValue);
            List<string> n0 = new List<string>(), n1 = new List<string>(), n2 = new List<string>();
            n0.AddRange(t0.Select(t => t.Title));
            List<TaskToSend> t1 = JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)res1.ReturnValue);
            n1.AddRange(t1.Select(t => t.Title));
            List<TaskToSend> t2 = JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)res2.ReturnValue);
            n2.AddRange(t2.Select(t => t.Title));
            board.BacklogTasks = n0;
            board.InProgressTasks = n1;
            board.DoneTasks = n2;
            return board;
        }
    }
}