using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Assignment.Models
{
    public class MockDiplomaRepository : IDiplomaRepository
    {
        private List<Diploma> _diplomaList;

        public Diploma Add(Diploma diploma)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Diploma> GetAllDiplomas()
        {
            throw new NotImplementedException();
        }

        public Diploma GetDiploma(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
