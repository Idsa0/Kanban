using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class UserTests
    {
        GradingService us;
        public UserTests() { }
        internal void Register()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            us = new GradingService();
            Console.WriteLine("testing Register:");
            string password, Email, res;

            Console.WriteLine("Testing a valid register");
            Console.WriteLine("test 1: password: Aa1234 , email: tomer@gmail.com");
            password = "Aa1234"; Email = "tomer@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should work.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a email that was all ready registered");
            Console.WriteLine("test 2: password: Aa1234 , email: tomer@gmail.com");
            password = "Aa1234"; Email = "tomer@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because we already have that mail registerd.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a null email");
            Console.WriteLine("test 3: password: Aa1234 , email: null");
            password = "Aa1234"; Email = null;
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because email is missing.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password lesser than 6");
            Console.WriteLine("test 4: password: Aa123 , email: tomer1@gmail.com");
            password = "Aa123"; Email = "tomer1@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password too short.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password higher than than 21");
            Console.WriteLine("test 5: password: Aa12345678901234567890 , email: tomer2@gmail.com");
            password = "Aa12345678901234567890"; Email = "tomer2@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password too long.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password dosent contain capital letter");
            Console.WriteLine("test 6: password: a12345 , email: tomer3@gmail.com");
            password = "a1234"; Email = "tomer3@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password dosent have upper letter.");
            Console.WriteLine("your response is:" + res);
            Console.WriteLine("Testing a password dosent contain lower letter");
            Console.WriteLine("test 7: password: A12345 , email: tomer4@gmail.com");
            password = "a1234"; Email = "tomer4@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password dosent have lower letter.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password dosent contain numbers");
            Console.WriteLine("test 8: password: Aabcde , email: tomer5@gmail.com");
            password = "Aabcde"; Email = "tomer5@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password dosent have a number.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password that is null");
            Console.WriteLine("test 9: password: null , email: tomer6@gmail.com");
            password = null; Email = "tomer6@gmail.com";
            res = us.Register(Email, password);
            Console.WriteLine("the test should fail because password is missing.");
            Console.WriteLine(res);
        }

        internal void Login()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            us = new GradingService();
            Console.WriteLine("testing Login:");

            Console.WriteLine("we register a new user to test on: password: Aa1234 email: tomerc@gmail.com");
            string password, Email, res;
            password = "Aa1234"; Email = "tomerc@gmail.com";
            us.Register(Email, password);

            Console.WriteLine("Testing a good login");
            Console.WriteLine("test 1: password: Aa1234 , email: tomerc@gmail.com");
            password = "Aa1234"; Email = "tomerc@gmail.com";
            res = us.Login(Email, password);
            Console.WriteLine("the test should work.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password dosent match to email");
            Console.WriteLine("test 2: password: Aa12345 , email: tomerc@gmail.com");
            password = "Aa12345"; Email = "tomerc@gmail.com";
            res = us.Login(Email, password);
            Console.WriteLine("the test should fail because the password is wrong.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a password dosent match to email and is null");
            Console.WriteLine("test 3: password: null , email: tomerc@gmail.com");
            password = null; Email = "tomerc@gmail.com";
            res = us.Login(Email, password);
            Console.WriteLine("the test should fail because the password is missing.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("Testing a email that dosent match to password");
            Console.WriteLine("test 4: password: Aa1234 , email: tomer0@gmail.com");
            password = "Aa1234"; Email = "tomer0@gmail.com";
            res = us.Login(Email, password);
            Console.WriteLine("the test should fail because the user isnt registered.");
            Console.WriteLine("your response is:" + res);
        }

        internal void Logout()
        {
            Console.WriteLine("\n\n ============================================\n\n");
            us = new GradingService();
            Console.WriteLine("testing Logout:");

            Console.WriteLine("we Register a new user to test on: password: Aa1234 email: tomerco@gmail.com");
            string password, Email, res;
            password = "Aa1234"; Email = "tomerco@gmail.com";
            us.Register(Email, password);
            Console.WriteLine("we try to Logout not as a logged in user");
            Email = "idan@gmail.com";
            res = us.Logout(Email);
            Console.WriteLine("the test should fail.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("we try to Logout as a logged user");
            Email = "tomerco@gmail.com";
            res = us.Logout(Email);
            Console.WriteLine("the test should work.");
            Console.WriteLine("your response is:" + res);

            Console.WriteLine("we try to Logout again even if we arent logged in");
            Email = "tomerco@gmail.com";
            res = us.Logout(Email);
            Console.WriteLine("the test should fail.");
            Console.WriteLine("your response is:" + res);
        }
    }
}
