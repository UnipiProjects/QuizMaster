using QuizMaster.Data;
using QuizMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SQLPlayerRepository : IPlayerRepository
    {
        private readonly QuizMasterDbContext context;
        public SQLPlayerRepository(QuizMasterDbContext context)
        {
            this.context = context;
        }
        public Player Add(Player player)
        {
            context.Players.Add(player);
            context.SaveChanges();
            return player;
        }

        public Player Delete(string id)
        {
            Player player = context.Players.Find(id);
            if(player != null)
            {
                context.Players.Remove(player);
                context.SaveChanges();
            }
            return player;
        }

        public IEnumerable<PlayerViewModel> GetAllPlayers()
        {
            var query = context.Players.Join(
                context.Users,
                player => player.Id,
                user => user.Id,
                (player, user) => new PlayerViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    Score = player.Score,
                    Questions = player.Questions,
                    Rank = player.Rank
                });
            return query;
        }

        public Player GetPlayer(string id)
        {
            return context.Players.Find(id);
        }

        public Player Update(Player playerChanges)
        {
            var player = context.Players.Attach(playerChanges);
            player.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return playerChanges;
        }
    }
}
