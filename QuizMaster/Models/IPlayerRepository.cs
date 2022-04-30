using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public interface IPlayerRepository
    {
        Player GetPlayer(string Id);
        IEnumerable<Player> GetAllDiplomas();
        Player Add(Player player);
        Player Update(Player playerChanges);
        Player Delete(string id);
    }
}
