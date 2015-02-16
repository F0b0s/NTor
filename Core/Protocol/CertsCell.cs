using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Protocol
{
    public class Certificate
    {
        public int CertType { get; set; }
        public byte[] CertBytes { get; set; }
    }

    public class CertsCell : Cell
    {
        public IEnumerable<Certificate> Certificates { get; set; }

        public CertsCell(IEnumerable<Certificate> certificates) : base(130)
        {
            Certificates = certificates;
        }

        protected override byte[] GetPayload()
        {
            throw new NotImplementedException();
        }
    }
}
