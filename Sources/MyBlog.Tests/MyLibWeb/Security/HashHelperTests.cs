using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib.Web.Security;

namespace MyBlog.Tests
{
    [TestClass]
    public class HashHelperTests
    {
        [TestMethod]
        public void NewHashTest()
        {
            // Password to hash
            String passwordToHash = "MyPassword|1234";

            // Hash
            HashResult result = HashHelper.Hash(passwordToHash);

            // Tests
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Hash));
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Salt));
            Assert.AreNotEqual(result.Hash, passwordToHash);
            // Data
            Debug.WriteLine($"Hash = {result.Hash}");
            Debug.WriteLine($"Salt = {result.Salt}");
        }

        [TestMethod]
        public void ExistingHashTest()
        {
            // Password to hash
            String passwordToHash = "911Me78@Toto.lan";

            // Hash
            HashResult result = HashHelper.Hash(passwordToHash);

            // Second hash with the Salt
            String actual = HashHelper.Hash(passwordToHash, result.Salt);

            // Tests
            String expected = result.Hash;
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.Salt));
            Assert.AreEqual(expected, actual);
            // Data
            Debug.WriteLine($"Hash = {result.Hash}");
            Debug.WriteLine($"Salt = {result.Salt}");
        }

    }
}
