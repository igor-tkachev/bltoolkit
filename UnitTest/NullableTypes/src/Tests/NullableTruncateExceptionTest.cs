//
// NullableTypes.Tests.NullableTruncateExceptionTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 08-May-2003  Luca    Created
//

namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysIO = System.IO;
    using sysFmt = System.Runtime.Serialization.Formatters;

    [nu.TestFixture]
    public class NullableTruncateExceptionTest {

        private string _message;
        private sys.Exception _exception;

        [nu.SetUp]
        public void SetUp() {
            _message = "this is an exception message for test pourpose!!!";
            _exception = new sys.Exception("Petrocle!!!!!!!");
        }
        

        [nu.Test]
        public void ConstructorDefault() {
            try {
                throw new NullableTruncateException();
            }
            catch (NullableTruncateException ne) {
//#if DEBUG
                nua.AssertEquals("TestA#01", 
                    "Numeric arithmetic causes truncation.", 
                    ne.Message);
//#endif
                nua.AssertEquals("TestA#02", null, ne.InnerException);
                return;
            }
        }


        [nu.Test]
        public void ConstructorMessage() {
            try {
                throw new NullableTruncateException(_message);
            }
            catch (NullableTruncateException ne) {
                nua.AssertEquals("TestA#11", _message, ne.Message);
                nua.AssertEquals("TestA#12", null, ne.InnerException);
                return;
            }
        }


        [nu.Test]
        public void ConstructorMessageInnerException() {
            try {
                throw new NullableTruncateException(_message, _exception);
            }
            catch (NullableTruncateException ne) {
                nua.AssertEquals("TestA#21", _message, ne.Message);
                nua.AssertEquals("TestA#22", _exception, ne.InnerException);
                return;
            }
        }


        [nu.Test]
        public void SerializationMessage() {
            NullableTruncateException ne = new NullableTruncateException(_message);


            sysFmt.Soap.SoapFormatter serializer = 
                new sysFmt.Soap.SoapFormatter();
               
            using (sysIO.MemoryStream stream = new sysIO.MemoryStream()) {
                serializer.Serialize(stream, ne);

                //                sysTxt.Decoder d = sysTxt.Encoding.Default.GetDecoder();
                //                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
                //                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
                //                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sysIO.SeekOrigin.Begin); // Return stream to start
                NullableTruncateException x = (NullableTruncateException)serializer.Deserialize(stream);

                nua.AssertEquals("TestA#31", ne.Message, x.Message);
                nua.AssertEquals("TestA#32", ne.ToString(), x.ToString());
                nua.AssertEquals("TestA#33", ne.HelpLink, x.HelpLink);
                nua.AssertEquals("TestA#34", ne.Source, x.Source);
                nua.AssertEquals("TestA#35", ne.StackTrace, x.StackTrace);
                nua.AssertEquals("TestA#36", ne.TargetSite, x.TargetSite);

                nua.AssertNull("TestA#37", ne.InnerException);
                nua.AssertNull("TestA#38", x.InnerException);

                stream.Close();
            }          
        }


        [nu.Test]
        public void SerializationMessageInnerException() {
            NullableTruncateException ne = new NullableTruncateException(_message, _exception);

            sysFmt.Soap.SoapFormatter serializer = new sysFmt.Soap.SoapFormatter();
               
            using (sysIO.MemoryStream stream = new sysIO.MemoryStream()) {
                serializer.Serialize(stream, ne);

                //                sysTxt.Decoder d = sysTxt.Encoding.Default.GetDecoder();
                //                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
                //                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
                //                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sysIO.SeekOrigin.Begin); // Return stream to start
                NullableTruncateException x = (NullableTruncateException)serializer.Deserialize(stream);

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

    }
}
