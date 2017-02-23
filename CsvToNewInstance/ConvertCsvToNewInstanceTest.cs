using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CsvToNewInstance
{
    [TestClass]
    public class ConvertCsvToNewInstanceTest
    {
        [TestMethod]
        public void InputEachLine_No_Value()
        {
            var target = new ConvertCsvToNewInstance();
            target.ClassName = "A";
            target.InputEachLine("F1,F2");
            var actual = target.Result;

            var expected = "";
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void InputEachLine_Value()
        {
            var target = new ConvertCsvToNewInstance();
            target.ClassName = "A";
            target.InputEachLine("F1,F2");
            target.InputEachLine("\"f1\",\"f2\"");
            var actual = target.Result;

            var expected = "new A { F1 = \"f1\", F2 = \"f2\", },\n";
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void InputEachLine_Value_2_line()
        {
            var target = new ConvertCsvToNewInstance();
            target.ClassName = "A";
            target.InputEachLine("F1,F2");
            target.InputEachLine("\"f1\",\"f2\"");
            target.InputEachLine("\"f1\",\"f2\"");
            var actual = target.Result;

            var expected = "new A { F1 = \"f1\", F2 = \"f2\", },\nnew A { F1 = \"f1\", F2 = \"f2\", },\n";
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        [MyExpectedException(typeof(Exception), "第 3 行的欄位數量不一致，請重新確認 csv 欄位數量")]
        public void Fields_Count_not_equal_Values_Count()
        {
            var target = new ConvertCsvToNewInstance();
            target.ClassName = "A";
            target.InputEachLine("F1,F2");
            target.InputEachLine("\"f1\",\"f2\"");
            target.InputEachLine("\"f1\",\"f2\",f3");
            var actual = target.Result;
        }


        public sealed class MyExpectedException : ExpectedExceptionBaseAttribute
        {
            private Type _expectedExceptionType;
            private string _expectedExceptionMessage;

            public MyExpectedException(Type expectedExceptionType)
            {
                _expectedExceptionType = expectedExceptionType;
                _expectedExceptionMessage = string.Empty;
            }

            public MyExpectedException(Type expectedExceptionType, string expectedExceptionMessage)
            {
                _expectedExceptionType = expectedExceptionType;
                _expectedExceptionMessage = expectedExceptionMessage;
            }

            protected override void Verify(Exception exception)
            {
                Assert.IsNotNull(exception);

                Assert.IsInstanceOfType(exception, _expectedExceptionType, "Wrong type of exception was thrown.");

                if (!_expectedExceptionMessage.Length.Equals(0))
                {
                    Assert.AreEqual(_expectedExceptionMessage, exception.Message, "Wrong exception message was returned.");
                }
            }
        }
    }
}
