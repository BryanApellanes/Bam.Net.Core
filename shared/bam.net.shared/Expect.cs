/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Bam.Net
{
    /// <summary>
    /// A utility for making assertions
    /// </summary>
    public static class Expect
    {
        public static bool ShouldHtmlEncodeExceptions { get; set; }

        public static void ShouldBeTrue(this bool boolToCheck, string failureMessage = null)
        {
            IsTrue(boolToCheck, failureMessage ?? "Expected <true>, Actual <false>");
        }

        public static void IsTrue(this bool boolToCheck, string failureMessage = "Expected <true>, Actual <false>") 
        {
            if (!boolToCheck)
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    throw new ExpectationFailedException(true, boolToCheck, ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }

        public static void FileExists(string filePath, string failureMessage = "File not found.")
        {
            Expect.IsTrue(File.Exists(filePath), failureMessage);
        }

        public static void ShouldBeFalse(this bool boolToCheck, string failureMessage = null)
        {
            IsFalse(boolToCheck, failureMessage);
        }
        
        public static void IsFalse(this bool boolToCheck)
        {
            IsFalse(boolToCheck, string.Empty);
        }

        public static void IsFalse(this bool boolToCheck, string failureMessage)
        {
            if (boolToCheck)
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    throw new ExpectationFailedException(false, boolToCheck, ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }

        public static T CanCast<T>(object instance, string failureMessage = null)
        {
            try
            {
                return (T)instance;
            }
            catch (Exception ex)
            {
                string o = instance == null ? "[null]" : instance.GetType().Name;
                string exceptionMessage = failureMessage == null ? $"Couldn't cast object {o} to type {typeof(T).Name}: {ex.Message}" : $"{failureMessage}: {ex.Message}";
                throw new ExpectationFailedException(exceptionMessage);
            }
        }

        /// <summary>
        /// Executes the specified actionThatThrowsException action passing the exception to the specified 
        /// catchDelegate and throws an ExpectFailedException if the actionThatThrowsException doesn't
        /// throw an Exception
        /// </summary>
        /// <param name="actionThatThrowsException"></param>
        /// <param name="failureMessage"></param>
        public static void Throws(Action actionThatThrowsException, string failureMessage = null)
        {
            Throws(actionThatThrowsException, null, failureMessage);
        }

        /// <summary>
        /// Executes the specified actionThatThrowsException action passing the exception to the specified 
        /// catchDelegate and throws an ExpectFailedException if the actionThatThrowsException doesn't
        /// throw an Exception
        /// </summary>
        /// <param name="actionThatThrowsException"></param>
        /// <param name="catchDelegate"></param>
        /// <param name="failureMessage"></param>
        public static void Throws(Action actionThatThrowsException, Action<Exception> catchDelegate = null, string failureMessage = null)
        {
            catchDelegate = catchDelegate ?? ((e) => { });
            bool thrown = false;
            try
            {
                actionThatThrowsException();
            }
            catch (Exception ex)
            {
                catchDelegate(ex);
                thrown = true;
            }

            if (!thrown)
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    failureMessage = "Exception was not thrown";
                }

                throw new ExpectationFailedException(failureMessage);
            }
        }

        /// <summary>
        /// Checks if the specified "left" value is greater than the specified "right" value.
        /// </summary>
        /// <param name="left">int on the left of &gt;</param>
        /// <param name="right">int on the right of &gt;</param>
        public static void IsGreaterThan(int left, int right)
        {
            IsGreaterThan((long)left, (long)right);
        }

        /// <summary>
        /// Checks if the specified "left" value is greater than the specified "right" value.
        /// </summary>
        /// <param name="left">int on the left of &gt;</param>
        /// <param name="right">int on the right of &gt;</param>
        public static void IsGreaterThan(long left, long right)
        {
            IsGreaterThan(left, right, string.Format("{0} is not greater than {1}", left, right));
        }

        /// <summary>
        /// Checks if the specified "left" value is greater than the specified "right" value.
        /// </summary>
        /// <param name="left">int on the left of &gt;</param>
        /// <param name="right">int on the right of &gt;</param>
        public static void IsGreaterThan(ulong left, ulong right)
        {
            IsGreaterThan(left, right, string.Format("{0} is not greater than {1}", left, right));
        }

        /// <summary>
        /// Checks if the specified "left" value is greater than the specified "right" value.
        /// </summary>
        /// <param name="left">int on the left of &gt;</param>
        /// <param name="right">int on the right of &gt;</param>
        public static void IsGreaterThan(long left, long right, string failureMessage)
        {
            if (!(left > right))
                throw new ExpectationFailedException(failureMessage);
        }

        /// <summary>
        /// Checks if the specified "left" value is greater than the specified "right" value.
        /// </summary>
        /// <param name="left">int on the left of &gt;</param>
        /// <param name="right">int on the right of &gt;</param>
        public static void IsGreaterThan(ulong left, ulong right, string failureMessage)
        {
            if (!(left > right))
                throw new ExpectationFailedException(failureMessage);
        }

        public static void IsGreaterThanOrEqualTo(int left, int right)
        {
            IsGreaterThanOrEqualTo(left, right, string.Format("{0} is not greater than or equal to {1}", left, right));
        }
        /// <summary>
        /// Checks if the specified "left" value is greater than or equal to the specified "right" value. 
        /// </summary>
        /// <param name="left">int on the left of &gt;=</param>
        /// <param name="right">int on the right of &gt;=</param>
        public static void IsGreaterThanOrEqualTo(int left, int right, string failureMessage)
        {
            if (!(left >= right))
                throw new ExpectationFailedException(failureMessage);
        }

        /// <summary>
        /// Checks if the specified objects are the same using == (!=).
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreSame(object expected, object actual)
        {
            AreSame(expected, actual, string.Empty);
        }
        
        /// <summary>
        /// Checks if the specified objects are the same using == (!=).
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreSame(object expected, object actual, string failureMessage)
        {
            if (expected != actual)
            {
                if (!string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException(failureMessage, ShouldHtmlEncodeExceptions);

                throw new ExpectationFailedException(expected.ToString(), actual.ToString(), ShouldHtmlEncodeExceptions);
            }
        }

        public static void ReferenceEquals(object one, object two)
        {
            ReferenceEquals(one, two, string.Empty);
        }

        public static void ReferenceEquals(object one, object two, string failureMessage)
        {
            if (!object.ReferenceEquals(one, two))
            {
                if (string.IsNullOrWhiteSpace(failureMessage))
                {
                    throw new ExpectationFailedException("References weren't equal");
                }

                throw new ExpectationFailedException(failureMessage, ShouldHtmlEncodeExceptions);
            }
        }
        
        public static void IsEqualTo(this int actual, int expected, string failureMessage = "")
        {
            AreEqual(expected, actual, failureMessage);
        }

        public static void AreEqual(int expected, int actual, string failureMessage = "")
        {
            if (expected != actual)
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    throw new ExpectationFailedException(expected.ToString(), actual.ToString(), ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }
        
        public static void IsEqualTo(this long actual, long expected)
        {
            AreEqual(expected, actual);
        }
        
        public static void AreEqual(long expected, long actual)
        {
            AreEqual(expected, actual, "");
        }

        public static void AreEqual(long expected, long actual, string failureMessage)
        {
            if (expected != actual)
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    throw new ExpectationFailedException(expected.ToString(), actual.ToString(), ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }

        /// <summary>
        /// Does an equality comparison using expected.Equals()
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreEqual(object expected, object actual)
        {
            AreEqual(expected, actual, "");
        }

        public static void IsEqualTo(this string actual, string expected, string failureMessage = "")
        {
            AreEqual(expected, actual, failureMessage);
        }
        
        /// <summary>
        /// Does an equality comparison using expected.Equals()
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void AreEqual(string expected, string actual)
        {
            AreEqual(expected, actual, "");
        }

        /// <summary>
        /// Does an equality comparison using expected.Equals()
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="failureMessage">The failureMessage to display if the comparison fails</param>
        public static void AreEqual(string expected, string actual, string failureMessage)
        {
            AreEqual((object)expected, (object)actual, failureMessage);
        }

        /// <summary>
        /// Checks if the specified objects are equal using the Equals() method.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreEqual(object expected, object actual, string failureMessage)
        {
            if (((expected == null && actual != null) ||
                (actual == null && expected != null)) ||
                (expected != null && !expected.Equals(actual))
                )
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    string expectString = expected == null ? "null" : expected.ToString();
                    string actualString = actual == null ? "null" : actual.ToString();
                    throw new ExpectationFailedException(expectString, actualString, ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }
		
        /// <summary>
        /// Throws an ExpectFailedException if the type doesn't 
        /// derive from the specified generic type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        public static void DerivesFromType<T>(this object objectToCheck)
        {
            DerivesFromType<T>(objectToCheck, string.Empty);
        }

        public static void DerivesFromType<T>(this object objectToCheck, string failureMessage)
        {
            Type checkType = objectToCheck.GetType();
            if (!checkType.IsSubclassOf(typeof(T)))
            {
                if (string.IsNullOrEmpty(failureMessage))
                {
                    throw new ExpectationFailedException(typeof(T), objectToCheck, ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }

        /// <summary>
        /// Asserts that the current instance is of the specified generic type.
        /// Throws an excpetion if the assertion fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        public static void IsObjectOfType<T>(this object objectToCheck)
        {
            IsObjectOfType<T>(objectToCheck, string.Empty);
        }

        /// <summary>
        /// Checks if the specified object is of type T using GetType().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        public static void IsObjectOfType<T>(this object objectToCheck, string failureMessage)
        {
            if (objectToCheck.GetType() != typeof(T))
            {
                if (string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException(typeof(T), objectToCheck, ShouldHtmlEncodeExceptions);
                else
                    throw new ExpectationFailedException(failureMessage);
            }
        }

        /// <summary>
        /// Asserts that the object is an instance of the specified generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        /// <param name="failureMessage"></param>
        public static void IsInstanceOfType<T>(this object objectToCheck, string failureMessage = "")
        {
            if (!typeof(T).IsInstanceOfType(objectToCheck))
            {
                if (string.IsNullOrWhiteSpace(failureMessage))
                {
                    throw new ExpectationFailedException(typeof(T), objectToCheck, ShouldHtmlEncodeExceptions);
                }
                else
                {
                    throw new ExpectationFailedException(failureMessage);
                }
            }
        }
        /// <summary>
        /// Asserts that the specified string is null or empty.  Throws
        /// an exception if the assertion fails.
        /// </summary>
        /// <param name="stringToCheck"></param>
        public static void IsNullOrEmpty(string stringToCheck)
        {
            IsNullOrEmpty(stringToCheck, string.Empty);
        }

        /// <summary>
        /// Asserts that the specified string is null or empty.  Throws
        /// an exception if the assertion fails.
        /// </summary>
        /// <param name="stringToCheck"></param>
        public static void IsNullOrEmpty(string stringToCheck, string failureMessage)
        {
            if (!string.IsNullOrEmpty(stringToCheck))
            {
                if (string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException("null or empty string", stringToCheck);
                else
                    throw new ExpectationFailedException(failureMessage);
            }
        }

        public static void IsNotNullOrEmpty(string stringToCheck)
        {
            IsNotNullOrEmpty(stringToCheck, "");
        }

        public static void IsNotNullOrEmpty(string stringToCheck, string failureMessage)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                if (string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException("string with value", "null or empty string");
                else
                    throw new ExpectationFailedException(failureMessage);
            }
        }

        /// <summary>
        /// Checks if the specified object extends type T using the "is" operator.  The same as Extends&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        public static void IsExtenderOfType<T>(object objectToCheck)
        {
            Extends<T>(objectToCheck);
        }

        /// <summary>
        /// Checks if the specified object extends type T using the "is" operator.
        /// </summary>
        /// <typeparam name="T">The type to be extended.</typeparam>
        /// <param name="objectToCheck">The object to check if it extends the specified type T.</param>
        public static void Extends<T>(object objectToCheck)
        {
            if (!(objectToCheck is T))
                throw new ExpectationFailedException($"{objectToCheck.GetType().Name} doesn't extend {typeof(T).Name}", ShouldHtmlEncodeExceptions);
        }

        public static void ShouldBeNull(this object objectToCheck, string failureMessage = null)
        {
            IsNull(objectToCheck, failureMessage);
        }
        
        public static void IsNull(this object objectToCheck)
        {
            IsNull(objectToCheck, "objectToCheck was not null as expected");
        }
        
        /// <summary>
        /// Throws an exception if the specified objectToCheck is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck"></param>
        /// <param name="failureMessage"></param>
        public static void IsNull(object objectToCheck, string failureMessage) 
        {
            if (objectToCheck != null)
            {
                if (!string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException(failureMessage);
                else
                    throw new ExpectationFailedException("null", objectToCheck.GetType().Name, ShouldHtmlEncodeExceptions);
            }
        }

        public static void ShouldNotBeNull(this object objectToCheck, string failureMessage)
        {
            IsNotNull(objectToCheck, failureMessage);
        }
        
        public static void IsNotNull(this object objectToCheck)
        {
            IsNotNull(objectToCheck, string.Empty);
        }

        public static void IsNotNull(object objectToCheck, string failureMessage) 
        {
            if (objectToCheck == null)
            {
                if (!string.IsNullOrEmpty(failureMessage))
                    throw new ExpectationFailedException(failureMessage);
                else
                    throw new ExpectationFailedException("object", "null", ShouldHtmlEncodeExceptions);
            }
        }   
        
        /// <summary>
        /// Throw an ExpectFailedException with the specified failureMessage
        /// </summary>
        /// <param name="failureMessage"></param>
        public static void Fail(string failureMessage = "Expect.Fail() was called to throw this exception.")
        {
            throw new ExpectationFailedException(failureMessage, ShouldHtmlEncodeExceptions);
        }
    }
}
