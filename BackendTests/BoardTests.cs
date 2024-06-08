using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    internal class BoardTests
    {
        GradingService gs;
        string res;
        const string VALIDEMAIL0 = "admin@post.bgu.ac.il";
        const string VALIDEMAIL1 = "rector@post.bgu.ac.il";
        const string INVALIDEMAIL0 = "";
        const string VALIDPASSWORD = "Itsasecret1234";
        const string VALIDBOARDNAME0 = "board_0";
        const string VALIDBOARDNAME1 = "board_1";
        const string INVALIDBOARDNAME = "";

        public BoardTests() { }

        internal void CreateBoard()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing CreateBoard");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);

            Console.WriteLine("Testing if a board can be created with valid arguments");
            res = gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 5");
            else Console.WriteLine("Fail: a board was not created");

            Console.WriteLine("Testing if a board with the same name can be created again");
            res = gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            Response r = JsonSerializer.Deserialize<Response>(res);
            Console.WriteLine(JsonSerializer.Deserialize<Response>(res).ErrorMessage);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 5");
            else Console.WriteLine("Fail: a board was created with the same name as an existing board");

            Console.WriteLine("Testing if a board can be created with an invalid name");
            res = gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 5");
            else Console.WriteLine("Fail: a board was created with an invalid name");

            gs.Logout(VALIDEMAIL0);

            Console.WriteLine("Testing if a board can be created with a logged out user");
            res = gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 5");
            else Console.WriteLine("Fail: a board was created");

            Console.WriteLine("Testing if a board can be created for a non existing user");
            res = gs.CreateBoard(INVALIDEMAIL0, VALIDBOARDNAME1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 5 out of 5");
            else Console.WriteLine("Fail: a board was created");
        }

        internal void DeleteBoard()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing DeleteBoard");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);

            Console.WriteLine("Testing if an existing board can be deleted");
            res = gs.DeleteBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: an existing board was not deleted");

            Console.WriteLine("Testing if a non existing board can be deleted");
            res = gs.DeleteBoard(VALIDEMAIL0, INVALIDBOARDNAME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: a non existing board was deleted");

            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            gs.Logout(VALIDEMAIL0);

            Console.WriteLine("Testing if an existing board can be deleted for a signed out user");
            res = gs.DeleteBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: a board was deleted for a signed out user");

            Console.WriteLine("Testing if a board can be deleted for a non existing user");
            res = gs.DeleteBoard(INVALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: a board was deleted non existing user");
        }

        internal void GetUserBoards()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing GetUserBoards");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);

            Console.WriteLine("Testing if a list can be retrieved with valid arguments");
            res = gs.GetUserBoards(VALIDEMAIL0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: could not get a list");

            Console.WriteLine("Testing if a list can be retrieved for a non existing user");
            res = gs.GetUserBoards(INVALIDEMAIL0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: a list of boards can be created for a non existing user");
        }

        internal void JoinBoard()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing JoinBoard");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);

            Console.WriteLine("Testing if a user can join an existing board");
            res = gs.JoinBoard(VALIDEMAIL1, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 5");
            else Console.WriteLine("Fail: a user could not join an existing board");

            Console.WriteLine("Testing if a non existing user can join an existing board");
            res = gs.JoinBoard(INVALIDEMAIL0, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 5");
            else Console.WriteLine("Fail: a non existing user could join an existing board");

            Console.WriteLine("Testing if a user can join a non existing board");
            res = gs.JoinBoard(VALIDEMAIL1, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 5");
            else Console.WriteLine("Fail: a user could join a non existing board");

            Console.WriteLine("Testing if a user that already joined a board can join the same board again");
            res = gs.JoinBoard(VALIDEMAIL1, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 5");
            else Console.WriteLine("Fail: a user could join a board that has already been joined");

            // Tests requirement 6
            gs.LeaveBoard(VALIDEMAIL1, 0);
            gs.CreateBoard(VALIDEMAIL1, VALIDBOARDNAME0);
            Console.WriteLine("Testing if a user can join a board with the same name as another joined board");
            res = gs.JoinBoard(VALIDEMAIL1, 1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 5 out of 5");
            else Console.WriteLine("Fail: a user could join a board with the same name as another joined board");
        }

        internal void LeaveBoard()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing LeaveBoard");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            gs.JoinBoard(VALIDEMAIL1, 0);

            Console.WriteLine("Testing if a user can leave a board");
            res = gs.LeaveBoard(VALIDEMAIL1, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: a user could not leave a board");

            Console.WriteLine("Testing if a user that does not belong to a board can leave it");
            res = gs.LeaveBoard(VALIDEMAIL1, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: a user could leave a board that does not belong to it");

            Console.WriteLine("Testing if an owner of a board can leave it");
            res = gs.LeaveBoard(VALIDEMAIL0, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: an owner could leave a board");

            Console.WriteLine("Testing if a user can leave a non existing board");
            res = gs.LeaveBoard(VALIDEMAIL1, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: a user could leave a non existing board");
        }

        internal void GetBoardName()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing GetBoardName");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);

            Console.WriteLine("Testing if it is possible to get an existing board's name");
            res = gs.GetBoardName(0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: could not get a board's name");

            Console.WriteLine("Testing if it is possible to get a non existing board's name");
            res = gs.GetBoardName(-1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: got a name from a non existing board");
        }

        internal void TransferOwnership()
        {
            // TODO
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing TransferOwnership");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME0);
            gs.JoinBoard(VALIDEMAIL1, 0);

            Console.WriteLine("Testing if transferring ownership is possible with legal arguments");
            res = gs.TransferOwnership(VALIDEMAIL0, VALIDEMAIL1, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: could not transfter ownership with legal arguments");

            Console.WriteLine("Testing if a user can transfter ownership for a board not owned by it");
            res = gs.TransferOwnership(VALIDEMAIL0, VALIDEMAIL1, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: board transferred by a non owner");

            gs.LeaveBoard(VALIDEMAIL0, 0);

            Console.WriteLine("Testing if an owner can transfer ownership to a user who has not joined the board");
            res = gs.TransferOwnership(VALIDEMAIL1, VALIDEMAIL1, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: board transferred to a user who has not join it");

            Console.WriteLine("Testing if a user can transfter ownership to a non existing user");
            res = gs.TransferOwnership(VALIDEMAIL0, INVALIDEMAIL0, VALIDBOARDNAME0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: board transferred to a non existing user");
        }
    }
}
