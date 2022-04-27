using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    interface IPlayerRepository
    {
        Player GetDiploma(int Id);
        IEnumerable<Player> GetAllDiplomas();
        Player Add(Player player);
        Player Update(Player playerChanges);
        Player Delete(int id);
    }
}
