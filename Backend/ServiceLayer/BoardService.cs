using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        //private BoardFacade bf;
        private TaskFacade tf;
        private Response res;

        public BoardService() { }

        //internal BoardFacade Bf { set { bf = value; } }
        internal TaskFacade Tf { set { tf = value; } }

        /// <summary>
        /// This method creates a board for the given user.   
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="ArgumentNullException">Thrown if the name is null</exception>
        /// <exception cref="ArgumentException">Thrown if the name is empty or if the email is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is not logged in</exception>
        public string CreateBoard(string email, string name)
        {
            try
            {
                tf.CreateBoard(email, name);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="ArgumentException">Thrown if the email is invalid or if the board does not exist</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is not logged in</exception>
        public string DeleteBoard(string email, string name)
        {
            try
            {
                tf.DeleteBoard(email, name);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }


        /// <summary>		 
        /// This method returns a list of IDs of all user's boards.		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>	
        /// <exception cref="ArgumentException">Thrown if the email is invalid</exception>
        public string GetUserBoards(string email)
        {
            try
            {
                res = new Response(tf.GetUserJoinedBoards(email));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>		 
        /// This method adds a user as member to an existing board.		 
        /// </summary>		 
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        /// <exception cref="ArgumentException">Thrown if the email is invalid or if the boardID is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is already in a board with the same name</exception>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                tf.JoinBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>		 
        /// This method removes a user from the members list of a board.		 
        /// </summary>		 
        /// <param name="email">The email of the user. Must be logged in</param>		 
        /// <param name="boardID">The board's ID</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        /// <exception cref="ArgumentException">Thrown if the email is invalid or if the boardID is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the user is not part of the board or is the board owner</exception>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                tf.LeaveBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>		 
        /// This method returns a board's name		 
        /// </summary>		 
        /// <param name="boardId">The board's ID</param>		 
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="ArgumentException">Thrown if the boardID is invalid</exception>
        public string GetBoardName(int boardId)
        {
            try
            {
                res = new Response((object)tf.GetBoardName(boardId), 0);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method transfers the board ownership between two users		 	
        /// </summary>		 
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>		 
        /// <param name="newOwnerEmail">Email of the new owner</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        /// <exception cref="ArgumentException">Thrown if one of the emails is invalid or if the board name is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the new owner has not joined the board</exception>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                tf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }
    }
}
