using System;

namespace MauiApp3.Commands
{
    /**
     * nazwa klasy:
     * EditPlayerCommand
     *
     * opis:
     * Klasa reprezentująca komendę edycji danych gracza, przechowująca nowe wartości pól.
     *
     * pola / właściwości:
     * Id, int, Unikalny identyfikator edytowanego gracza
     * Name, string, Nowa nazwa, która ma zostać przypisana graczowi
     * Level, int, Nowy poziom doświadczenia, który ma zostać przypisany graczowi
     *
     * autor:
     * Dawid machul
     */
    public class EditPlayerCommand
    {
        public int Id { get; }
        public string Name { get; }
        public int Level { get; }

        /**
         * nazwa funkcji:
         * EditPlayerCommand
         *
         * parametry wejściowe:
         * id, int, Unikalny identyfikator gracza
         * name, string, Nowa nazwa gracza
         * level, int, Nowy poziom gracza
         *
         * wartość zwracana:
         * brak, Konstruktor klasy komendy
         *
         * opis funkcji:
         * Inicjalizuje nową instancję klasy EditPlayerCommand z podanymi parametrami.
         *
         * autor:
         * Dawid machul
         */
        public EditPlayerCommand(int id, string name, int level)
        {
            Id = id;
            Name = name;
            Level = level;
        }
    }
}
