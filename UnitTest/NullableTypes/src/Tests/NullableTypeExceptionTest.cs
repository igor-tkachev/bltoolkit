//
// NullableTypes.Tests.NullableTypeExceptionTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 08-May-2003  Luca    Create
//

namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysIO = System.IO;
    using sysFmt = System.Runtime.Serialization.Formatters;

    
    [nu.TestFixture]
    public class NullableTypeExceptionTest {

        private string _message;
        private sys.Exception _exception;

        [nu.SetUp]
        public void SetUp() {
            _message = "this is an exception message for test pourpose!!!";
            _exception = new sys.Exception("Gino");
        }


        #region Constructor Tests - A#

        [nu.Test]
        public void ConstructorMessage() {
            try {
                throw new NullableTypeException(_message);
            }
            catch (NullableTypeException ne) {
                nua.AssertEquals("TestA#01", _message, ne.Message);
                nua.AssertEquals("TestA#02", null, ne.InnerException);
                return;
            }
        }


        [nu.Test]
        public void ConstructorMessageInnerException() {
            try {
                throw new NullableTypeException(_message, _exception);
            }
            catch (NullableTypeException ne) {
                nua.AssertEquals("TestA#11", _message, ne.Message);
                nua.AssertEquals("TestA#12", _exception, ne.InnerException);
                return;
            }
        }


        [nu.Test]
        public void SerializationMessage() {
            NullableTypeException ne = new NullableTypeException(_message);


            sysFmt.Soap.SoapFormatter serializer = 
                new sysFmt.Soap.SoapFormatter();
               
            using (sysIO.MemoryStream stream = new sysIO.MemoryStream()) {
                serializer.Serialize(stream, ne);

//                sysTxt.Decoder d = sysTxt.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sysIO.SeekOrigin.Begin); // Return stream to start
                NullableTypeException x = (NullableTypeException)serializer.Deserialize(stream);

                nua.AssertEquals("TestA#21", ne.Message, x.Message);
                nua.AssertEquals("TestA#22", ne.ToString(), x.ToString());
                nua.AssertEquals("TestA#23", ne.HelpLink, x.HelpLink);
                nua.AssertEquals("TestA#24", ne.Source, x.Source);
                nua.AssertEquals("TestA#25", ne.StackTrace, x.StackTrace);
                nua.AssertEquals("TestA#26", ne.TargetSite, x.TargetSite);

                nua.AssertNull("TestA#27", ne.InnerException);
                nua.AssertNull("TestA#28", x.InnerException);

                stream.Close();
            }          
        }


        [nu.Test]
        public void SerializationMessageInnerException() {
            NullableTypeException ne = new NullableTypeException(_message, _exception);

            sysFmt.Soap.SoapFormatter serializer = new sysFmt.Soap.SoapFormatter();
               
            using (sysIO.MemoryStream stream = new sysIO.MemoryStream()) {
                serializer.Serialize(stream, ne);

                //                sysTxt.Decoder d = sysTxt.Encoding.Default.GetDecoder();
                //                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
                //                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
                //                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sysIO.SeekOrigin.Begin); // Return stream to start
                NullableTypeException x = (NullableTypeException)serializer.Deserialize(stream);

                nua.AssertEquals("TestA#41", ne.Message, x.Message);
                nua.AssertEquals("TestA#42", ne.ToString(), x.ToString());
                nua.AssertEquals("TestA#43", ne.HelpLink, x.HelpLink);
                nua.AssertEquals("TestA#44", ne.Source, x.Source);
                nua.AssertEquals("TestA#45", ne.StackTrace, x.StackTrace);
                nua.AssertEquals("TestA#46", ne.TargetSite, x.TargetSite);
                nua.AssertEquals("TestA#47", ne.InnerException.Message, x.InnerException.Message);
                nua.AssertEquals("TestA#48", ne.InnerException.ToString(), x.InnerException.ToString());
                nua.AssertEquals("TestA#49", ne.InnerException.HelpLink, x.InnerException.HelpLink);
                nua.AssertEquals("TestA#50", ne.InnerException.Source, x.InnerException.Source);
                nua.AssertEquals("TestA#51", ne.InnerException.StackTrace, x.InnerException.StackTrace);
                nua.AssertEquals("TestA#52", ne.InnerException.TargetSite, x.InnerException.TargetSite);

                stream.Close();
            }
            




        }


        #endregion //Constructor Tests - A#

    }
}
