using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    internal class TaskTests
    {
        private GradingService gs;

        string res;
        const string VALIDEMAIL0 = "admin@post.bgu.ac.il";
        const string VALIDEMAIL1 = "rector@post.bgu.ac.il";
        const string INVALIDEMAIL0 = "";
        const string VALIDPASSWORD = "Itsasecret1234";
        const string VALIDBOARDNAME = "board_0";
        const string INVALIDBOARDNAME = "";
        const int MINVALIDCOLUMNORDINAL = 0; // base this on future functional requirements
        const int MAXVALIDCOLUMNORDINAL = 2; // base this on future functional requirements
        const int MINLIMIT = 1;
        const int MAXLIMIT = 10; // base this on future functional requirements
        const string VALIDTASKTITLE = "mytask";
        const string INVALIDTASKTITLE = "";
        const string VALIDTASKDESCRIPTION = "mydesc";
        const string INVALIDTASKDESCRIPTION = "Elon Reeve Musk FRS (/ˈiːlɒn/ EE-lon; born June 28, 1971) is a business magnate and investor. He is the founder, CEO and chief engineer of SpaceX; angel investor, CEO and product architect of Tesla, Inc.; owner and CEO of Twitter; founder of the Boring Company; co-founder of Neuralink and OpenAI; and president of the philanthropic Musk Foundation. With an estimated net worth of around $192 billion as of March 27, 2023, primarily from his ownership stakes in Tesla and SpaceX,[4][5] Musk is the second-wealthiest person in the world, according to both the Bloomberg Billionaires Index and Forbes's real-time billionaires list.[6][7]\r\n\r\nMusk was born in Pretoria, South Africa, and briefly attended at the University of Pretoria before moving to Canada at age 18, acquiring citizenship through his Canadian-born mother. Two years later, he matriculated at Queen's University and transferred to the University of Pennsylvania, where he received bachelor's degrees in economics and physics. He moved to California in 1995 to attend Stanford University. After two days, he dropped out and, with his brother Kimbal, co-founded the online city guide software company Zip2. In 1999, Zip2 was acquired by Compaq for $307 million and Musk co-founded X.com, a direct bank. X.com merged with Confinity in 2000 to form PayPal, which eBay acquired for $1.5 billion in 2002. Musk received an EB-5 investor green card in 1997, which led to his U.S. citizenship in 2002.[8]\r\n\r\nWith $175.8 million, Musk founded SpaceX in 2002, a spaceflight services company. In 2004, he was an early investor in the electric vehicle manufacturer Tesla Motors, Inc. (Tesla, Inc.). He became its chairman and product architect, assuming the position of CEO in 2008. In 2006, he helped create SolarCity, a solar energy company that was later acquired by Tesla and became Tesla Energy. In 2015, he co-founded OpenAI, a nonprofit artificial intelligence research company. The following year, he co-founded Neuralink—a neurotechnology company developing brain–computer interfaces—and the Boring Company, a tunnel construction company. Musk has also proposed a hyperloop high-speed vactrain transportation system. In 2022, his acquisition of Twitter for $44 billion was completed.\r\n\r\nMusk has made controversial statements on politics and technology, particularly on Twitter, and is a polarizing figure. He has been criticized for making unscientific and misleading statements, including spreading COVID-19 misinformation. In 2018, the U.S. Securities and Exchange Commission (SEC) sued Musk for falsely tweeting that he had secured funding for a private takeover of Tesla. Musk stepped down as chairman of Tesla and paid a $20 million fine as part of a settlement agreement with the SEC.";
        DateTime VALIDDATETIME = DateTime.Now;
        DateTime INVALIDDATETIME = new DateTime();  // base this on future functional requirements
        const int VALIDTASKID = 0;
        const int INVALIDTASKID = -1;

        public TaskTests() { }

        internal void UpdateTaskDueDate()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("testing UpdateTaskDueDate:");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);
            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);

            Console.WriteLine("Testing if a task due date can be updated with a valid due date");
            res = gs.UpdateTaskDueDate(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 1");
            else Console.WriteLine("Fail: the function does not work with valid arguments");
        }

        internal void UpdateTaskTitle()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("testing Update Task Title:");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);
            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);

            Console.WriteLine("Testing if a task title can be updated with a new valid title");
            res = gs.UpdateTaskTitle(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDTASKTITLE);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 3");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if a task title can be updated with an empty title");
            res = gs.UpdateTaskTitle(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, INVALIDTASKTITLE);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 3");
            else Console.WriteLine("Fail: the function works with an invalid title");

            Console.WriteLine("Testing if a task title can be updated with a null title");
            res = gs.UpdateTaskTitle(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, null);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 3");
            else Console.WriteLine("Fail: the function works with an invalid title");
        }

        internal void UpdateTaskDescription()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("testing UpdateTaskDescription:");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);
            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);

            Console.WriteLine("Testing if a task description can be updated with a new valid description");
            res = gs.UpdateTaskDescription(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDTASKDESCRIPTION);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 3");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if a task description can be updated with an invalid description");
            res = gs.UpdateTaskDescription(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, INVALIDTASKDESCRIPTION);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 3");
            else Console.WriteLine("Fail: the function works with an invalid description");

            Console.WriteLine("Testing if a task description can be updated with a null description");
            res = gs.UpdateTaskDescription(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, null);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 3");
            else Console.WriteLine("Fail: the function works with a null description");
        }

        internal void LimitColumn()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing LimitColumn");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            Console.WriteLine("Testing if a column can be limited with valid arguments");
            res = gs.LimitColumn(VALIDEMAIL0, VALIDBOARDNAME, 0, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if a column can be limited with valid arguments");
            res = gs.LimitColumn(VALIDEMAIL0, VALIDBOARDNAME, 0, 5);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if a column can be limited with invalid arguments");
            res = gs.LimitColumn(VALIDEMAIL0, VALIDBOARDNAME, 0, -5);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: the function works with an invalid limit");

            Console.WriteLine("Testing if a non existing column can be limited");
            res = gs.LimitColumn(VALIDEMAIL0, VALIDBOARDNAME, -1, 5);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: the function works for a non existing column");
        }

        internal void GetColumnLimit()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing GetColumnLimit");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            Console.WriteLine("Testing if the function works on a valid column");
            res = gs.GetColumnLimit(VALIDEMAIL0, VALIDBOARDNAME, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: the function does not work on a valid column");

            Console.WriteLine("Testing if the function works on an invalid column");
            res = gs.GetColumnLimit(VALIDEMAIL0, VALIDBOARDNAME, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: the function works on an invalid column");
        }

        internal void GetColumnName()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing GetColumnName");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            Console.WriteLine("Testing if the function works on a valid column");
            res = gs.GetColumnName(VALIDEMAIL0, VALIDBOARDNAME, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: the function does not work on a valid column");

            Console.WriteLine("Testing if the function works on an invalid column");
            res = gs.GetColumnName(VALIDEMAIL0, VALIDBOARDNAME, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: the function work on an invalid column");
        }

        internal void AddTask()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing AddTask");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            Console.WriteLine("Testing if a task can be added with valid arguments");
            res = gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 5");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if a task can be added with an invalid email");
            res = gs.AddTask(INVALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 5");
            else Console.WriteLine("Fail: the function works with an invalid email");

            Console.WriteLine("Testing if a task can be added with an invalid board");
            res = gs.AddTask(VALIDEMAIL0, INVALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 5");
            else Console.WriteLine("Fail: the function works with an invalid board name");

            Console.WriteLine("Testing if a task can be added with an invalid task title");
            res = gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, INVALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 5");
            else Console.WriteLine("Fail: the function works with an invalid task title");

            Console.WriteLine("Testing if a task can be added with an invalid task description");
            res = gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, INVALIDTASKDESCRIPTION, VALIDDATETIME);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 5 out of 5");
            else Console.WriteLine("Fail: the function works with an invalid task description");
        }

        internal void GetColumn()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing GetColumn");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);

            Console.WriteLine("Testing if a column can be retrived with valid arguments");
            res = gs.GetColumn(VALIDEMAIL0, VALIDBOARDNAME, MINVALIDCOLUMNORDINAL);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: the function does not work with valid credentials");

            Console.WriteLine("Testing if a non existing column can be retrived");
            res = gs.GetColumn(INVALIDEMAIL0, VALIDBOARDNAME, MINVALIDCOLUMNORDINAL);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: the function works with an invalid email");
        }


        internal void InProgressTasks()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing InProgressTasks");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);

            Console.WriteLine("Testing if a all tasks can be retrieved for an existing user");
            res = gs.InProgressTasks(VALIDEMAIL0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 2");
            else Console.WriteLine("Fail: the function does not work with valid credentials");

            Console.WriteLine("Testing if a all tasks can be retrieved for a non existing user");
            res = gs.InProgressTasks(INVALIDEMAIL0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 2");
            else Console.WriteLine("Fail: the function works with an invalid email");
        }

        internal void AdvanceTask()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing AdvanceTask");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);
            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);

            Console.WriteLine("Testing if an existing task can be advanced from backlog to in progress");
            res = gs.AdvanceTask(VALIDEMAIL0, VALIDBOARDNAME, 0, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if an existing task can be advanced from in progress to done");
            res = gs.AdvanceTask(VALIDEMAIL0, VALIDBOARDNAME, 1, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: the function does not work with valid arguments");

            Console.WriteLine("Testing if an existing task can be advanced from done");
            res = gs.AdvanceTask(VALIDEMAIL0, VALIDBOARDNAME, 2, 0);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: the function works with invalid arguments");

            Console.WriteLine("Testing if a non existing task can be advanced");
            res = gs.AdvanceTask(VALIDEMAIL0, VALIDBOARDNAME, 2, -1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: the function works with invalid arguments");
        }

        internal void AssignTask()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            Console.WriteLine("Testing AdvanceTask");
            gs = new GradingService();
            gs.Register(VALIDEMAIL0, VALIDPASSWORD);
            gs.Login(VALIDEMAIL0, VALIDPASSWORD);
            gs.CreateBoard(VALIDEMAIL0, VALIDBOARDNAME);
            gs.AddTask(VALIDEMAIL0, VALIDBOARDNAME, VALIDTASKTITLE, VALIDTASKDESCRIPTION, VALIDDATETIME);
            gs.JoinBoard(VALIDEMAIL1, 0);

            Console.WriteLine("Testing if a task can be assigned to a user in the task's board");
            res = gs.AssignTask(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDEMAIL1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage == null)
                Console.WriteLine("Passed part 1 out of 4");
            else Console.WriteLine("Fail: a task was not assigned to a user in a board");

            // TEST requirement 23
            Console.WriteLine("Testing if a user can reassign a task when not assigned to it");
            res = gs.AssignTask(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDEMAIL1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 2 out of 4");
            else Console.WriteLine("Fail: a task can be reassigned by a user not assigned to it");

            Console.WriteLine("Testing if a user can assign a non existing task");
            res = gs.AssignTask(VALIDEMAIL0, VALIDBOARDNAME, 0, -1, VALIDEMAIL1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 3 out of 4");
            else Console.WriteLine("Fail: a non existing task can be assigned to a user");

            Console.WriteLine("Testing if a user can assign a task to a non existing user");
            res = gs.AssignTask(VALIDEMAIL0, VALIDBOARDNAME, 0, 0, VALIDEMAIL1);
            if (JsonSerializer.Deserialize<Response>(res).ErrorMessage != null)
                Console.WriteLine("Passed part 4 out of 4");
            else Console.WriteLine("Fail: a task can be assigned to a non existing user");
        }
    }
}
