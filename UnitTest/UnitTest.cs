using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Chess.View.Login;
using System.ComponentModel.Design;

namespace Chess
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void Validation1()
        {
            string login1 = "Oleg2004";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login1), true);
        }

        [TestMethod]
        public void Validation2()
        {
            string login2 = "a111";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login2), true);
        }

        [TestMethod]
        public void Validation3()
        {
            string login3 = "B00000000000000000000000000000";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login3), true);
        }

        [TestMethod]
        public void Validation4()
        {
            string login4 = "Andrew07";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login4), true);
        }

        [TestMethod]
        public void Validation5()
        {
            string login5 = "Yeg0r4";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login5), true);
        }

        [TestMethod]
        public void Validation6()
        {
            string login6 = "0Peter";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login6), false);
        }

        [TestMethod]
        public void Validation7()
        {
            string login7 = "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login7), false);
        }

        [TestMethod]
        public void Validation8()
        {
            string login8 = "Liam@";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login8), false);
        }

        [TestMethod]
        public void Validation9()
        {
            string login9 = "Victor_";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login9), false);
        }

        [TestMethod]
        public void Validation10()
        {
            string login10 = "Jace-";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login10), false);
        }

        [TestMethod]
        public void Validation11()
        {
            string login11 = "Pi%rre";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login11), false);
        }

        [TestMethod]
        public void Validation12()
        {
            string login12 = "S$crooge";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login12), false);
        }

        [TestMethod]
        public void Validation13()
        {
            string login13 = "Alekey Semyonov";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login13), false);
        }

        [TestMethod]
        public void Validation14()
        {
            string login14 = "Русский2005";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login14), false);
        }

        [TestMethod]
        public void Validation15()
        {
            string login15 = "РeterAbramov";
            Assert.AreEqual(Validator.RegisterSymbolsValid(login15), false);
        }
    }
}
