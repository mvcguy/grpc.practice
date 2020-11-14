using CommonLibrary.Properties;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary
{
    public static class FakeKeyStore
    {
        public static AsymmetricSecurityKey key;

        static FakeKeyStore()
        {
            var props = RSA.Create();
            props.FromXmlString(Resources.RsaProps);
            key = new RsaSecurityKey(props);
        }
    }
}
