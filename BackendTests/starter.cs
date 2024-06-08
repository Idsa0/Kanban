using BackendTests;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;

class starter
{

    // Main Method
    static public void Main(String[] args)
    {
        //string mail1 = "GOLD.AIDEN2002@GMAIL.COM";
        //string pass1 = "Aa123456";
        //string mail2 = "G1.AIDEN2002@GMAIL.COM";
        //string pass2 = "Bb123456";
        //string bname1 = "table1";
        //string bname2 = "table2";
        //string bname3 = "table3";
        //string bname4 = "table4";
        string email = "mail@mail.com";
        string password = "Password1";
        string board1 = "board1";
        string board2 = "board2";
        string task1 = "task to the future";
        string task2 = "progressive tasks rock";
        string task3 = "done and done";

        MainTests mt = new MainTests();
        GradingService gs = new GradingService();

        gs.DeleteData();
        gs.Register(email, password);
        gs.CreateBoard(email, board1);
        gs.CreateBoard(email, board2);
        gs.AddTask(email, board1, task1, "fuck my life", DateTime.Now);
        gs.AddTask(email, board1, task2, "fuck my life", DateTime.Now);
        gs.AddTask(email, board1, task3, "fuck my life", DateTime.Now);

        gs.AssignTask(email, board1, 0, 1, email);
        gs.AssignTask(email, board1, 0, 2, email);

        gs.AdvanceTask(email, board1, 0, 1);
        gs.AdvanceTask(email, board1, 0, 2);
        gs.AdvanceTask(email, board1, 1, 2);

        //gs.DeleteData();
        //gs.Register(mail1, pass1);
        //gs.Register(mail2, pass2);
        //gs.CreateBoard(mail1, bname1);
        //gs.CreateBoard(mail1, bname2);
        //gs.CreateBoard(mail2, bname3);
        //gs.CreateBoard(mail2, bname4);

        //gs.JoinBoard(mail2, 0);
        //gs.LeaveBoard(mail2, 0);

        //gs.JoinBoard(mail2, 0);
        //gs.JoinBoard(mail2, 1);
        //// gs.JoinBoard(mail1, 2);
        //// gs.JoinBoard(mail1, 3);

        //gs.AddTask(mail1, bname1, "t1", "fuck my life", DateTime.Now);
        //gs.AddTask(mail2, bname1, "t2", "fuck my life2", DateTime.Now);

        //gs.AddTask(mail1, bname2, "t1", "fuck my life", DateTime.Now);
        //gs.AddTask(mail2, bname2, "t2", "fuck my life2", DateTime.Now);

        //gs.AssignTask(mail1, bname1, 0, 0, mail2);
        //gs.AssignTask(mail1, bname2, 0, 2, mail2);

        //gs.AdvanceTask(mail2, bname1, 0, 0);
        //gs.AdvanceTask(mail2, bname1, 1, 0);
        //gs.AdvanceTask(mail2, bname1, 2, 0);

        // gs.LeaveBoard(mail2, 0);
        //
        // for (int i = 0; i < 4; i++)
        //     Console.WriteLine(gs.GetBoardName(i));
        // gs.DeleteData();
        //mt.Run("all");
    }
}
