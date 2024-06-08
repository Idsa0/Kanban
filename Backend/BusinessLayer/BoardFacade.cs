using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {
        private BoardDTO _boardDTO;
        private UserFacade uf;
        private int nextId = 0;
        private InputValidator iv = new InputValidator();
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<Tuple<string, string>, BoardBusiness> boards;

        internal BoardFacade(UserFacade uf)
        {
            this.uf = uf;
            this.boards = new Dictionary<Tuple<string, string>, BoardBusiness>();
            _boardDTO = new BoardDTO();

        }

        private void DTOUpdate(BoardBusiness board)
        {
            _boardDTO.ChangeValues(board.Id, board.Name, board.OwnerEmail, board.Users);
        }

        internal BoardBusiness CreateBoard(string email, string name)
        {
            log.Info("CreateBoard called");
            if (!uf.UserExists(email))
            {
                log.Error("Cannot create board - user does not exist");
                throw new ArgumentException("User does not exist");
            }
            if (!uf.Users[email].IsLoggedIn)
            {
                log.Error("Cannot create board - user is not logged in");
                throw new InvalidOperationException("User is not logged in");
            }

            Tuple<string, string> key = Tuple.Create(email, name);

            if (boards.TryGetValue(key, out BoardBusiness board))
            {
                log.Error("Cannot create board - board already exists");
                throw new InvalidOperationException("Board already exists");
            }
            BoardBusiness b = new BoardBusiness(nextId++, name, email);
            boards.Add(key, b);
            DTOUpdate(b);
            _boardDTO.InsertMe();
            log.Info("Successfully added board " + name + "for user " + email);
            return b;
        }

        internal bool DeleteBoard(string email, string name)
        {
            log.Info("DeleteBoard called");
            if (!uf.UserExists(email))
            {
                log.Error("Cannot create board - user does not exist");
                throw new ArgumentException("User does not exist");
            }
            if (!uf.Users[email].IsLoggedIn)
            {
                log.Error("Cannot create board - user is not logged in");
                throw new InvalidOperationException("User is not logged in");
            }
            Tuple<string, string> key = Tuple.Create(email, name);

            if (boards.TryGetValue(key, out BoardBusiness b))
            {
                if (b.OwnerEmail != email)
                {
                    log.Error("User is not the board owner");
                    throw new InvalidOperationException("User is not the board owner");
                }
                boards.Remove(key);
                DTOUpdate(b);
                _boardDTO.DeleteMe();
                log.Info("Successfully removed board " + name + "for user " + email);
                return true;
            }
            log.Error("Cannot remove board - board does not exist");
            throw new ArgumentException("Board does not exist");
        }

        internal List<int> GetUserOwnedBoards(string email)
        {
            log.Info("GetUserBoards called");

            if (!uf.UserExists(email))
            {
                log.Error("User doesn't exist");
                throw new ArgumentException("User doesn't exist");
            }

            List<int> IDs = new List<int>();
            foreach (BoardBusiness board in Boards.Values)
                if (board.OwnerEmail.Equals(email))
                    IDs.Add(board.Id);
            return IDs;
        }

        internal List<int> GetUserJoinedBoards(string email)
        {
            log.Info("GetUserBoards called");

            if (!uf.UserExists(email))
            {
                log.Error("User doesn't exist");
                throw new ArgumentException("User doesn't exist");
            }

            List<int> IDs = new List<int>();
            foreach (BoardBusiness board in Boards.Values)
                if (board.HasUserJoined(email))
                    IDs.Add(board.Id);
            return IDs;
        }

        internal bool JoinBoard(string email, int boardID)
        {
            log.Info("JoinBoard called");
            if (!uf.UserExists(email))
            {
                log.Error("User does not exist");
                throw new ArgumentException("User does not exist");
            }
            BoardBusiness b = GetBoardById(boardID);
            if (b == null)
            {
                log.Error("Board does not exist");
                throw new ArgumentException("Board does not exist");
            }
            if (b.HasUserJoined(email))
            {
                log.Error("User has already joined the board");
                throw new InvalidOperationException("User has already joined the board");
            }
            if (GetUserJoinedBoards(email).Contains(boardID))
            {
                log.Error("User has already joined a board with the same name");
                throw new InvalidOperationException("User has already joined a board with the same name");
            }
            b.AddUser(email);
            DTOUpdate(b);
            _boardDTO.ChangeValues(b.Id, b.Name, b.OwnerEmail, b.Users);
            _boardDTO.Join(email);
            return true;
        }

        internal bool LeaveBoard(string email, int boardID)
        {
            log.Info("LeaveBoard called");
            if (!uf.UserExists(email))
            {
                log.Error("User does not exist");
                throw new ArgumentException("User does not exist");
            }
            BoardBusiness b = GetBoardById(boardID);
            if (b == null)
            {
                log.Error("Board does not exist");
                throw new ArgumentException("Board does not exist");
            }
            if (b.OwnerEmail.Equals(email))
            {
                log.Error("Board owner cannot leave the board");
                throw new InvalidOperationException("Board owner cannot leave the board");
            }
            if (!b.HasUserJoined(email))
            {
                log.Error("User has not joined the board");
                throw new InvalidOperationException("User has not joined the board");
            }
            if (!b.RemoveUser(email))
            {
                log.Error("Board remove failed");
                throw new InvalidOperationException("Board remove failed");
            }
            DTOUpdate(b);
            _boardDTO.ChangeValues(b.Id, b.Name, b.OwnerEmail, b.Users);
            _boardDTO.Leave(email);
            return true;
        }


        internal string GetBoardName(int boardId)
        {
            log.Info("GetBoardName called");
            BoardBusiness b = GetBoardById(boardId);
            if (b == null)
            {
                log.Error("Board does not exist");
                throw new ArgumentException("Board does not exist");
            }
            return b.Name;
        }

        internal bool TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            log.Info("TransferOwnership called");
            BoardBusiness b = GetBoard(currentOwnerEmail, boardName);
            if (!b.HasUserJoined(newOwnerEmail))
            {
                log.Error("Cannot transfer ownership to a user that has not joined the board");
                throw new InvalidOperationException("Cannot transfer ownership to a user that has not joined the board");
            }
            b.OwnerEmail = newOwnerEmail;
            boards.Remove(Tuple.Create(currentOwnerEmail, boardName));
            boards.Add(Tuple.Create(newOwnerEmail, boardName), b);

            DTOUpdate(b);
            _boardDTO.ChangeValues(b.Id, b.Name, newOwnerEmail, b.Users);
            return true;
        }

        internal BoardBusiness GetBoard(string email, string name)
        {
            log.Info("GetBoard called");
            if (!uf.UserExists(email))
            {
                log.Error("Cannot get board - user does not exist");
                throw new ArgumentException("User does not exist");
            }
            if (boards.TryGetValue(Tuple.Create(email, name), out BoardBusiness b))
            {
                log.Info("Successfully got board " + name + "for user " + email);
                return b;
            }
            log.Error("Cannot get board - board does not exist");
            throw new ArgumentException("Board does not exist");
        }

        internal BoardBusiness GetBoardForJoinedUser(string email, string name)
        {
            log.Info("GetBoardForJoinedUser called");
            if (!uf.UserExists(email))
            {
                log.Error("Cannot get board - user does not exist");
                throw new ArgumentException("User does not exist");
            }
            foreach (BoardBusiness board in boards.Values)
            {
                if (board.Name == name && board.HasUserJoined(email))
                    return board;
            }
            return null;
        }

        internal bool BoardExists(string email, string name)
        {
            return boards.ContainsKey(Tuple.Create(email, name));
        }

        internal UserFacade UserFacade { get { return uf; } }

        internal Dictionary<Tuple<string, string>, BoardBusiness> Boards
        {
            get { return boards; }
        }

        internal int GetIdByEmailAndName(string email, string name)
        {
            foreach (BoardBusiness board in boards.Values)
                if (board.Name.Equals(name) && board.HasUserJoined(email))
                    return board.Id;
            throw new ArgumentException($"could not find board with email: {email}, name: {name}");
        }

        internal BoardBusiness GetBoardById(int boardId)
        {
            foreach (var board in boards.Values)
                if (board.Id == boardId)
                    return board;
            return null;
        }

        internal int GetIdByEmailAndNameForJoinedUser(string email, string boardName)
        {
            BoardBusiness b = GetBoardForJoinedUser(email, boardName);
            if (b == null)
            {
                log.Error("Board does not exist");
                throw new ArgumentNullException("Board does not exist");
            }
            return b.Id;
        }

        internal void LoadData()
        {
            Tuple<LinkedList<BoardDTO>, Dictionary<int, LinkedList<string>>> b = _boardDTO.LoadBoards();
            foreach (BoardDTO board in b.Item1)
            {
                Tuple<string, string> t = Tuple.Create(board.OwnerEmail, board.Name);
                BoardBusiness bb = DTOtoBusiness(board);
                boards.Add(t, bb);
                bb.AddUsers(b.Item2[board.Id]);
                if (bb.Id >= nextId)
                    nextId = bb.Id + 1;
            }
        }

        private BoardBusiness DTOtoBusiness(BoardDTO boardDTO)
        {
            BoardBusiness b = new BoardBusiness(boardDTO.Id, boardDTO.Name, boardDTO.OwnerEmail);
            b.AddUsers(boardDTO.Users);
            return b;
        }

        internal void DeleteData()
        {
            Tuple<LinkedList<BoardDTO>, Dictionary<int, LinkedList<string>>> b = _boardDTO.LoadBoards(); // DELETELINKINGT
            foreach (BoardDTO board in b.Item1)
                board.DeleteMe();
            nextId = 0;
            boards = new Dictionary<Tuple<string, string>, BoardBusiness>();
        }
    }
}
