namespace Warcraft.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Warcraft.Model;

    /// <summary>
    /// Does repo things.
    /// </summary>
    public class StorageRepo : IStorageRepo
    {
        /// <inheritdoc/>
        public List<KeyValuePair<string, TimeSpan>> GetHighScores()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public GameModel Load(string name)
        {
            return JsonConvert.DeserializeObject<GameModel>(File.ReadAllText(name));
        }

        /// <inheritdoc/>
        public void Save(GameModel model, string name)
        {
            string text = JsonConvert.SerializeObject(model);
            File.WriteAllText(name + ".json", text);
        }
    }
}
