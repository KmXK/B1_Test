using Task_02.Persistence;

namespace Task_02;

public partial class MainWindow
{
    public MainWindow(AppDbContext context)
    {
        InitializeComponent();
    }
}