namespace Warcraft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Warcraft.Model;

    /// <summary>
    /// Stores all saved games and highscore.
    /// Can save games, and reload them.
    /// </summary>
    public interface IStorageRepo
    {
        /// <summary>
        /// Saves a game state to a file.
        /// </summary>
        /// <param name="model">Game state to save.</param>
        /// <param name="name">File name to save to.</param>
        void Save(GameModel model, string name);

        /// <summary>
        /// Loads a given game state.
        /// </summary>
        /// <param name="name">File name where the game is saved.</param>
        /// <returns>The reloaded game state.</returns>
        GameModel Load(string name);

        /// <summary>
        /// Gives the high scores in a descending order.
        /// </summary>
        /// <returns>High scores :).</returns>
        List<KeyValuePair<string, TimeSpan>> GetHighScores();
    }
}
