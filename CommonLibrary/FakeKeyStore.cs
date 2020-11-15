using CommonLibrary.Properties;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CommonLibrary
{
    public static class FakeKeyStore
    {
        public static AsymmetricSecurityKey Key;

        static FakeKeyStore()
        {
            var props = RSA.Create();
            props.FromXmlString(Resources.RsaProps);
            Key = new RsaSecurityKey(props);
        }
    }
}
