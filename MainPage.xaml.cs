namespace projeto_pi
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

        }

        void OnStartQuiz(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.QuizPage());
        }

    }

}
