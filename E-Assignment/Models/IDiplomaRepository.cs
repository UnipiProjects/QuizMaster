using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Assignment.Models
{
    interface IDiplomaRepository
    {
        Diploma GetDiploma(int Id);
        IEnumerable<Diploma> GetAllDiplomas();
        Diploma Add(Diploma diploma);
    }
}
