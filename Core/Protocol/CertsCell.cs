using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;

namespace Core.Protocol
{
    public class Certificate
    {
        public int CertType { get; set; }
        public byte[] CertBytes { get; set; }
    }

    public class CertsCell : Cell
    {
        public IEnumerable<Certificate> Certificates { get; private set; }

        public CertsCell(IEnumerable<Certificate> certificates)
            : base(129)
        {
            if (certificates == null) throw new ArgumentNullException("certificates");

            Certificates = certificates;
        }

        public static CertsCell Parse(byte[] cellBytes)
        {
            var certificates = new List<Certificate>();
            var certsCount = cellBytes[0];

            for (int i = 0, index = 1; i < certsCount; i++)
            {
                var certType = cellBytes[index];
                var size = ByteHelper.ReadInt16(cellBytes[index + 1], cellBytes[index + 2]);
                var certBytes = new byte[size];
                Array.Copy(cellBytes, index + 3, certBytes, 0, certBytes.Length);
                index += 3 + certBytes.Length;

                var certificate = new Certificate
                                   {
                                       CertType = certType,
                                       CertBytes = certBytes
                                   };
                certificates.Add(certificate);
            }

            return new CertsCell(certificates);
        }

        protected override byte[] GetPayload()
        {
            var size = 1 + Certificates.Count()*3 + Certificates.Sum(certificate => certificate.CertBytes.Length);
            var payload = new byte[size];

            payload[0] = (byte) Certificates.Count();
            var index = 1;
            foreach (var certificate in Certificates)
            {
                payload[index] = (byte) certificate.CertType;
                var certSizeBytes = BitConverter.GetBytes((ushort)certificate.CertBytes.Length);
                
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(certSizeBytes);
                }

                Array.Copy(certSizeBytes, 0, payload, index + 1, certSizeBytes.Length);
                Array.Copy(certificate.CertBytes, 0, payload, index + 3, certificate.CertBytes.Length);

                index += 3 + certificate.CertBytes.Length;
            }

            return payload;
        }
    }
}
