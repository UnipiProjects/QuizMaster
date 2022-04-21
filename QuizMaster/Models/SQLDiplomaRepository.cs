using QuizMaster.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SQLDiplomaRepository : IDiplomaRepository
    {
        private readonly QuizMasterDbContext context;
        public SQLDiplomaRepository(QuizMasterDbContext context)
        {
            this.context = context;
        }
        public Diploma Add(Diploma diploma)
        {
            context.Diplomas.Add(diploma);
            context.SaveChanges();
            return diploma;
        }

        public Diploma Delete(int id)
        {
            Diploma diploma = context.Diplomas.Find(id);
            if(diploma != null)
            {
                context.Diplomas.Remove(diploma);
                context.SaveChanges();
            }
            return diploma;
        }

        public IEnumerable<Diploma> GetAllDiplomas()
        {
            return context.Diplomas;
        }

        public Diploma GetDiploma(int Id)
        {
            return context.Diplomas.Find(Id);
        }

        public Diploma Update(Diploma diplomaChanges)
        {
            var diploma = context.Diplomas.Attach(diplomaChanges);
            diploma.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return diplomaChanges;
        }
    }
}
