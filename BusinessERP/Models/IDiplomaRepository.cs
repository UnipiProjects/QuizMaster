using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessERP.Models
{
    public interface IDiplomaRepository
    {
        Diploma GetDiploma(int Id);
        IEnumerable<Diploma> GetAllDiplomas();
        Diploma Add(Diploma diploma);
        Diploma Update(Diploma diplomaChanges);
        Diploma Delete(int id);
    }
}
