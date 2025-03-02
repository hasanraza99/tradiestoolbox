using TradiesToolbox.ViewModels;
namespace TradiesToolbox.Views;

public partial class LoginPage : ContentPage
{
	private LoginViewModel _viewModel;

	public LoginPage()
	{
		InitializeComponent();

        _viewModel = new LoginViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialize();
    }
}