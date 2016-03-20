using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSServicePoster
{
    public class RC4
    {
        const int S_LENGTH = 256;

        private byte[] K;
        private byte[] S;

        public byte[] State
        {
            get { return this.S; }
        }

        public RC4(String key)
        {
            this.K = StringToByteArray(key);
            KeySchedule();
        }

        public RC4(byte[] key)
        {
            this.K = key;
            KeySchedule();
        }

        private void KeySchedule()
        {
            this.S = new byte[S_LENGTH];

            for (int i = 0; i < S_LENGTH; i++)
            {
                this.S[i] = (byte)i;
            }

            int j = 0;

            for (int i = 0; i < S_LENGTH; i++)
            {
                j = (j + this.K[i % K.Length] + this.S[i]) % S_LENGTH;
                this.Swap(i, j);
            }
        }

        private int i = 0;
        private int j = 0;

        public byte Loop()
        {
            i = (i + 1) % S_LENGTH;
            j = (j + S[i]) % S_LENGTH;

            this.Swap(i, j);

            byte z = this.S[(this.S[i] + this.S[j]) % S_LENGTH];

            return z;
        }

        public void Skip(long length)
        {
            for (int i = 0; i < length; i++)
            {
                this.Loop();
            }
        }

        public void EncryptInPlace(byte[] input)
        {
            EncryptInPlace(input, 0, input.Length);
        }

        public void EncryptInPlace(byte[] input, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                byte result = (byte)(this.Loop() ^ input[i]);
                input[i] = result;
            }
        }

        private void Swap(int i, int j)
        {
            byte tmp = this.S[i];
            this.S[i] = this.S[j];
            this.S[j] = tmp;
        }

        private static byte[] StringToByteArray(String input)
        {
            Encoding enc_default = Encoding.UTF8;
            byte[] byteArray = enc_default.GetBytes(input);
            return byteArray;
        }
    }
}
