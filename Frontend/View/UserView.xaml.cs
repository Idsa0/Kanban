using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace IntroSE.Kanban.Frontend.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        UserVM vm;
        UserModel model;

        public UserView(UserModel user)
        {
            InitializeComponent();
            vm = new UserVM(user);
            DataContext = vm;
            Title = user.Email;
            BoardListView.ItemsSource = user.Boards;
            model = user;
        }

        public UserView(BoardModel board)
        {
            InitializeComponent();
            vm = new UserVM(board.Controller);
            DataContext = vm;
            Title = board.User.Email;
            BoardListView.ItemsSource = board.User.Boards;
            model = board.User;
        }

        private void BoardListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BoardListView.SelectedItem != null)
            {
                BoardModel board = vm.GetBoard(model, ""+BoardListView.SelectedItem.ToString());
                // TODO should we check for possible null board?
                BoardView boardView = new BoardView(board);
                boardView.Show();
                Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(model.Controller);
            loginWindow.Show();
            Close();
        }
    }
}
