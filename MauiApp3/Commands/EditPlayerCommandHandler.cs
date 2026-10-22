using MauiApp3.Data;
using MauiApp3.Model;
using System;

namespace MauiApp3.Commands
{
    /**
     * nazwa klasy:
     * EditPlayerCommandHandler
     *
     * opis:
     * Handler odpowiedzialny za przetworzenie komendy edycji gracza i aktualizację jego danych w repozytorium.
     *
     * pola / właściwości:
     * _playerRepository, PlayerRepository, Instancja repozytorium zarządzającego danymi graczy
     *
     * autor:
     * Dawid Machul
     */
    public class EditPlayerCommandHandler
    {
        private readonly PlayerRepository _playerRepository;

        /**
         * nazwa funkcji:
         * EditPlayerCommandHandler
         *
         * parametry wejściowe:
         * playerRepository, PlayerRepository, Instancja repozytorium graczy dostarczana przez DI
         *
         * wartość zwracana:
         * brak, Konstruktor klasy handlera
         *
         * opis funkcji:
         * Inicjalizuje handler i przypisuje instancję repozytorium do prywatnego pola.
         *
         * autor:
         * Dawid Machul
         */
        public EditPlayerCommandHandler(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        /**
         * nazwa funkcji:
         * Handle
         *
         * parametry wejściowe:
         * command, EditPlayerCommand, Obiekt komendy zawierający nowe dane gracza
         *
         * wartość zwracana:
         * bool, Zwraca true jeśli edycja się powiodła lub false jeśli gracz o podanym ID nie istnieje
         *
         * opis funkcji:
         * Odnajduje gracza w bazie danych/repozytorium za pomocą podanego ID, a następnie aktualizuje jego nazwę oraz poziom.
         *
         * autor:
         * Dawid Machul
         */
        public bool Handle(EditPlayerCommand command)
        {
            // Szukamy gracza bezpośrednio we właściwości Players repozytorium
            var player = _playerRepository.Players.Find(p => p.Id == command.Id);

            if (player == null)
            {
                return false;
            }

            player.Name = command.Name;
            player.Level = command.Level;
            return true;
        }
    }
}
