using System;
using TradiesToolbox.ViewModels;
using TradiesToolbox.Views;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
// ClientDetailViewModel - Additional functionality for client management
using System.Collections.ObjectModel;
using System.Windows.Input;

public class ClientDetailViewModel : BaseViewModel
{
    private readonly ClientDatabase _clientDatabase = new();
    private readonly JobDatabase _jobDatabase = new();

    private Client _client;
    public Client Client
    {
        get => _client;
        set => SetProperty(ref _client, value);
    }

    private int _clientId;
    public int ClientId
    {
        get => _clientId;
        set
        {
            _clientId = value;
            LoadClientAsync(_clientId).ConfigureAwait(false);
        }
    }

    // Collection of client activities
    public ObservableCollection<ClientActivity> ClientActivity { get; } = new();

    // Commands for UI interaction
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CallCommand { get; }
    public ICommand EmailCommand { get; }
    public ICommand NewJobCommand { get; }
    public ICommand NewQuoteCommand { get; }
    public ICommand MapCommand { get; } // Added for address mapping

    public ClientDetailViewModel()
    {
        Title = "Client Details";
        EditCommand = new Command(OnEdit);
        DeleteCommand = new Command(OnDelete);
        CallCommand = new Command(OnCall);
        EmailCommand = new Command(OnEmail);
        NewJobCommand = new Command(OnNewJob);
        NewQuoteCommand = new Command(OnNewQuote);
        MapCommand = new Command(OnOpenMap);
    }

    // Load client data from database
    public async Task LoadClientAsync(int clientId)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            Client = _clientDatabase.GetClient(clientId);
            Title = Client?.Name ?? "Client Details";

            // Load client activity history
            LoadClientActivity();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Load client related activity (jobs, quotes, etc.)
    private void LoadClientActivity()
    {
        ClientActivity.Clear();

        if (Client?.Id > 0)
        {
            // Get jobs for this client
            var clientJobs = _jobDatabase.GetJobs()
                .Where(j => j.ClientID == Client.Id)
                .OrderByDescending(j => j.CreatedDate)
                .Take(5);

            // Add job activities
            foreach (var job in clientJobs)
            {
                ClientActivity.Add(new ClientActivity
                {
                    Description = $"Job: {job.Title}",
                    Date = job.CreatedDate,
                    Type = "Job",
                    Color = "#3E5C76" // Blue
                });
            }

            // Could expand this to include quotes, invoices, etc.
        }
    }

    // Navigation to edit client (stub)
    private async void OnEdit()
    {
        await Application.Current.MainPage.DisplayAlert("Edit", "Navigate to Edit Client Page", "OK");
        // Actual implementation would navigate to edit page
        // await Shell.Current.GoToAsync($"{nameof(EditClientPage)}?id={Client.Id}");
    }

    // Delete client after confirmation
    private async void OnDelete()
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Client",
            $"Are you sure you want to delete {Client.Name}? This will also delete all associated jobs and quotes.",
            "Yes", "No");

        if (confirm)
        {
            _clientDatabase.DeleteClient(Client.Id);
            await Shell.Current.GoToAsync("..");
        }
    }

    // Call client using device capabilities
    private async void OnCall()
    {
        try
        {
            if (string.IsNullOrEmpty(Client.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "No Phone Number",
                    "This client doesn't have a phone number.",
                    "OK");
                return;
            }

            // Use platform phone capabilities, if available
            // For now, just show a message
            await Application.Current.MainPage.DisplayAlert(
                "Call",
                $"Calling {Client.Phone}...",
                "OK");

            // Full implementation would use:
            // await Launcher.OpenAsync(new Uri($"tel:{Client.Phone}"));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                $"Unable to make call: {ex.Message}",
                "OK");
        }
    }

    // Email client using device capabilities
    private async void OnEmail()
    {
        try
        {
            if (string.IsNullOrEmpty(Client.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "No Email",
                    "This client doesn't have an email address.",
                    "OK");
                return;
            }

            // Use email capabilities if available
            // For now, just show a message
            await Application.Current.MainPage.DisplayAlert(
                "Email",
                $"Opening email for {Client.Email}...",
                "OK");

            // Full implementation would use:
            // await Launcher.OpenAsync(new Uri($"mailto:{Client.Email}"));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                $"Unable to send email: {ex.Message}",
                "OK");
        }
    }

    // Create a new job for this client
    private async void OnNewJob()
    {
        // Navigate to the Add Job page with client ID pre-filled
        await Shell.Current.GoToAsync($"{nameof(AddJobPage)}?clientId={Client.Id}");
    }

    // Create a new quote for this client
    private async void OnNewQuote()
    {
        // Navigate to the Add Quote page with client ID pre-filled
        await Shell.Current.GoToAsync($"{nameof(AddQuotePage)}?clientId={Client.Id}");
    }

    // Open map with client address
    private async void OnOpenMap()
    {
        try
        {
            if (string.IsNullOrEmpty(Client.Address))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "No Address",
                    "This client doesn't have an address.",
                    "OK");
                return;
            }

            // Use map capabilities if available
            // For now, just show a message
            await Application.Current.MainPage.DisplayAlert(
                "Map",
                $"Opening map for address: {Client.Address}",
                "OK");

            // Full implementation would use:
            // var encodedAddress = Uri.EscapeDataString(Client.Address);
            // await Launcher.OpenAsync(new Uri($"https://maps.google.com/?q={encodedAddress}"));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                $"Unable to open map: {ex.Message}",
                "OK");
        }
    }
}