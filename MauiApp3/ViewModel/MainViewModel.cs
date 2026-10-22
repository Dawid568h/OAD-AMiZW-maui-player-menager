using MauiApp3.Commands;
using MauiApp3.Model;
using MauiApp3.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace MauiApp3.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GetPlayerByIdQueryHandler _getPlayerByIdQueryHandler;
        private readonly GetPlayersQueryHandler _getPlayersQueryHandler;
        private readonly AddPlayerCommandHandler _addPlayerCommandHandler;
        private readonly EditPlayerCommandHandler _editPlayerCommandHandler; // DODANE

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Player> Players { get; } = new();
        public string PlayerName { get; set; } = "";
        public string PlayerLevel { get; set; } = "";
        public string PlayerId { get; set; } = "";

        // Pola dla formularza edycji, aby nie mieszać ich z dodawaniem/szukaniem
        private string _editPlayerId = "";
        private string _editPlayerName = "";
        private string _editPlayerLevel = "";
        private string _searchResult = "";
        private string _editResult = ""; // Komunikaty błędów/sukcesu edycji

        public string EditPlayerId
        {
            get => _editPlayerId;
            set { _editPlayerId = value; OnPropertyChanged(); }
        }

        public string EditPlayerName
        {
            get => _editPlayerName;
            set { _editPlayerName = value; OnPropertyChanged(); }
        }

        public string EditPlayerLevel
        {
            get => _editPlayerLevel;
            set { _editPlayerLevel = value; OnPropertyChanged(); }
        }

        public string SearchResult
        {
            get => _searchResult;
            set { _searchResult = value; OnPropertyChanged(); }
        }

        public string EditResult
        {
            get => _editResult;
            set { _editResult = value; OnPropertyChanged(); }
        }

        public ICommand AddPlayerCommandButton { get; }
        public ICommand FindPlayerCommandButton { get; }
        public ICommand EditPlayerCommandButton { get; } // DODANE

        public MainViewModel(
            GetPlayerByIdQueryHandler getPlayerByIdQueryHandler,
            GetPlayersQueryHandler getPlayersQueryHandler,
            AddPlayerCommandHandler addPlayerCommandHandler,
            EditPlayerCommandHandler editPlayerCommandHandler) // DODANE
        {
            _getPlayerByIdQueryHandler = getPlayerByIdQueryHandler;
            _getPlayersQueryHandler = getPlayersQueryHandler;
            _addPlayerCommandHandler = addPlayerCommandHandler;
            _editPlayerCommandHandler = editPlayerCommandHandler; // DODANE

            AddPlayerCommandButton = new Command(AddPlayer);
            FindPlayerCommandButton = new Command(FindPlayer);
            EditPlayerCommandButton = new Command(EditPlayer); // DODANE

            LoadPlayers();
        }

        private void AddPlayer()
        {
            if (!int.TryParse(PlayerLevel, out int playerLevel))
            {
                return;
            }
            AddPlayerCommand command = new AddPlayerCommand(PlayerName, playerLevel);
            _addPlayerCommandHandler.Handle(command);

            LoadPlayers();
        }

        private void FindPlayer()
        {
            if (!int.TryParse(PlayerId, out int playerId))
            {
                SearchResult = "Niepoprawne ID";
                return;
            }

            GetPlayerByIdQuery query = new GetPlayerByIdQuery(playerId);
            Player? player = _getPlayerByIdQueryHandler.Handle(query);

            if (player == null)
            {
                SearchResult = "Nie znaleziono gracza";
                return;
            }

            SearchResult = $"{player.Name} - poziom {player.Level}";
        }

        // NOWA METODA - OBSŁUGA EDYCJI I WALIDACJA
        private void EditPlayer()
        {
            // 1. Obsługa pustego ID oraz tekstu zamiast ID
            if (string.IsNullOrWhiteSpace(EditPlayerId))
            {
                EditResult = "Błąd: Id gracza nie może być puste.";
                return;
            }

            if (!int.TryParse(EditPlayerId, out int id))
            {
                EditResult = "Błąd: Id gracza musi być liczbą.";
                return;
            }

            // 2. Obsługa pustej nazwy
            if (string.IsNullOrWhiteSpace(EditPlayerName))
            {
                EditResult = "Błąd: Nowa nazwa nie może być pusta.";
                return;
            }

            // 3. Obsługa tekstu zamiast poziomu
            if (string.IsNullOrWhiteSpace(EditPlayerLevel))
            {
                EditResult = "Błąd: Nowy poziom nie może być pusty.";
                return;
            }

            if (!int.TryParse(EditPlayerLevel, out int level))
            {
                EditResult = "Błąd: Poziom musi być liczbą całkowitą.";
                return;
            }

            // 4. Obsługa niepoprawnego poziomu (np. ujemny)
            if (level < 0)
            {
                EditResult = "Błąd: Poziom nie może być mniejszy od 0.";
                return;
            }

            // 5. Utworzenie komendy i przekazanie do handlera
            EditPlayerCommand command = new EditPlayerCommand(id, EditPlayerName.Trim(), level);
            bool success = _editPlayerCommandHandler.Handle(command);

            // 6. Obsługa nieistniejącego gracza
            if (!success)
            {
                EditResult = $"Błąd: Nie znaleziono gracza o ID {id}.";
                return;
            }

            EditResult = "Sukces: Dane gracza zostały zaktualizowane.";

            // 7. Odświeżenie listy graczy
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            GetPlayersQuery query = new GetPlayersQuery();
            List<Player> players = _getPlayersQueryHandler.Handle(query);

            Players.Clear();

            foreach (Player player in players)
            {
                Players.Add(player);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}